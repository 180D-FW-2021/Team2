# Position Tracking for Player Obstacle Navigation

## Installation

1. Install python dependencies

```
pip install paho-mqtt
pip install tensorflow
pip install --upgrade tensorflow-hub
pip install -q git+https://github.com/tensorflow/docs
pip install --upgrade tensorflow-estimator==2.6.0
```

2. Download movenet thunder and/or lightning TFLite model:

- [Thunder](https://tfhub.dev/google/movenet/singlepose/thunder/4)
- [Lightning](https://tfhub.dev/google/movenet/singlepose/lightning/4)

3. Add the model to the Movenet/models directory.

## Usage

1. Run position tracking for obstacle navigation

`python3 position_tracking.py --no-mqtt -m [thunder|lightning]`

2. Run data collection

`python3 collect_data.py -m [thunder|lightning] -f file_name.csv`

## Files

1. position_tracking.py

Main script for detecting obstacle navigation (duck/jump)
and sending data to Unity over MQTT.

2. movenet.py

Movenet class for detecting pose keypoints using tensorflow movenet

3. publisher.py

MQTT publisher class.

4. movement_recognizer.py

Stores previous $max_frames keypoints and outputs whether player is
jumping, ducking, out of frame, or stationary.

5. keypoints_filter.py

Apply one-euro filter to x and y coords in keypoints.

6. one_euro_filter.py

One-euro filter class.

7. enums.py

Enums for player position and mapping from body part to keypoint index.

8. collect_data.py

Collect data for tuning position tracking model.

## Resources

### Download MoveNet TFLite

- Thunder model: https://tfhub.dev/google/movenet/singlepose/thunder/4
- Lightning model: https://tfhub.dev/google/movenet/singlepose/lightning/4

### MoveNet Examples

- Lightening example: https://github.com/nicknochnack/MoveNetLightning
- Tensorflow-js: https://github.com/tensorflow/tfjs-models/tree/master/pose-detection
- Python MoveNet TFLite example: https://github.com/tensorflow/examples/blob/master/lite/examples/pose_estimation/raspberry_pi/ml/movenet.py
- Browser demo: https://storage.googleapis.com/tfjs-models/demos/pose-detection/index.html?model=movenet

### One Euro Filter

- Filter: https://github.com/jaantollander/OneEuroFilter/blob/master/python/one_euro_filter.py
- Applying filter to keypoints: https://github.com/tensorflow/tfjs-models/blob/e7f6e120aa05c24bdbf9663358e44f082f76f345/shared/filters/keypoints_one_euro_filter.ts#L31