from one_euro_filter import OneEuroFilter
import datetime

"""
Apply Euro filter to keypoints (x, y coords) for smoothing

Reference: 
https://github.com/tensorflow/tfjs-models/blob/e7f6e120aa05c24bdbf9663358e44f082f76f345/shared/filters/keypoints_one_euro_filter.ts#L31
"""


def get_time_us():
    return datetime.datetime.now().microsecond


class KeypointsFilter:
    NUM_KEYPOINTS = 17

    def __init__(self):
        self.x_filters = []
        self.y_filters = []

    def init_filters(self):
        curr_time_us = get_time_us()

        while len(self.x_filters) < KeypointsFilter.NUM_KEYPOINTS:
            self.x_filters.append(OneEuroFilter(curr_time_us))

        while len(self.y_filters) < KeypointsFilter.NUM_KEYPOINTS:
            self.y_filters.append(OneEuroFilter(curr_time_us))

    def clear_filters(self):
        self.x_filters = []
        self.y_filters = []

    def apply_filter(self, keypoints):
        if (
            len(self.x_filters) < KeypointsFilter.NUM_KEYPOINTS
            or len(self.y_filters) < KeypointsFilter.NUM_KEYPOINTS
        ):
            self.init_filters()

        for i in range(len(keypoints)):
            keypoint = keypoints[i]
            y, x, conf = keypoint
            filt_x = self.x_filters[i].__call__(get_time_us(), x)
            filt_y = self.y_filters[i].__call__(get_time_us(), y)
            keypoints[i] = [filt_y, filt_x, conf]
        return keypoints
