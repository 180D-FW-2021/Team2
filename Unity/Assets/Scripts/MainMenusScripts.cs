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

    // Set the GameState to SelectLevel and Load Level Selector Menu
    public void To_LevelSelector() {
        GameManagerScript.UpdateGameState(GameState.SelectLevel);
    }

    // Load the level and then set the GameState to 'Playing'
    public void To_Maze(int Level) {
        GameManagerScript.UpdateGameState(GameState.LoadLevel, Level);
    }



}