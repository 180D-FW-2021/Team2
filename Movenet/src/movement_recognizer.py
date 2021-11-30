from enums import Position as Pos, BodyPart as BP
from keypoints import Keypoints
import numpy as np
import time

"""
Recognize player obstacle-navigation movement (jump, duck)
based on body keypoint data
"""

"""
TODO:
- fine-tuning model for accuracy

Current Issues:
- moving up after duck sometimes gets recognized as jump (mostly resolved)
- slight duck before jump sometimes recognized as duck (mostly resolved)
- fast forwards movement sometimes recognized as jump
- fast backwards movement sometimes recognized as duck
- average more visible keypoints to avoid noise
- occasionally very fast duck does not return to stationary 
  (very unlikely player will be ducking quickly)

Future plans: 
- use x coord to differentiate between jump/duck and moving forward/backward quickly
- average over more visible body parts to reduce noise
"""


class MovementRecognizer:
    KEYPOINT_CONF_THRES = 0.3
    JUMP_RATE_THRES = 0.2
    DUCK_RATE_THRES = 0.2
    STATIONARY_THRES = 0.015

    def __init__(self, max_keypoints: int = 150) -> None:
        """
        Props:
          max_keypoints (int): max number of sequential keypoints to store
          keypoints_data (List[Keypoints]): last $max_keypoints keypoints
          position (Pos): current player position
          last_pos_update (float): time of last position update (secs)
          prev_coords (Keypoints): averaged keypoints before jump/duck
        """
        self.max_keypoints = max_keypoints
        self.keypoints_data = list()
        self.position = Pos.STATIONARY
        self.last_pos_update = time.time()
        self.prev_coords = None

    def add_and_recognize(self, keypoints: np.ndarray) -> Pos:
        """ Add keypoints to keypoints_data and recognize player movement
        Args:
          keypoints: predicted body keypoints of shape [17, 3]
        Returns:
          enum depicting recognized player position (jump/duck/stationary/out-of-frame)
        """
        self._add(keypoints)
        return self._recognize()

    def _add(self, keypoints: np.ndarray) -> None:
        """ Add new keypoint sample; ensure < $max_keypoint total samples
        Args:
          keypoints: predicted body keypoints of shape [17, 3]
        """
        self.keypoints_data.append(Keypoints(keypoints))
        if len(self.keypoints_data) > self.max_keypoints:
            self.keypoints_data.pop(0)

    def _set_prev_coords(self):
        """ Store averaged coords before jumping/ducking
            Used to determine if player has returned to stationary position
        """
        if len(self.keypoints_data) > 15:
            # ignore last 5 samples (begin jumping/ducking samples)
            samples = self.keypoints_data[-15:-5]
        else:
            samples = self.keypoints_data
        self.prev_coords = Keypoints(np.mean([s.keypoints() for s in samples], axis=0))

    def _recognize(self) -> Pos:
        """ Recognize player position
        Returns:
          Position enum depicting whether player is
          jumping, ducking, out-of-frame, or stationary
        """
        next_pos = self.position

        if self._player_out_of_frame():
            next_pos = Pos.OUT_OF_FRAME
        elif self.position == Pos.OUT_OF_FRAME:
            next_pos = Pos.OUT_FRAME_STATIONARY

        if self.position == Pos.STATIONARY:
            # avoid quick state update (< 0.3 seconds)
            # prevents jump registered after duck
            if time.time() - self.last_pos_update < 0.5:
                return self.position
            # check for jump/duck
            if self._jump_recognized():
                next_pos = Pos.JUMP_START
                self._set_prev_coords()
            elif self._duck_recognized():
                next_pos = Pos.DUCK_START
                self._set_prev_coords()

        if self.position == Pos.DUCK_START:
            next_pos = Pos.DUCKING
        elif self.position == Pos.JUMP_START:
            next_pos = Pos.JUMPING

        if self.position == Pos.JUMPING:
            if self._stationary_recognized():
                next_pos = Pos.STATIONARY
            # jump should not last longer than 0.7 s
            # prevent false positive never returning to stationary
            if time.time() - self.last_pos_update >= 0.7:
                next_pos = Pos.STATIONARY
        elif self.position == Pos.DUCKING:
            if self._stationary_recognized():
                next_pos = Pos.DUCK_STATIONARY
            # duck unlikely to last longer than 2 s
            # prevent false position never returning to stationary
            if time.time() - self.last_pos_update >= 2:
                next_pos = Pos.DUCK_STATIONARY

        if (
            self.position == Pos.DUCK_STATIONARY
            or self.position == Pos.OUT_FRAME_STATIONARY
        ):
            next_pos = Pos.STATIONARY

        if next_pos != self.position:
            self.position = next_pos
            self.last_pos_update = time.time()

        return self.position

    def _jump_recognized(self) -> bool:
        """ Determine if player has started jumping
        Current condition: 
          Left shoulder is 0.3 higher than 5th-last sample
          (and confidence is > 0.3)
        Returns:
          True if player is jumping; False o.w.
        """
        if len(self.keypoints_data) < 5:
            return False
        last_sample = self.keypoints_data[-1]
        fifth_last_sample = self.keypoints_data[-5]
        y1, _, c1 = last_sample.get(BP.LEFT_SHOULDER)
        y2, _, c2 = fifth_last_sample.get(BP.LEFT_SHOULDER)
        y3, _, c3 = last_sample.get(BP.LEFT_SHOULDER)
        y4, _, c4 = fifth_last_sample.get(BP.LEFT_SHOULDER)
        if any(conf < self.KEYPOINT_CONF_THRES for conf in [c1, c2, c3, c4]):
            return False
        return y2 - y1 >= self.JUMP_RATE_THRES and y4 - y3 >= self.JUMP_RATE_THRES

    def _duck_recognized(self) -> bool:
        """ Determine if player is ducking
        Current condition: 
          Left shoulder is 0.2 lower than 10-th last sample
          (and confience is > 0.3)
        Returns:
          True if player is ducking; False o.w.
        """
        if len(self.keypoints_data) < 10:
            return False
        last_sample = self.keypoints_data[-1]
        tenth_last_sample = self.keypoints_data[-10]
        # left shoulder
        y1, _, c1 = last_sample.get(BP.LEFT_SHOULDER)
        y2, _, c2 = tenth_last_sample.get(BP.LEFT_SHOULDER)
        # right shoulder
        y3, _, c3 = last_sample.get(BP.RIGHT_SHOULDER)
        y4, _, c4 = tenth_last_sample.get(BP.RIGHT_SHOULDER)
        if any(conf < self.KEYPOINT_CONF_THRES for conf in [c1, c2, c3, c4]):
            return False
        return y1 - y2 >= self.DUCK_RATE_THRES and y3 - y4 >= self.DUCK_RATE_THRES

    def _stationary_recognized(self):
        """Determine if player has returned to stationary after jump/duck
          Current Condition:
            Shoulder keypoints are within $threshold of keypoints before jump/duck
            (averaged over 10 samples in _set_prev_coords function)
        """
        ls_prev = self.prev_coords.get_y(BP.LEFT_SHOULDER)
        rs_prev = self.prev_coords.get_y(BP.RIGHT_SHOULDER)
        ls_curr = self.keypoints_data[-1].get_y(BP.LEFT_SHOULDER)
        rs_curr = self.keypoints_data[-1].get_y(BP.RIGHT_SHOULDER)
        return (
            abs(ls_prev - ls_curr) < self.STATIONARY_THRES
            and abs(rs_prev - rs_curr) < self.STATIONARY_THRES
        )

    def _player_out_of_frame(self) -> bool:
        """ Determine if player is out of frame
        Current condition: 
          Shoulders must be in frame within the last 5 samples
        Returns:
          True if player is out-of-frame; False o.w.
        """
        if len(self.keypoints_data) < 5:
            return False
        last_5_samples = self.keypoints_data[-5:]
        for sample in last_5_samples:
            shoulders_visible = sample.shoulders_visible()
            if shoulders_visible:
                return False
        return True
