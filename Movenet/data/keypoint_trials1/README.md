# Data collection notes

- Data collection to document pose keypoints for various actions
- Use to fine-tune thresholding/brainstorm action-estimation model

## General conditions

- before model optimization: no cropping/smoothing
- model: movenet lightning
- keypoint confidence threshold: 0.4
- data collection: python3 collect_data.py -f file_name.csv

## Trials

1. jump.csv

- one jump
- night time with desk light - bright lighting
- bright clothing (pink shirt, grey shorts)
- glasses (disrupts accuracy of face estimations)
- highest accuracy: left/right shoulder points
- knees sometimes below camera frame
- conf threshold initially set to 0.6 (lowered to 0.4)

2. duck.csv

- one duck
- duck1.csv: slow duck, bright clothes
- duck2.csv: fast duck, dark clothes
- duck3.csv: medium duck, dark clothes
- highest accuracy: left/right shoulder points
- conf threshold: 0.4

3. forward_back.csv

- walk forwards and then backwards
- used to test false positive

4. back_forward.csv

- walk backwards and then forwards
- used to test false positive

5. walk_back.csv, walk_forward.csv

- model player general movement/migration throughout game

## Latency

1. latency.csv

- avg real-time latency of 0.044992862 s between frame (~22 fps)
- measured on MacOS without Unity running in background

# Observations

Keypoint confidence

- accuracy optimized around 2 ft from camera
- most accurate keypoints (in order): shoulders, hips
- knees and below rarely in frame (esp after ducking)
- face keypoints less accurate

Jumping

- short initial peak (slight duck before jump)
- sharper, concave-up, symmetric peak (jump)
- movement is reflected across multiple keypoints
- approx length: 10 frames (~0.5 s)

Ducking

- concave-down, symmetric peak
- approx range in length: 17 - 33 frames (0.76 - 1.5 s)

Walk backwards/forwards

- much lower slope compared to jumping/ducking
- use slope thresholding to differentiate jump/duck with regular
  player movement

Possible model:

- detect sharp inc in y for duck
- detect sharp dec in y for jump
- can detect symmetric inc/dec but probably too low latency
  for creating a responsive user experience
- higher weight to higher-confidence keypoints (shoulders, hips)
- measure movement variance from sliding window average of player position
  to account for player drift while playing

# Concerns/challenges

- lower-accuracy model to optimize latency
- need to adjust main keypoints depending on in-frame body parts/confidence
- player drift/movement while playing game
- may apply filters to smoothen output & improve accuracy
- latency incurred streaming video, making predictions, and sending data over MQTT
- may have to send prediction before full movement is complete
