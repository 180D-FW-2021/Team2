using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    // Open the Instructions in the Webapp
    public void OpenWebappInstructions()
    {
        Application.OpenURL("https://amaze-webapp.herokuapp.com/instructions");
    }

    // Open the Leaderboard in the Webapp
    public void OpenWebappLeaderboard()
    {
        Application.OpenURL("https://amaze-webapp.herokuapp.com/leaderboard");
    }
}
