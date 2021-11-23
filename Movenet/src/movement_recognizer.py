from enums import Position, BodyPart
import numpy as np
import time

"""
Recognize player obstacle-navigation movement (jump, duck)
based on body keypoint data
"""

"""
TODO:
- code cleanup and documentation
- fine-tuning model for accuracy

Current Issues:
- requires high ducking speed for recognition
- moving up after duck sometimes gets recognized as jump
- slight duck before jump sometimes recognized as duck
- fast player movement forwards sometimes recognized as jump
"""


class MovementRecognizer:
    def __init__(self, max_keypoints: int = 150) -> None:
        """
        Props:
          max_keypoints: max number of keypoints to store
          keypoints_data: list of last $max_keypoints keypoints
        """
        self.max_keypoints = max_keypoints
        self.keypoints_data = list()
        self.position = Position.STATIONARY
        self.last_pos_update = time.time()
        self.prev_hips = None
        self.prev_shoulders = None

    def add_and_recognize(self, keypoints: np.ndarray) -> Position:
        """ Add keypoints to keypoints_data and recognize player movement
        Args:
          keypoints: predicted body keypoints of shape [17, 3]
        Returns:
          enum depicting recognized player position (jump/duck/stationary/out-of-frame)
        """
        self._add(keypoints)
        return self._recognize()

    def _add(self, keypoints: np.ndarray) -> None:
        """ Add new keypoint sample; ensure < $max_keypoint samples
        Args:
          keypoints: predicted body keypoints of shape [17, 3]
        """
        self.keypoints_data.append(keypoints)
        if len(self.keypoints_data) > self.max_keypoints:
            self.keypoints_data.pop(0)

    def _set_prev_coords(self):
        if len(self.keypoints_data) > 10:
            samples = self.keypoints_data[-10:]
        else:
            samples = self.keypoints_data
        shoulders_avg = np.array([0.0, 0.0])
        hips_avg = np.array([0.0, 0.0])
        for s in samples:
            s_lshoulder = s[BodyPart.LEFT_SHOULDER.value][0]
            s_rshoulder = s[BodyPart.RIGHT_SHOULDER.value][0]
            shoulders_avg[0] += s_lshoulder
            shoulders_avg[1] += s_rshoulder
            s_lhip = s[BodyPart.LEFT_HIP.value][0]
            s_rhip = s[BodyPart.RIGHT_HIP.value][0]
            hips_avg[0] += s_lhip
            hips_avg[1] += s_rhip
        self.prev_hips = hips_avg / len(samples)
        self.prev_shoulders = shoulders_avg / len(samples)

    def _within_threshold(self, threshold=0.05):
        # lh_prev, rh_prev = self.prev_hips
        ls_prev, rs_prev = self.prev_shoulders
        ls_curr = self.keypoints_data[-1][BodyPart.LEFT_SHOULDER.value][0]
        rs_curr = self.keypoints_data[-1][BodyPart.RIGHT_SHOULDER.value][0]
        return abs(ls_prev - ls_curr) < threshold and abs(rs_prev - rs_curr) < threshold

    def _recognize(self) -> Position:
        """ Recognize player position
        Returns:
          Position enum depicting whether player is
          jumping, ducking, out-of-frame, or stationary
        """
        next_pos = self.position

        if self._player_out_of_frame():
            next_pos = Position.OUT_OF_FRAME
        elif self.position == Position.OUT_OF_FRAME:
            next_pos = Position.STATIONARY

        if self.position == Position.STATIONARY:
            if time.time() - self.last_pos_update < 0.3:
                return self.position
            if self._jump_recognized():
                next_pos = Position.JUMP_START
                self._set_prev_coords()
            elif self._duck_recognized():
                next_pos = Position.DUCK_START
                self._set_prev_coords()
        elif self.position == Position.DUCK_START:
            next_pos = Position.DUCKING
        elif self.position == Position.JUMP_START:
            next_pos = Position.JUMPING
        elif self.position == Position.JUMPING:
            if self._within_threshold():
                next_pos = Position.STATIONARY
            if time.time() - self.last_pos_update >= 0.7:
                next_pos = Position.STATIONARY
        elif self.position == Position.DUCKING:
            if self._within_threshold():
                next_pos = Position.STATIONARY
            if time.time() - self.last_pos_update >= 5:
                next_pos = Position.STATIONARY
        if next_pos != self.position:
            self.position = next_pos
            self.last_pos_update = time.time()

        return self.position

    def _jump_recognized(self) -> bool:
        """ Determine if player is jumping
        Current condition: 
          left shoulder keypoint is on top half of frame
        Returns:
          True if player is jumping; False o.w.
        """
        if len(self.keypoints_data) < 5:
            return False
        last_sample = self.keypoints_data[-1]
        fifth_last_sample = self.keypoints_data[-5]
        y1, _, c1 = last_sample[BodyPart.LEFT_SHOULDER.value]
        y2, _, c2 = fifth_last_sample[BodyPart.LEFT_SHOULDER.value]
        if c1 < 0.3 or c2 < 0.3:
            return False
        return y2 - y1 >= 0.1

    def _duck_recognized(self) -> bool:
        """ Determine if player is ducking
        Current condition: 
          left shoulder keypoint is on bottom 40% of frame
        Returns:
          True if player is ducking; False o.w.
        """
        if len(self.keypoints_data) < 5:
            return False
        last_sample = self.keypoints_data[-1]
        fifth_last_sample = self.keypoints_data[-5]
        y1, _, c1 = last_sample[BodyPart.LEFT_SHOULDER.value]
        y2, _, c2 = fifth_last_sample[BodyPart.LEFT_SHOULDER.value]
        if c1 < 0.3 or c2 < 0.3:
            return False
        return y1 - y2 >= 0.1

    def _duck_up_recognized(self) -> bool:
        if len(self.keypoints_data) < 5:
            return False
        last_sample = self.keypoints_data[-1]
        fifth_last_sample = self.keypoints_data[-5]
        y1, _, c1 = last_sample[BodyPart.LEFT_SHOULDER.value]
        y2, _, c2 = fifth_last_sample[BodyPart.LEFT_SHOULDER.value]
        if c1 < 0.3 or c2 < 0.3:
            return False
        return y1 - y2 <= 0.04

    def _player_out_of_frame(self) -> bool:
        """ Determine if player is out of frame
        Current condition: 
          shoulders or hips must be in frame within the last 3 samples
        Returns:
          True if player is out-of-frame; False o.w.
        """
        if len(self.keypoints_data) < 10:
            return False
        last_5_samples = self.keypoints_data[-5:]
        for sample in last_5_samples:
            shoulders_visible = self._shoulders_visible(sample)
            if shoulders_visible:
                return False
        return True

    def _hips_visible(self, keypoint: np.ndarray, conf_threshold: float = 0.3) -> bool:
        left_hip_visible = self._body_part_visible(
            keypoint, BodyPart.LEFT_HIP, conf_threshold
        )
        right_hip_visible = self._body_part_visible(
            keypoint, BodyPart.RIGHT_HIP, conf_threshold
        )
        return left_hip_visible and right_hip_visible

    def _shoulders_visible(
        self, keypoint: np.ndarray, conf_threshold: float = 0.3
    ) -> bool:
        left_shoulder_visible = self._body_part_visible(
            keypoint, BodyPart.LEFT_SHOULDER, conf_threshold
        )
        right_shoulder_visible = self._body_part_visible(
            keypoint, BodyPart.RIGHT_SHOULDER, conf_threshold
        )
        return left_shoulder_visible and right_shoulder_visible

    def _body_part_visible(
        self, keypoint: np.ndarray, body_part: BodyPart, conf_threshold: float = 0.3
    ) -> bool:
        _, _, conf = keypoint[body_part.value]
        return conf >= conf_threshold