import cv2
import argparse
from movenet import Movenet
from publisher import Publisher

"""
Script for detecting obstacle navigation (jump/duck)
and sending data to Unity through MQTT

References: 
- https://github.com/nicknochnack/MoveNetLightning
- lab 3 spec code for mqtt
"""

# TODO: jump/duck detection code

if __name__ == "__main__":

    # usage: python3 obstacle_detection.py --no-mqtt -m [thunder|lightning]
    parser = argparse.ArgumentParser()
    parser.add_argument("-m", "--model", type=str, default="lightning")
    parser.add_argument("--no-mqtt", dest="mqtt_on", action="store_false")
    args = parser.parse_args()
    mqtt_on = args.mqtt_on
    model = args.model

    movenet_obj = Movenet(model)

    # set up client
    if mqtt_on:
        publisher = Publisher()
        publisher.run()
        # TODO: send data only during jump/duck detection
        publisher.publish("ece180d/test/message", "i love 180da", qos=1)

    # capture video from webcam
    cap = cv2.VideoCapture(0)
    while cap.isOpened():
        ret, frame = cap.read()

        # predict body keypoints
        keypoints_with_scores = movenet_obj.predict_keypoints(frame)
        print(keypoints_with_scores)

        # render keypoint predictions on frame
        movenet_obj.render_keypoints(frame, keypoints_with_scores)
        cv2.imshow("frame", frame)

        if cv2.waitKey(10) & 0xFF == ord("q"):
            break

    cap.release()
    cv2.destroyAllWindows()

    if mqtt_on:
        publisher.stop()
