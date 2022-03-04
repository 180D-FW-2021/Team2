using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInstructions : MonoBehaviour
{
    public Text instrText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void Show(string instr)
    {
        instrText.text = instr;
    }

    public void Hide()
    {
        instrText.text = "";
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.name == "MoveInstrCollider")
        {
            Show("To move forward, use the arrow keys, WASD, or tilt the controller forward");
        }
        else if (hit.collider.name == "DuckInstrCollider")
        {
            Show("To duck, hold shift or duck while in frame of your webcam");
        }
        else if (hit.collider.name == "TurnInstrCollider")
        {
            Show("To turn left and right, use 'z' and 'x' or tilt the controller left and right");
        }
        else if (hit.collider.name == "JumpInstrCollider")
        {
            Show("To jump, press the space bar or lightly jump while in frame of your webcam");
        }
        else if (hit.collider.name == "EndInstrCollider")
        {
            Show("To finish the level, touch the door at the end. The floating arrow indicates where the door is.");
        }
        else if (hit.collider.name == "StopInstrCollider1" || hit.collider.name == "StopInstrCollider2" || hit.collider.name == "StopInstrCollider3")
        {
            Hide();
        }
    }
}
