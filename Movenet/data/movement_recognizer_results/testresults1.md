# MovementRecognizer Test Results

## Constants

- KEYPOINT_CONF_THRES = 0.3
- JUMP_RATE_THRES = 0.2
- DUCK_RATE_THRES = 0.2
- STATIONARY_THRES = 0.015

## Test Output

```
jump1.mov PASSED
jump2.mov PASSED
jump3.mov PASSED
jump4.mov PASSED
jump5.mov PASSED
jump6.mov PASSED
jump7.mov PASSED
jump8.mov FAILED
Expected:
['JUMP_START']
Actual:
[]
jump9.mov FAILED
Expected:
['JUMP_START']
Actual:
['DUCK_START']
jump10.mov PASSED
duck1.mov PASSED
duck2.mov PASSED
duck3.mov PASSED
duck4.mov PASSED
duck5.mov PASSED
duck6.mov PASSED
duck7.mov FAILED
Expected:
['DUCK_START']
Actual:
[]
duck8.mov PASSED
duck9.mov FAILED
Expected:
['DUCK_START']
Actual:
[]
duck10.mov FAILED
Expected:
['DUCK_START']
Actual:
[]
jump_duck1.mov FAILED
Expected:
['JUMP_START', 'DUCK_START', 'JUMP_START']
Actual:
['JUMP_START', 'DUCK_START']
jump_duck2.mov PASSED
jump_duck3.mov PASSED
jump_duck4.mov PASSED
jump_duck5.mov PASSED
jump_duck6.mov PASSED
jump_duck7.mov PASSED
jump_duck8.mov FAILED
Expected:
['DUCK_START', 'DUCK_START', 'JUMP_START']
Actual:
['DUCK_START', 'DUCK_START', 'DUCK_START']
walk_forward1.mov PASSED
walk_forward2.mov FAILED
Expected:
[]
Actual:
['DUCK_START']
walk_backward1.mov FAILED
Expected:
[]
Actual:
['DUCK_START']
walk_backward2.mov PASSED
lift_shoulders.mov FAILED
Expected:
[]
Actual:
['DUCK_START']
player_drift.mov PASSED
TOTAL ACCURACY: 0.7058823529411765
```
