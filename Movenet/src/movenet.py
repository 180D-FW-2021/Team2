import tensorflow as tf
import numpy as np
import cv2
import argparse

'''
    code reference: https://github.com/nicknochnack/MoveNetLightning
'''

# Dictionary that maps from joint names to keypoint indices.
KEYPOINT_DICT = {
    'nose': 0,
    'left_eye': 1,
    'right_eye': 2,
    'left_ear': 3,
    'right_ear': 4,
    'left_shoulder': 5,
    'right_shoulder': 6,
    'left_elbow': 7,
    'right_elbow': 8,
    'left_wrist': 9,
    'right_wrist': 10,
    'left_hip': 11,
    'right_hip': 12,
    'left_knee': 13,
    'right_knee': 14,
    'left_ankle': 15,
    'right_ankle': 16
}

# connections between keypoints for drawing keypoint estimation
EDGES = {
    (0, 1): 'm',
    (0, 2): 'c',
    (1, 3): 'm',
    (2, 4): 'c',
    (0, 5): 'm',
    (0, 6): 'c',
    (5, 7): 'm',
    (7, 9): 'm',
    (6, 8): 'c',
    (8, 10): 'c',
    (5, 6): 'y',
    (5, 11): 'm',
    (6, 12): 'c',
    (11, 12): 'y',
    (11, 13): 'm',
    (13, 15): 'm',
    (12, 14): 'c',
    (14, 16): 'c'
}

class Movenet:
    def __init__(self, model):
        if(model == 'lightning'):
            self.interpreter = tf.lite.Interpreter(model_path='../models/lite-model_movenet_singlepose_lightning_3.tflite')
            self.dim = 192
        else:
            self.interpreter = tf.lite.Interpreter('../models/lite-model_movenet_singlepose_thunder_3.tflite')
            self.dim = 256
        self.interpreter.allocate_tensors()

    def reshape_image(self, frame):
        # Reshape image
        img = frame.copy()

        img = tf.image.resize_with_pad(np.expand_dims(img, axis=0), self.dim, self.dim)
        input_image = tf.cast(img, dtype=tf.float32)
        return input_image

    def draw_keypoints(self, frame, keypoints, confidence_threshold):
        y, x, c = frame.shape
        shaped = np.squeeze(np.multiply(keypoints, [y,x,1]))
        
        for kp in shaped:
            ky, kx, kp_conf = kp
            if kp_conf > confidence_threshold:
                cv2.circle(frame, (int(kx), int(ky)), 4, (0,255,0), -1)

    def draw_connections(self, frame, keypoints, edges, confidence_threshold):
        y, x, c = frame.shape
        shaped = np.squeeze(np.multiply(keypoints, [y,x,1]))
        
        for edge, color in edges.items():
            p1, p2 = edge
            y1, x1, c1 = shaped[p1]
            y2, x2, c2 = shaped[p2]
            
            if (c1 > confidence_threshold) & (c2 > confidence_threshold):      
                cv2.line(frame, (int(x1), int(y1)), (int(x2), int(y2)), (0, 0, 255), 2)

    def predict_keypoints(self, input_image):
        reshaped_frame = self.reshape_image(input_image)
        # Setup input and output 
        input_details = self.interpreter.get_input_details()
        output_details = self.interpreter.get_output_details()
        
        # Make predictions 
        self.interpreter.set_tensor(input_details[0]['index'], np.array(reshaped_frame))
        self.interpreter.invoke()
        keypoints_with_scores = self.interpreter.get_tensor(output_details[0]['index'])
        return keypoints_with_scores

    def render_keypoints(self, frame, keypoints_with_scores):
        self.draw_connections(frame, keypoints_with_scores, EDGES, 0.4)
        self.draw_keypoints(frame, keypoints_with_scores, 0.4)

if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('-m', '--model', type=str, default='lightning')
    args = parser.parse_args()
    model = args.model
    movenet = Movenet(model)

    cap = cv2.VideoCapture(0)
    while cap.isOpened():
        ret, frame = cap.read()
        
        keypoints_with_scores = movenet.predict_keypoints(frame)
        
        movenet.render_keypoints(frame, keypoints_with_scores)
        
        cv2.imshow('Movenet', frame)
        
        if cv2.waitKey(10) & 0xFF==ord('q'):
            break
            
    cap.release()
    cv2.destroyAllWindows()