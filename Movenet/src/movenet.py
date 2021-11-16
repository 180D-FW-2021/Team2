# Copyright 2021 The TensorFlow Authors. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

"""
References: 
- Tensorflow TFLite MoveNet example code:
  https://github.com/tensorflow/examples/blob/master/lite/examples/pose_estimation/raspberry_pi/ml/movenet.py
- MoveNet example with drawing keypoint predictions:
  https://github.com/nicknochnack/MoveNetLightning

Modifications to TF code:
- functions to draw predictions on video frames
"""

from typing import Dict, List, Tuple

import cv2
from enums import BodyPart
import numpy as np
import tensorflow as tf


class Movenet(object):
    """A wrapper class for a Movenet TFLite pose estimation model."""

    # Configure how confidence the model should be on the detected keypoints to
    # proceed with using smart cropping logic.
    _MIN_CROP_KEYPOINT_SCORE = 0.2
    _TORSO_EXPANSION_RATIO = 1.9
    _BODY_EXPANSION_RATIO = 1.2

    # edges between body parts - i.e. (left_shoulder, right_shoulder)
    # used to draw predictions on image
    KEYPOINT_EDGES = [
        (0, 1),
        (0, 2),
        (1, 3),
        (2, 4),
        (5, 7),
        (7, 9),
        (6, 8),
        (8, 10),
        (5, 6),
        (5, 11),
        (6, 12),
        (11, 12),
        (11, 13),
        (13, 15),
        (12, 14),
        (14, 16),
    ]

    def __init__(self, model_name: str) -> None:
        """Initialize a MoveNet pose estimation model.
    Args:
      model_name: Name of the TFLite MoveNet model.
    """
        if model_name == "lightning":
            self._interpreter = tf.lite.Interpreter(
                model_path="../models/lite-model_movenet_singlepose_lightning_3.tflite",
                num_threads=4,
            )
        else:
            self._interpreter = tf.lite.Interpreter(
                "../models/lite-model_movenet_singlepose_thunder_3.tflite",
                num_threads=4,
            )

        self._interpreter.allocate_tensors()

        self._input_index = self._interpreter.get_input_details()[0]["index"]
        self._output_index = self._interpreter.get_output_details()[0]["index"]

        self._input_height = self._interpreter.get_input_details()[0]["shape"][1]
        self._input_width = self._interpreter.get_input_details()[0]["shape"][2]

        self._crop_region = None

    def init_crop_region(
        self, image_height: int, image_width: int
    ) -> Dict[(str, float)]:
        """Defines the default crop region.
    The function provides the initial crop region (pads the full image from
    both sides to make it a square image) when the algorithm cannot reliably
    determine the crop region from the previous frame.
    Args:
      image_height (int): The input image width
      image_width (int): The input image height
    Returns:
      crop_region (dict): The default crop region.
    """
        if image_width > image_height:
            x_min = 0.0
            box_width = 1.0
            # Pad the vertical dimension to become a square image.
            y_min = (image_height / 2 - image_width / 2) / image_height
            box_height = image_width / image_height
        else:
            y_min = 0.0
            box_height = 1.0
            # Pad the horizontal dimension to become a square image.
            x_min = (image_width / 2 - image_height / 2) / image_width
            box_width = image_height / image_width

        return {
            "y_min": y_min,
            "x_min": x_min,
            "y_max": y_min + box_height,
            "x_max": x_min + box_width,
            "height": box_height,
            "width": box_width,
        }

    def _torso_visible(self, keypoints: np.ndarray) -> bool:
        """Checks whether there are enough torso keypoints.
    This function checks whether the model is confident at predicting one of
    the shoulders/hips which is required to determine a good crop region.
    Args:
      keypoints: Detection result of Movenet model.
    Returns:
      True/False
    """
        left_hip_score = keypoints[BodyPart.LEFT_HIP.value, 2]
        right_hip_score = keypoints[BodyPart.RIGHT_HIP.value, 2]
        left_shoulder_score = keypoints[BodyPart.LEFT_SHOULDER.value, 2]
        right_shoulder_score = keypoints[BodyPart.RIGHT_SHOULDER.value, 2]

        left_hip_visible = left_hip_score > Movenet._MIN_CROP_KEYPOINT_SCORE
        right_hip_visible = right_hip_score > Movenet._MIN_CROP_KEYPOINT_SCORE
        left_shoulder_visible = left_shoulder_score > Movenet._MIN_CROP_KEYPOINT_SCORE
        right_shoulder_visible = right_shoulder_score > Movenet._MIN_CROP_KEYPOINT_SCORE

        return (left_hip_visible or right_hip_visible) and (
            left_shoulder_visible or right_shoulder_visible
        )

    def _determine_torso_and_body_range(
        self,
        keypoints: np.ndarray,
        target_keypoints: Dict[(str, float)],
        center_y: float,
        center_x: float,
    ) -> List[float]:
        """Calculates the maximum distance from each keypoints to the center.
    The function returns the maximum distances from the two sets of keypoints:
    full 17 keypoints and 4 torso keypoints. The returned information will
    be used to determine the crop size. See determine_crop_region for more
    details.
    Args:
      keypoints: Detection result of Movenet model.
      target_keypoints: The 4 torso keypoints.
      center_y (float): Vertical coordinate of the body center.
      center_x (float): Horizontal coordinate of the body center.
    Returns:
      The maximum distance from each keypoints to the center location.
    """
        torso_joints = [
            BodyPart.LEFT_SHOULDER,
            BodyPart.RIGHT_SHOULDER,
            BodyPart.LEFT_HIP,
            BodyPart.RIGHT_HIP,
        ]
        max_torso_yrange = 0.0
        max_torso_xrange = 0.0
        for joint in torso_joints:
            dist_y = abs(center_y - target_keypoints[joint][0])
            dist_x = abs(center_x - target_keypoints[joint][1])
            if dist_y > max_torso_yrange:
                max_torso_yrange = dist_y
            if dist_x > max_torso_xrange:
                max_torso_xrange = dist_x

        max_body_yrange = 0.0
        max_body_xrange = 0.0
        for idx in range(len(BodyPart)):
            if keypoints[BodyPart(idx).value, 2] < Movenet._MIN_CROP_KEYPOINT_SCORE:
                continue
            dist_y = abs(center_y - target_keypoints[joint][0])
            dist_x = abs(center_x - target_keypoints[joint][1])
            if dist_y > max_body_yrange:
                max_body_yrange = dist_y

            if dist_x > max_body_xrange:
                max_body_xrange = dist_x

        return [max_torso_yrange, max_torso_xrange, max_body_yrange, max_body_xrange]

    def _determine_crop_region(
        self, keypoints: np.ndarray, image_height: int, image_width: int
    ) -> Dict[(str, float)]:
        """Determines the region to crop the image for the model to run inference on.
    The algorithm uses the detected joints from the previous frame to
    estimate the square region that encloses the full body of the target
    person and centers at the midpoint of two hip joints. The crop size is
    determined by the distances between each joints and the center point.
    When the model is not confident with the four torso joint predictions,
    the function returns a default crop which is the full image padded to
    square.
    Args:
      keypoints: Detection result of Movenet model.
      image_height (int): The input image width
      image_width (int): The input image height
    Returns:
      crop_region (dict): The crop region to run inference on.
    """
        # Convert keypoint index to human-readable names.
        target_keypoints = {}
        for idx in range(len(BodyPart)):
            target_keypoints[BodyPart(idx)] = [
                keypoints[idx, 0] * image_height,
                keypoints[idx, 1] * image_width,
            ]

        # Calculate crop region if the torso is visible.
        if self._torso_visible(keypoints):
            center_y = (
                target_keypoints[BodyPart.LEFT_HIP][0]
                + target_keypoints[BodyPart.RIGHT_HIP][0]
            ) / 2
            center_x = (
                target_keypoints[BodyPart.LEFT_HIP][1]
                + target_keypoints[BodyPart.RIGHT_HIP][1]
            ) / 2

            (
                max_torso_yrange,
                max_torso_xrange,
                max_body_yrange,
                max_body_xrange,
            ) = self._determine_torso_and_body_range(
                keypoints, target_keypoints, center_y, center_x
            )

            crop_length_half = np.amax(
                [
                    max_torso_xrange * Movenet._TORSO_EXPANSION_RATIO,
                    max_torso_yrange * Movenet._TORSO_EXPANSION_RATIO,
                    max_body_yrange * Movenet._BODY_EXPANSION_RATIO,
                    max_body_xrange * Movenet._BODY_EXPANSION_RATIO,
                ]
            )

            # Adjust crop length so that it is still within the image border
            distances_to_border = np.array(
                [center_x, image_width - center_x, center_y, image_height - center_y]
            )
            crop_length_half = np.amin([crop_length_half, np.amax(distances_to_border)])

            # If the body is large enough, there's no need to apply cropping logic.
            if crop_length_half > max(image_width, image_height) / 2:
                return self.init_crop_region(image_height, image_width)
            # Calculate the crop region that nicely covers the full body.
            else:
                crop_length = crop_length_half * 2
            crop_corner = [center_y - crop_length_half, center_x - crop_length_half]
            return {
                "y_min": crop_corner[0] / image_height,
                "x_min": crop_corner[1] / image_width,
                "y_max": (crop_corner[0] + crop_length) / image_height,
                "x_max": (crop_corner[1] + crop_length) / image_width,
                "height": (crop_corner[0] + crop_length) / image_height
                - crop_corner[0] / image_height,
                "width": (crop_corner[1] + crop_length) / image_width
                - crop_corner[1] / image_width,
            }
        # Return the initial crop regsion if the torso isn't visible.
        else:
            return self.init_crop_region(image_height, image_width)

    def _crop_and_resize(
        self,
        image: np.ndarray,
        crop_region: Dict[(str, float)],
        crop_size: Tuple[int, int],
    ) -> np.ndarray:
        """Crops and resize the image to prepare for the model input."""
        y_min, x_min, y_max, x_max = [
            crop_region["y_min"],
            crop_region["x_min"],
            crop_region["y_max"],
            crop_region["x_max"],
        ]

        crop_top = int(0 if y_min < 0 else y_min * image.shape[0])
        crop_bottom = int(image.shape[0] if y_max >= 1 else y_max * image.shape[0])
        crop_left = int(0 if x_min < 0 else x_min * image.shape[1])
        crop_right = int(image.shape[1] if x_max >= 1 else x_max * image.shape[1])

        padding_top = int(0 - y_min * image.shape[0] if y_min < 0 else 0)
        padding_bottom = int((y_max - 1) * image.shape[0] if y_max >= 1 else 0)
        padding_left = int(0 - x_min * image.shape[1] if x_min < 0 else 0)
        padding_right = int((x_max - 1) * image.shape[1] if x_max >= 1 else 0)

        # Crop and resize image
        output_image = image[crop_top:crop_bottom, crop_left:crop_right]
        output_image = cv2.copyMakeBorder(
            output_image,
            padding_top,
            padding_bottom,
            padding_left,
            padding_right,
            cv2.BORDER_CONSTANT,
        )
        output_image = cv2.resize(output_image, (crop_size[0], crop_size[1]))

        return output_image

    def _run_detector(
        self,
        image: np.ndarray,
        crop_region: Dict[(str, float)],
        crop_size: Tuple[int, int],
    ) -> np.ndarray:
        """Runs model inference on the cropped region.
    The function runs the model inference on the cropped region and updates
    the model output to the original image coordinate system.
    Args:
      image: The input image.
      crop_region: The region of interest to run inference on.
      crop_size: The size of the crop region.
    Returns:
      An array of shape [17, 3] representing the keypoint absolute coordinates
      and scores.
    """

        input_image = self._crop_and_resize(image, crop_region, crop_size=crop_size)
        input_image = input_image.astype(dtype=np.float32)

        self._interpreter.set_tensor(
            self._input_index, np.expand_dims(input_image, axis=0)
        )
        self._interpreter.invoke()

        keypoints_with_scores = self._interpreter.get_tensor(self._output_index)
        keypoints_with_scores = np.squeeze(keypoints_with_scores)

        # Update the coordinates.
        for idx in range(len(BodyPart)):
            keypoints_with_scores[idx, 0] = (
                crop_region["y_min"]
                + crop_region["height"] * keypoints_with_scores[idx, 0]
            )
            keypoints_with_scores[idx, 1] = (
                crop_region["x_min"]
                + crop_region["width"] * keypoints_with_scores[idx, 1]
            )

        return keypoints_with_scores

    def detect(
        self, input_image: np.ndarray, reset_crop_region: bool = True
    ) -> np.ndarray:
        """Run detection on an input image.
    Args:
      input_image: A [height, width, 3] RGB image. Note that height and width
        can be anything since the image will be immediately resized according to
        the needs of the model within this function.
      reset_crop_region: Whether to use the crop region inferred from the
        previous detection result to improve accuracy. Set to True if this is a
        frame from a video. Set to False if this is a static image. Default
        value is True.
    Returns:
      An array of shape [17, 3] representing the keypoint coordinates and
      scores.
    """
        image_height, image_width, _ = input_image.shape

        if (self._crop_region is None) or reset_crop_region:
            # Set crop region for the first frame.
            self._crop_region = self.init_crop_region(image_height, image_width)

        # Detect pose using the crop region inferred from the detection result in
        # the previous frame
        keypoint_with_scores = self._run_detector(
            input_image,
            self._crop_region,
            crop_size=(self._input_height, self._input_width),
        )
        # Calculate the crop region for the next frame
        self._crop_region = self._determine_crop_region(
            keypoint_with_scores, image_height, image_width
        )

        return keypoint_with_scores

    def draw_keypoints(
        self, frame: np.ndarray, keypoints: np.ndarray, confidence_threshold: float
    ) -> None:
        """Draw circles around predicted body keypoints (eyes, knees, etc.)
    Args:
      frame: image frame from video
      keypoints: model-predicted keypoints of shape [17, 3]
      confidence_threshold: confidence threshold (0-1) for displaying keypoint
    """
        y, x, c = frame.shape
        # scale keypoints to frame dimensions
        shaped = np.squeeze(np.multiply(keypoints, [y, x, 1]))
        index = 0
        for kp in shaped:
            ky, kx, kp_conf = kp
            if kp_conf > confidence_threshold:
                cv2.circle(frame, (int(kx), int(ky)), 4, (255, 255, 255), -1)
            index += 1

    def draw_connections(
        self,
        frame: np.ndarray,
        keypoints: np.ndarray,
        edges: List[Tuple[int, int]],
        confidence_threshold: float,
    ) -> None:
        """Draw connections between keypoints (i.e. connection between eyes)
      Args:
        frame: image frame from video
        keypoints: model-predicted keypoints of shape [17, 3]
        edges: list of tuples indicating connected indices in keypoints
        confidence_threshold: confidence threshold (0-1) for displaying connections
      """
        y, x, c = frame.shape
        # scale keypoints to frame dimensions
        shaped = np.squeeze(np.multiply(keypoints, [y, x, 1]))

        for edge in edges:
            p1, p2 = edge
            y1, x1, c1 = shaped[p1]
            y2, x2, c2 = shaped[p2]

            if c1 > confidence_threshold and c2 > confidence_threshold:
                cv2.line(
                    frame, (int(x1), int(y1)), (int(x2), int(y2)), (255, 255, 255), 2
                )

    def render_keypoints(self, frame, keypoints_with_scores, confidence_threshold=0.3):
        """Render predicted keypoints (points and connections) on live video
      Args:
        frame: image frame
        keypoints_with_scores: model-predicted keypoints of shape [17, 3]
      """
        self.draw_connections(
            frame, keypoints_with_scores, Movenet.KEYPOINT_EDGES, confidence_threshold
        )
        self.draw_keypoints(frame, keypoints_with_scores, confidence_threshold)
