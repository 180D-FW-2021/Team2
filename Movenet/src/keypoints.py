import numpy as np
from enums import BodyPart

"""Class for encapsulating extracting body part keypoints"""


class Keypoints:
    def __init__(self, keypoints: np.ndarray) -> None:
        self._keypoints = keypoints

    def keypoints(self):
        return self._keypoints

    def get(self, body_part: BodyPart) -> np.ndarray:
        """get keypoint by body part"""
        return self._keypoints[body_part.value]

    def get_x(self, body_part: BodyPart) -> float:
        """get x coord"""
        return self.get(body_part)[1]

    def get_y(self, body_part: BodyPart) -> float:
        """get y coord"""
        return self.get(body_part)[0]

    def get_conf(self, body_part: BodyPart) -> float:
        """get confidence"""
        return self.get(body_part)[2]

    def head_visible(self, conf_threshold: float = 0.3) -> bool:
        left_eye_visible = self.body_part_visible(BodyPart.LEFT_EYE, conf_threshold)
        right_eye_visible = self.body_part_visible(BodyPart.RIGHT_EYE, conf_threshold)
        left_ear_visible = self.body_part_visible(BodyPart.LEFT_EAR, conf_threshold)
        right_ear_visible = self.body_part_visible(BodyPart.RIGHT_EAR, conf_threshold)
        return (
            left_eye_visible
            and right_eye_visible
            and left_ear_visible
            and right_ear_visible
        )

    def hips_visible(self, conf_threshold: float = 0.3) -> bool:
        left_hip_visible = self.body_part_visible(BodyPart.LEFT_HIP, conf_threshold)
        right_hip_visible = self.body_part_visible(BodyPart.RIGHT_HIP, conf_threshold)
        return left_hip_visible and right_hip_visible

    def shoulders_visible(self, conf_threshold: float = 0.3) -> bool:
        left_shoulder_visible = self.body_part_visible(
            BodyPart.LEFT_SHOULDER, conf_threshold
        )
        right_shoulder_visible = self.body_part_visible(
            BodyPart.RIGHT_SHOULDER, conf_threshold
        )
        return left_shoulder_visible and right_shoulder_visible

    def body_part_visible(
        self, body_part: BodyPart, conf_threshold: float = 0.3
    ) -> bool:
        _, _, conf = self._keypoints[body_part.value]
        return conf >= conf_threshold
