from enums import Position, BodyPart
import numpy as np

"""
Recognize player obstacle-navigation movement (jump, duck)
based on body keypoint data
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

    def add_and_recognize(self, keypoints: np.ndarray) -> Position:
        """ Add keypoints to keypoints_data and recognize player movement
        Args:
          keypoints: predicted body keypoints of shape [17, 3]
        Returns:
          enum depicting recognized player position (jump/duck/stationary/out-of-frame)
        """
        self.add(keypoints)
        return self.recognize()

    def add(self, keypoints: np.ndarray) -> None:
        """ Add new keypoint sample; ensure < $max_keypoint samples
        Args:
          keypoints: predicted body keypoints of shape [17, 3]
        """
        self.keypoints_data.append(keypoints)
        if len(self.keypoints_data) > self.max_keypoints:
            self.keypoints_data.pop(0)

    # TODO: improve model beyond simple thresholding
    # - avoid sending detection to Unity multiple times
    # - code documentation

    def recognize(self) -> Position:
        """ Recognize player position
        Returns:
          Position enum depicting whether player is
          jumping, ducking, out-of-frame, or stationary
        """
        if self.player_out_of_frame():
            return Position.OUT_OF_FRAME
        elif self.jump_recognized():
            return Position.JUMP
        elif self.jump_recognized():
            return Position.DUCK
        else:
            return Position.STATIONARY

    def jump_recognized(self) -> bool:
        """ Determine if player is jumping
        Current condition: 
          left shoulder keypoint is on top half of frame
        Returns:
          True if player is jumping; False o.w.
        """
        last_sample = self.keypoints_data[-1]
        y, _, _ = last_sample[BodyPart.LEFT_SHOULDER.value]
        return y < 0.5

    def duck_recognized(self) -> bool:
        """ Determine if player is ducking
        Current condition: 
          left shoulder keypoint is on bottom 40% of frame
        Returns:
          True if player is ducking; False o.w.
        """
        last_sample = self.keypoints_data[-1]
        y, _, _ = last_sample[BodyPart.LEFT_SHOULDER.value]
        return y > 0.6

    def player_out_of_frame(self) -> bool:
        """ Determine if player is jumping
        Current condition: 
          at least one main keypoint (shoulders, hips) is in 
          frame (conf_thres > 0.4) in the last 5 samples
        Returns:
          True if player is out-of-frame; False o.w.
        """
        if len(self.keypoints_data) < 5:
            return False
        labels = [
            BodyPart.LEFT_HIP.value,
            BodyPart.RIGHT_HIP.value,
            BodyPart.LEFT_SHOULDER.value,
            BodyPart.RIGHT_SHOULDER.value,
        ]
        last_5_samp = self.keypoints_data[-5:]
        for samp in last_5_samp:
            for l in labels:
                _, _, conf = samp[l]
                if conf > 0.4:
                    return False
        return True
