# Position Tracking for Player Obstacle Navigation

## Installation

1. Install python dependencies

```
# opencv for visualizing movenet keypoint predictions
pip install opencv-python
# mqtt to send data to Unity
pip install paho-mqtt
# tensorflow for movenet model
pip install tensorflow
pip install --upgrade tensorflow-hub
pip install -q git+https://github.com/tensorflow/docs
pip install --upgrade tensorflow-estimator==2.6.0
```

2. Download movenet lightning TFLite model:

- [Lightning](https://tfhub.dev/google/lite-model/movenet/singlepose/lightning/3)

3. Add the model to the `Movenet/models` directory.

## Usage

1. Run position tracking for obstacle navigation

`python3 position_tracking.py --no-mqtt --unity --latency`

The `no-mqtt` argument runs the script without comms with Unity. The `unity` argument should only be set when running the script from Unity, as it uses relative paths to obtain the Movenet models. The latency argument outputs the average time per frame in seconds.

2. Run data collection

`python3 collect_data.py -f file_name.csv`

## Files

1. position_tracking.py

Main script for detecting obstacle navigation (duck/jump)
and sending data to Unity over MQTT.

2. movenet.py

Movenet class for detecting pose keypoints using tensorflow movenet.

3. publisher.py

MQTT publisher class.

4. movement_recognizer.py

Stores previous $max_keypoints keypoints and outputs whether player is
jumping, ducking, out of frame, or stationary.

5. keypoints.py

Keypoints class to encapsulate logic for extracting body part coords.

6. keypoints_filter.py

Apply one-euro filter to x and y coords in keypoints.

7. one_euro_filter.py

One-euro filter class.

8. enums.py

Enums for player position and mapping from body part to keypoint index.

## Test

1. collect_data.py

Collect data for tuning position tracking model.

2. integration_test.py

Test accuracy of movenet/movement_recognizer using pre-recorded videos in data/test_video_data.

Note that the reported accuracy differs by FPS, which differs depending on your computer's real-time latency. I tried using a set FPS with opencv, but this is unreliable. As a result, some test cases towards the end will fail that typically would pass if ran individually or with the original position_tracking code.

Current accuracy over 40 samples if 0.675 on my computer without accounting for FPS difference for the last test cases. Accuracy is 0.75 accounting for this difference.

3. Real-time Latency: position_tracking.py

You can measure the real-time latency in secs/frame for your machine by running the position_tracking script with the latency flag set:

`python3 position_tracking --latency`

The average real-time latency per frame will be outputted once the program exits.

Current MacOS latencies:

- Without Unity: ~0.0653 secs/frame (15.3 FPS)
- With Unity: ~0.0715 sec/frame (13.986 FPS)

## Resources

### Download MoveNet TFLite

- Thunder model: https://tfhub.dev/google/movenet/singlepose/thunder/4
- Lightning model: https://tfhub.dev/google/movenet/singlepose/lightning/4

### MoveNet Examples

- Lightning example: https://github.com/nicknochnack/MoveNetLightning
- Tensorflow-js: https://github.com/tensorflow/tfjs-models/tree/master/pose-detection
- Python MoveNet TFLite example: https://github.com/tensorflow/examples/blob/master/lite/examples/pose_estimation/raspberry_pi/ml/movenet.py
- Browser demo: https://storage.googleapis.com/tfjs-models/demos/pose-detection/index.html?model=movenet

### One Euro Filter

- Filter: https://github.com/jaantollander/OneEuroFilter/blob/master/python/one_euro_filter.py
- Applying filter to keypoints: https://github.com/tensorflow/tfjs-models/blob/e7f6e120aa05c24bdbf9663358e44f082f76f345/shared/filters/keypoints_one_euro_filter.ts#L31
