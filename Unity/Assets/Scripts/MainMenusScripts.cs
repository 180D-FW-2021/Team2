//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Attach this Script to any Menus to change Game State and Load a relevant menu

public class MainMenusScripts : MonoBehaviour
{
    public GameManager GameManagerScript;

    void Awake() {
        GameManagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    // Set the GameState to StartScreen and Load Start Screen
    public void To_StartScreen() {
        GameManagerScript.UpdateGameState(GameState.StartScreen);
    }

    // Set the GameState to MainMenu and Load Main Menu
    public void To_MainMenu() {
        GameManagerScript.UpdateGameState(GameState.MainMenu);
    }

    // Set the GameState to HelpMenu and Load Help Screen/Instructions
    public void To_HelpMenu() {
        GameManagerScript.UpdateGameState(GameState.HelpMenu);
    }

    // Set the GameState to SelectLevel and Load Level Selector Menu
    public void To_LevelSelector() {
        GameManagerScript.UpdateGameState(GameState.SelectLevel);
    }

    // Confirm the Selected Level
    public void To_ConfirmMaze(int Level) {
        GameManagerScript.UpdateGameState(GameState.ConfirmLevelSelection, Level);
    }

    // Load the level and then set the GameState to 'Playing'
    public void To_PlayMaze() {
        GameManagerScript.UpdateGameState(GameState.LoadSelectedLevel);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

}