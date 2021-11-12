from enum import Enum
from keypoints import Keypoints


class Position(Enum):
    JUMP = 1
    DUCK = 2
    OUT_OF_FRAME = 3
    STATIONARY = 4


class MovementRecognizer:
    def __init__(self, max_frames=150):
        self.max_frames = max_frames
        self.keypoints_data = list()

    def add_and_recognize(self, keypoints):
        self.add(keypoints)
        return self.recognize()

    def add(self, keypoints):
        k_obj = Keypoints(keypoints)
        self.keypoints_data.append(k_obj)
        if len(self.keypoints_data) > self.max_frames:
            self.keypoints_data.pop(0)

    # TODO: improve model beyond simple thresholding
    # - avoid sending detection to Unity multiple times
    # - code documentation

    def recognize(self):
        if self.player_out_of_frame():
            return Position.OUT_OF_FRAME
        elif self.jump_recognized():
            return Position.JUMP
        elif self.jump_recognized():
            return Position.DUCK
        else:
            return Position.STATIONARY

    def jump_recognized(self):
        last_sample = self.keypoints_data[-1]
        y, _, _ = last_sample.get_keypoint("left_hip")
        return y < 0.5

    def duck_recognized(self):
        last_sample = self.keypoints_data[-1]
        y, _, _ = last_sample.get_keypoint("left_shoulder")
        return y > 0.6

    def player_out_of_frame(self):
        # last 5 samples out of frame
        if len(self.keypoints_data) < 5:
            return False
        labels = ["left_hip", "right_hip", "left_shoulder", "right_shoulder"]
        last_5_samp = self.keypoints_data[-5:]
        for samp in last_5_samp:
            for l in labels:
                kp = samp.get_keypoint(l)
                if samp.get_conf(kp) > 0.4:
                    return False
        return True

