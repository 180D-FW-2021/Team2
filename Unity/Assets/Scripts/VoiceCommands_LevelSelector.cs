using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

// reference: https://www.youtube.com/watch?v=29vyEOgsW8s

public class VoiceCommands_LevelSelector : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	public GameManager GameManagerScript;

    void Start() {
		GameManagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;

        keywords.Add("zero", ZeroCallback);
        keywords.Add("tutorial", ZeroCallback);
    	keywords.Add("one", OneCallback);
	keywords.Add("two", TwoCallback);
	keywords.Add("three", ThreeCallback);
	keywords.Add("four", FourCallback);
    	keywords.Add("back", BackCallback);

		// Create the keyword recognizer and tell it what to recognize
		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
    	
		// Register OnPhraseRecognized event
		keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
    	keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
    	keywords[speech.text].Invoke();
    }

    private void ZeroCallback() {
    	Debug.Log("confirming level 0 ");
		GameManagerScript.UpdateGameState(GameState.ConfirmLevelSelection, 0);
    }

    private void OneCallback() {
    	Debug.Log("confirming level 1 ");
		GameManagerScript.UpdateGameState(GameState.ConfirmLevelSelection, 1);
    }

    private void TwoCallback() {
    	Debug.Log("confirming level 2 ");
		GameManagerScript.UpdateGameState(GameState.ConfirmLevelSelection, 2);
    }

    private void ThreeCallback() {
    	Debug.Log("confirming level 3 ");
		GameManagerScript.UpdateGameState(GameState.ConfirmLevelSelection, 3);
    }


    private void FourCallback() {
    	Debug.Log("confirming level 4 ");
		GameManagerScript.UpdateGameState(GameState.ConfirmLevelSelection, 4);
    }

    private void BackCallback() {
    	Debug.Log("Back to Main Menu");
		GameManagerScript.UpdateGameState(GameState.MainMenu);
    }
}
