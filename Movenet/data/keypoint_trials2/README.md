# Data collection notes

- Data collection to document pose keypoints for various actions
- Use to fine-tune thresholding/brainstorm action-estimation model

## General conditions

- after model optimization: cropping/smoothing
- model: movenet lightning
- keypoint confidence threshold: 0.3
- data collection: python3 collect_data.py -f file_name.csv

## Trials

1. jump.csv

- one jump
- conf threshold initially set to 0.3
- total duration: ~12 samples (0.5 s)
- upward movement: ~6 samples (.2594 s)

2. duck.csv

- one duck
- conf threshold: 0.3
- total duration: 16 samples (trial1), 30 samples (trial2)

3. forward_back.csv

- walk forwards and then backwards
- used to test false positive

4. back_forward.csv

- walk backwards and then forwards
- used to test false positive

## Latency

1. latency.csv

- avg real-time latency of 0.04323597 s between frame (~22 fps)
- measured on MacOS without Unity running in background

# Observations

- Higher accuracy compared to non-optimized version (keypoint_trials1)
- Might need state machine of jump/duck to decide whether to send
  data with MQTT
- Isolated jump/duck data with graphs:
  https://docs.google.com/spreadsheets/d/11mQONTfApCqtQkOTAuxMjhGCdroXAihDo0AmXSqPGQw/edit?usp=sharing
