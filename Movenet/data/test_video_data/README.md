# Test Video Data Notes

Recorded videos for testing movenet and movement_recognizer code.

## Test Files

Test files uploaded to privately-shared team google drive folder.

1. jump[num].mov

Single jump test cases (10 files)

2. duck[num].mov

Single duck test cases (10 files)

3. jump_duck[num].mov

Mixed duck/jump combinations (10 files)

4. walk_backward.mov, walk_forward.mov, lift_shoulders.mov, ...

Test false positives (i.e. walk forwards/backwards) (10 files)

### Single Jump Test Files

#### 1-2 ft from camera

1. jump1.mov

- 1 medium-height jump
- slight duck before jump
- jump start: 0.01 s; jump 3nd: 0.02 s
- distance: ~1.5-2 ft from camera

2. jump2.mov

- high jump
- bigger duck before jump
- arms out-of-frame at jump height
- jump start: 0.02 s
- distance: ~1.5-2 ft from camera

3. jump3.mov

- very light jump
- no noticeable duck before jump
- jump: ~0.02 s
- distance: ~1.5-2 ft from camera

4. jump4.mov

- light jump
- 2 ft from camera
- head not visible

5. jump5.mov

- high jump
- 2 ft from camera
- head not visible

#### 4-5 ft from camera

6. jump6.mov

- fast move back and forward to start/stop recording
- 0.03 s
- high jump

7. jump7.mov

- 0.01 s
- medium high jump

8. jump8.mov

- ~4-5 ft from camera
- fast move back and forward to start/stop recording
- jump: ~0.04 s
- very light jump
- FAILS

9. jump9.mov

- ~4-5 ft from camera
- jump: ~0.03 s
- medium jump
- FAILED

### Single Jump Test Files

#### 1-2 ft from camera

1. duck1.mov

- slight duck
- slow

2. duck2.mov

- fast duck
- full duck

3. duck3.mov

- medium (normal) duck

4. duck4.mov

- medium (normal) duck
- camera pointed upwards

5. duck5.mov

- fast duck
- camera pointed downwards

#### 4-5 ft from camera

1. duck6.mov

- fast duck

2. duck7.mov

- medium duck speed

3. duck8.mov

- fast duck

4. duck9.mov

- slow duck

5. duck10.mov

- slow-medium duck

## Jump/Duck Combinations

1. jump_duck.mov

- test combinations of 3 consecutive jump/duck(s)

## Test False Positives

1. walk_backward.mov
2. walk_forward.mov
3. raise_shoulders.mov
4. player_drift.mov
5. move_jump.mov
6. move_duck.mov
