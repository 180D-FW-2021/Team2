import cv2
import argparse
from movenet import Movenet
from publisher import Publisher
from movement_recognizer import MovementRecognizer, Position

"""
Script for detecting obstacle navigation (jump/duck)
and sending data to Unity through MQTT

References: 
- https://github.com/nicknochnack/MoveNetLightning
- lab 3 spec code for mqtt
"""

# TODO: jump/duck detection code

def print_position(pos):
    if pos == Position.DUCK:
        print("DUCK")
    elif pos == Position.JUMP:
        print("JUMP")
    elif pos == Position.OUT_OF_FRAME:
        print("OUT OF FRAME")

if __name__ == "__main__":

    # usage: python3 obstacle_detection.py --no-mqtt -m [thunder|lightning]
    parser = argparse.ArgumentParser()
    parser.add_argument("-m", "--model", type=str, default="lightning")
    parser.add_argument("--no-mqtt", dest="mqtt_on", action="store_false")
    args = parser.parse_args()
    mqtt_on = args.mqtt_on
    model = args.model

    movenet_obj = Movenet(model)
    recog_obj = MovementRecognizer()

    # set up MQTT publisher
    if mqtt_on:
        publisher = Publisher()
        publisher.run()

    # capture video from webcam
    cap = cv2.VideoCapture(0)
    while cap.isOpened():
        ret, frame = cap.read()

        # predict body keypoints
        keypoints_with_scores = movenet_obj.predict_keypoints(frame)
        
        player_pos = recog_obj.add_and_recognize(keypoints_with_scores)
        print_position(player_pos)

        # send data to Unity
        if mqtt_on:
            publisher.send(player_pos)

        # render keypoint predictions on frame
        movenet_obj.render_keypoints(frame, keypoints_with_scores)
        cv2.imshow("frame", frame)

        if cv2.waitKey(10) & 0xFF == ord("q"):
            break

    cap.release()
    cv2.destroyAllWindows()

    if mqtt_on:
        publisher.stop()
