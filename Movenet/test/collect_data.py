import cv2
import argparse
import csv
import time
import sys

sys.path.append("../src")
from movenet import Movenet

""" 
Collect keypoint-estimation data from webcam livestream

Reference: 
https://github.com/nicknochnack/MoveNetLightning
"""

# map keypoint_name -> index
keypoint_order = [
    "nose",
    "left_eye",
    "right_eye",
    "left_ear",
    "right_ear",
    "left_shoulder",
    "right_shoulder",
    "left_elbow",
    "right_elbow",
    "left_wrist",
    "right_wrist",
    "left_hip",
    "right_hip",
    "left_knee",
    "right_knee",
    "left_ankle",
    "right_ankle",
]

# get header with csv labels
def get_csv_header():
    header = []
    for label in keypoint_order:
        # (y_coord, x_coord, confidence)
        header.append(f"{label}_y")
        header.append(f"{label}_x")
        header.append(f"{label}_cfd")
    header.append("real_time")
    return header


# get only y-coord labels
# (jumping/ducking only considers y coordinates)
def get_csv_header_y():
    header = []
    for label in keypoint_order:
        header.append(f"{label}_y")
    header.append("real_time")
    return header


if __name__ == "__main__":

    # usage: python3 collect_data.py -f file_name.csv -m [ lightning | thunder ]
    parser = argparse.ArgumentParser()
    parser.add_argument("-m", "--model", type=str, default="lightning")
    parser.add_argument("-f", "--file_name", type=str, default="data.csv")

    args = parser.parse_args()
    model = args.model
    file_name = args.file_name
    movenet = Movenet(model)

    # capture webcam video
    cap = cv2.VideoCapture(0)
    with open(file_name, "w", newline="") as file:

        writer = csv.DictWriter(file, fieldnames=get_csv_header_y())
        writer.writeheader()

        while cap.isOpened():
            start_rt = time.time()
            ret, frame = cap.read()

            keypoints_with_scores = movenet.detect(frame)

            write_dict = {}
            for i in range(len(keypoints_with_scores)):
                kp = keypoints_with_scores[i]
                # (x_coord, y_coord, confidence)
                ky, kx, kp_conf = kp
                # only consider keypoints with confidence > 0.3
                if kp_conf > 0.3:
                    kp_label = keypoint_order[i]
                    y_label = f"{kp_label}_y"
                    # measure real-time latency per frame (s)
                    time_elapsed = time.time() - start_rt
                    '''
                    uncomment to write (x, conf) data
                    x_label = f"{kp_label}_x"
                    conf_label = f"{kp_label}_cfd"
                    write_dict.update(
                        {
                            y_label: ky,
                            x_label: kx,
                            conf_label: kp_conf,
                            "real_time": time_elapsed,
                        }
                    )
                    '''
                    write_dict.update({y_label: ky, "real_time": time_elapsed})

            if write_dict != {}:
                writer.writerow(write_dict)

            movenet.render_keypoints(frame, keypoints_with_scores)

            cv2.imshow("Movenet", frame)

            if cv2.waitKey(10) & 0xFF == ord("q"):
                break

    cap.release()
    cv2.destroyAllWindows()
