"""expected results for integration test"""
expected_results = {
    "forward_backward.mov": [],
}

# items: (file_name (str), predicted_positions (List[Position.name]))
expected_results2 = {
    # SINGLE JUMP:
    ## 1-2 ft from camera with shoulders in frame
    "jump1.mov": ["JUMP_START"],
    "jump2.mov": ["JUMP_START"],
    "jump3.mov": ["JUMP_START"],
    "jump4.mov": ["JUMP_START"],
    "jump5.mov": ["JUMP_START"],
    ## 4-5 ft from camera with shoulders in frame
    "jump6.mov": ["JUMP_START"],
    "jump7.mov": ["JUMP_START"],
    "jump8.mov": ["JUMP_START"],
    "jump9.mov": ["JUMP_START"],
    "jump10.mov": ["JUMP_START"],
    # SINGLE DUCK
    ## 1-2 ft from camera with shoulders in frame
    "duck1.mov": ["DUCK_START"],
    "duck2.mov": ["DUCK_START"],
    "duck3.mov": ["DUCK_START"],
    "duck4.mov": ["DUCK_START"],
    "duck5.mov": ["DUCK_START"],
    ## 4-5 ft from camera with shoulders in frame
    "duck6.mov": ["DUCK_START"],
    "duck7.mov": ["DUCK_START"],
    "duck8.mov": ["DUCK_START"],
    "duck9.mov": ["DUCK_START"],
    "duck10.mov": ["DUCK_START"],
    # JUMP/DUCK Combinations
    "jump_duck1.mov": ["JUMP_START", "DUCK_START", "JUMP_START"],
    "jump_duck2.mov": ["JUMP_START", "DUCK_START", "JUMP_START"],
    "jump_duck3.mov": ["JUMP_START", "JUMP_START", "DUCK_START"],
    "jump_duck4.mov": ["DUCK_START", "DUCK_START", "JUMP_START"],
    "jump_duck5.mov": ["JUMP_START", "JUMP_START", "JUMP_START"],
    "jump_duck6.mov": ["DUCK_START", "DUCK_START", "DUCK_START"],
    "jump_duck7.mov": ["JUMP_START", "DUCK_START", "DUCK_START"],
    "jump_duck8.mov": ["DUCK_START", "DUCK_START", "JUMP_START"],
    "jump_duck9.mov": ["JUMP_START", "JUMP_START", "JUMP_START"],
    "jump_duck10.mov": ["DUCK_START", "DUCK_START", "DUCK_START"],
    # Movements to test false positives
    "walk_forward1.mov": [],
    "walk_forward2.mov": [],
    "walk_backward1.mov": [],
    "walk_backward2.mov": [],
    "lift_shoulders.mov": [],
    # slow drift in different directions
    "player_drift.mov": [],
    ## slow forwards/backwards
    "forward_backward.mov": [],
    ## fast forwards/backwards
    "forward_backward2.mov": [],
    # move and then jump
    "move_jump.mov": ["JUMP_START"],
    # move and then duck
    "move_duck.mov": ["DUCK_START"],
}
