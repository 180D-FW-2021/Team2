import cv2
import numpy as np
import time
import argparse
from movenet import Movenet
from publisher import Publisher
from movement_recognizer import MovementRecognizer
from keypoints_filter import KeypointsFilter

"""
Script for detecting obstacle navigation (jump/duck)
and sending data to Unity through MQTT

References: 
- https://github.com/nicknochnack/MoveNetLightning
- lab 3 spec code for mqtt
"""

if __name__ == "__main__":

    # usage: python3 obstacle_detection.py --no-mqtt --latency -m [thunder|lightning]
    parser = argparse.ArgumentParser()
    parser.add_argument("-u", "--username", type=str, default="demo")
    parser.add_argument("-m", "--model", type=str, default="lightning")
    parser.add_argument("--no-mqtt", dest="mqtt_on", action="store_false")
    # output avg real-time latency in secs/frame
    parser.add_argument("--latency", dest="latency", action="store_true")
    args = parser.parse_args()
    username = args.username
    mqtt_on = args.mqtt_on
    model = args.model
    measure_latency = args.latency

    movenet_obj = Movenet(model)
    recog_obj = MovementRecognizer()
    filt_obj = KeypointsFilter()
    latencies = []

    # set up MQTT publisher
    if mqtt_on:
        publisher = Publisher()
        publisher.run(username)

    prev_time = time.time()
    # capture video from webcam
    cap = cv2.VideoCapture(0)
    while cap.isOpened():
        try:
            ret, frame = cap.read()

            # predict body keypoints
            keypoints_with_scores = movenet_obj.detect(frame)

            # uncomment to apply filter
            # keypoints_with_scores = filt_obj.apply_filter(keypoints_with_scores)
            
            player_pos = recog_obj.add_and_recognize(keypoints_with_scores)
            print(player_pos.name)

            # send data to Unity
            if mqtt_on:
                publisher.send(player_pos)

            # render keypoint predictions on frame
            movenet_obj.render_keypoints(frame, keypoints_with_scores)
            cv2.imshow("webcam_video", frame)
            
            if measure_latency:
                curr_time = time.time()
                latencies.append(curr_time - prev_time)
                prev_time = curr_time

            if cv2.waitKey(10) & 0xFF == ord("q"):
                break

        # exit gracefully on Ctrl-C
        except KeyboardInterrupt:
            break

    cap.release()
    cv2.destroyAllWindows()

    if mqtt_on:
        publisher.stop()

    if measure_latency:
        print(f"AVG LATENCY (secs/frame): {np.mean(latencies)}")