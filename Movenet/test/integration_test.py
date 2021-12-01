from typing import List
import sys
import skvideo.io
from int_expected_results import expected_results

sys.path.append("../src")
from movenet import Movenet
from movement_recognizer import MovementRecognizer
from keypoints_filter import KeypointsFilter
from enums import Position as Pos

"""
Simple integration test for pre-recorded video samples
Test movenet and movenet_recognizer code together
"""

movenet_obj = Movenet("lightning")
recog_obj = MovementRecognizer()


def get_time_sec(frame_num: int) -> float:
    """Convert frame number to time in video (seconds)
       Future use to make testing more robust by checking
       jump/duck time
    """
    # determined through data collection
    sec_per_frame = 0.044992862
    return sec_per_frame * frame_num


def get_predictions(file_name: str) -> List[str]:
    """ output jump/duck predictions from video
    """
    pos_lst = []
    # append relative path to video data direc
    file_name = f"../data/test_video_data/{file_name}"
    videodata = skvideo.io.vread(file_name)

    for frame in videodata:

        # predict body keypoints
        keypoints_with_scores = movenet_obj.detect(frame)

        player_pos = recog_obj.add_and_recognize(keypoints_with_scores)

        # only consider jump/duck for testing
        if player_pos == Pos.JUMP_START or player_pos == Pos.DUCK_START:
            pos_lst.append(player_pos.name)

    return pos_lst


if __name__ == "__main__":
    total_passed = 0
    for test_file in expected_results.keys():
        actual_result = get_predictions(test_file)
        if actual_result != expected_results[test_file]:
            print(f"{test_file} FAILED")
            print("Expected:")
            print(expected_results[test_file])
            print("Actual:")
            print(actual_result)
        else:
            print(f"{test_file} PASSED")
            total_passed += 1
    accuracy = float(total_passed) / len(expected_results)
    print(f"TOTAL ACCURACY: {accuracy}")
