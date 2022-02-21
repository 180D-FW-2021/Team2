from enum import Enum

"""importable enums used across scripts/classes"""

# mapping from body part index in keypoints nparray
# e.g. nose = keypoints[BodyPart.NOSE.VAL]
class BodyPart(Enum):
    NOSE = 0
    LEFT_EYE = 1
    RIGHT_EYE = 2
    LEFT_EAR = 3
    RIGHT_EAR = 4
    LEFT_SHOULDER = 5
    RIGHT_SHOULDER = 6
    LEFT_ELBOW = 7
    RIGHT_ELBOW = 8
    LEFT_WRIST = 9
    RIGHT_WRIST = 10
    LEFT_HIP = 11
    RIGHT_HIP = 12
    LEFT_KNEE = 13
    RIGHT_KNEE = 14
    LEFT_ANKLE = 15
    RIGHT_ANKLE = 16

# detected player position
# state machine for updating position
class Position(Enum):
    # only send JUMP/DUCK to mqtt during start
    JUMP_START = 1 
    JUMPING = 2
    DUCK_START = 4
    DUCKING = 5
    OUT_OF_FRAME = 6
    STATIONARY = 7
    # transition from duck to stationary (tell Unity to stop ducking)
    DUCK_STATIONARY = 8
    # transition from out-of-frame to stationary
    # (tell Unity to stop out-of-frame message)
    OUT_FRAME_STATIONARY = 9
    GAME_START = 10
