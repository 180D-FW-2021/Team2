import tensorflow as tf
import numpy as np
import cv2
import argparse

"""
    code reference: https://github.com/nicknochnack/MoveNetLightning
"""

# connections between keypoints for drawing keypoint estimation
EDGES = {
    (0, 1): "m",
    (0, 2): "c",
    (1, 3): "m",
    (2, 4): "c",
    (0, 5): "m",
    (0, 6): "c",
    (5, 7): "m",
    (7, 9): "m",
    (6, 8): "c",
    (8, 10): "c",
    (5, 6): "y",
    (5, 11): "m",
    (6, 12): "c",
    (11, 12): "y",
    (11, 13): "m",
    (13, 15): "m",
    (12, 14): "c",
    (14, 16): "c",
}


class Movenet:
    def __init__(self, model):
        """
        initialize movenet based on model (thunder, lightning)
        - model (str): movenet model
        - self.interpreter: movenet interpreter
        - self.dim (int): input image dimensions
        """
        if model == "lightning":
            self.interpreter = tf.lite.Interpreter(
                model_path="../models/lite-model_movenet_singlepose_lightning_3.tflite"
            )
            self.dim = 192
        else:
            self.interpreter = tf.lite.Interpreter(
                "../models/lite-model_movenet_singlepose_thunder_3.tflite"
            )
            self.dim = 256
        self.interpreter.allocate_tensors()

    def reshape_image(self, frame):
        """
        reshape image to proper input image dimensions and data type
        for model predictions
        """
        img = frame.copy()

        img = tf.image.resize_with_pad(np.expand_dims(img, axis=0), self.dim, self.dim)
        input_image = tf.cast(img, dtype=tf.float32)
        return input_image

    def draw_keypoints(self, frame, keypoints, confidence_threshold):
        """
        draw circles around predicted body keypoints (eyes, knees, etc.)
        args:
        - frame: image frame from video
        - keypoints: model prediction keypoints
        - confidence_threshold: confidence threshold (0-1) for displaying keypoint
        """
        y, x, c = frame.shape
        # scale keypoints to frame dimensions
        shaped = np.squeeze(np.multiply(keypoints, [y, x, 1]))

        for kp in shaped:
            ky, kx, kp_conf = kp
            if kp_conf > confidence_threshold:
                cv2.circle(frame, (int(kx), int(ky)), 4, (0, 255, 0), -1)

    def draw_connections(self, frame, keypoints, edges, confidence_threshold):
        """
        draw connections between keypoints (i.e. connection between eyes)
        args:
        - frame: image frame from video
        - keypoints: model-predicted keypoints
        - edges: mapping between connected keypoints
        - confidence_threshold: confidence threshold (0-1) for display
        """
        y, x, c = frame.shape
        # scale keypoints to frame dimensions
        shaped = np.squeeze(np.multiply(keypoints, [y, x, 1]))

        for edge, color in edges.items():
            p1, p2 = edge
            y1, x1, c1 = shaped[p1]
            y2, x2, c2 = shaped[p2]

            if (c1 > confidence_threshold) & (c2 > confidence_threshold):
                cv2.line(frame, (int(x1), int(y1)), (int(x2), int(y2)), (0, 0, 255), 2)

    def predict_keypoints(self, input_image):
        """
        output model prediction for body keypoints
        args:
        - input_image: image frame from video
        """
        reshaped_image = self.reshape_image(input_image)
        input_details = self.interpreter.get_input_details()
        output_details = self.interpreter.get_output_details()

        # make predictions
        self.interpreter.set_tensor(input_details[0]["index"], np.array(reshaped_image))
        self.interpreter.invoke()
        keypoints_with_scores = self.interpreter.get_tensor(output_details[0]["index"])
        return keypoints_with_scores

    def render_keypoints(self, frame, keypoints_with_scores):
        """
        render predicted keypoints (points and connections) on live video
        args:
        - frame: image frame
        - keypoints_with_scores: 
        """
        self.draw_connections(frame, keypoints_with_scores, EDGES, 0.4)
        self.draw_keypoints(frame, keypoints_with_scores, 0.4)


if __name__ == "__main__":
    # usage: python3 -m [lightning | thunder]
    parser = argparse.ArgumentParser()
    parser.add_argument("-m", "--model", type=str, default="lightning")
    args = parser.parse_args()
    model = args.model
    movenet = Movenet(model)

    # stream video from webcam and render model predictions
    cap = cv2.VideoCapture(0)
    while cap.isOpened():
        ret, frame = cap.read()

        keypoints_with_scores = movenet.predict_keypoints(frame)

        movenet.render_keypoints(frame, keypoints_with_scores)

        cv2.imshow("Movenet", frame)

        if cv2.waitKey(10) & 0xFF == ord("q"):
            break

    cap.release()
    cv2.destroyAllWindows()
