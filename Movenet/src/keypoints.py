import numpy as np

keypoints_dict = {
    "nose": 0,
    "left_eye": 1,
    "right_eye": 2,
    "left_ear": 3,
    "right_ear": 4,
    "left_shoulder": 5,
    "right_shoulder": 6,
    "left_elbow": 7,
    "right_elbow": 8,
    "left_wrist": 9,
    "right_wrist": 10,
    "left_hip": 11,
    "right_hip": 12,
    "left_knee": 13,
    "right_knee": 14,
    "left_ankle": 15,
    "right_ankle": 16,
}


class Keypoints:
    def __init__(self, keypoints):
        # make keypoints tensor 1D
        self.keypoints = np.squeeze(keypoints)

    def get_keypoints(self):
        return self.keypoints

    def get_keypoint(self, body_part):
        if body_part not in keypoints_dict.keys():
            raise ValueError
        index = keypoints_dict[body_part]
        return self.keypoints[index]

    def get_ycoord(self, keypoint):
        return keypoint[0]

    def get_xcoord(self, keypoint):
        return keypoint[1]

    def get_conf(self, keypoint):
        return keypoint[2]
