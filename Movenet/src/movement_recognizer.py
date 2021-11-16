from enums import Position, BodyPart


class MovementRecognizer:
    def __init__(self, max_frames=150):
        self.max_frames = max_frames
        self.keypoints_data = list()

    def add_and_recognize(self, keypoints):
        self.add(keypoints)
        return self.recognize()

    def add(self, keypoints):
        self.keypoints_data.append(keypoints)
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
        y, _, _ = last_sample[BodyPart.LEFT_SHOULDER.value]
        return y < 0.5

    def duck_recognized(self):
        last_sample = self.keypoints_data[-1]
        y, _, _ = last_sample[BodyPart.LEFT_SHOULDER.value]
        return y > 0.6

    def player_out_of_frame(self):
        # require at least one main keypoint in-frame in last 5 samples
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
