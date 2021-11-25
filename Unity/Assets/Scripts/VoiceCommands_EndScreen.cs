using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

// reference: https://www.youtube.com/watch?v=29vyEOgsW8s

public class VoiceCommands_EndScreen : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	public GameManager GameManagerScript;

    void Start() {
		GameManagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        
    	keywords.Add("menu", MenuCallback);

		// Create the keyword recognizer and tell it what to recognize
		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
    	
		// Register OnPhraseRecognized event
		keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
    	keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
    	keywords[speech.text].Invoke();
    }

    private void MenuCallback() {
    	Debug.Log("to Main Menu");
		// MainMenuScripts.To_LevelSelector();
		GameManagerScript.UpdateGameState(GameState.MainMenu);
    }

}
