using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Authentication : MonoBehaviour
{
    public GameManager GameManagerScript;
    public InputField inputUsername;
    public InputField inputPassword;
    public GameObject invalidCredentialsUI;
    public RunMovenet myrunMovenet;

    string username;
    string password;
    //bool invalidCredentialsProvided = false;

    private void Start()
    {
        // clear player prefs when user starts login page
        PlayerPrefs.DeleteAll();
    }

    public void ContinueAsGuest() 
    {
        // Set to False b/c Movenet will be turned off
        PlayerPrefs.SetString("MovenetConnected", "F");

        // Note that we are playing as guest
        PlayerPrefs.SetString("PlayingAsGuest", "T");

        // Load Main Menu
        GameManagerScript.UpdateGameState(GameState.MainMenu);
    }

    public void Authenticate(string uri) 
    {
        // Organize info to make POST request
        username = inputUsername.text;
        Debug.Log(username);
        password = inputPassword.text;
        string url = "https://amaze-webapp.herokuapp.com/api/" + uri; // uri is either 'signup' or 'login'

        // Save username (in case we authenticate properly)
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("PlayingAsGuest", "F");
        PlayerPrefs.SetString("MovenetConnected", "F");

        // Prepare JSON body for Post Request
        LoginCredentials credentialsObj = new LoginCredentials();
        credentialsObj.username = username;
        credentialsObj.password = password;
        string json = credentialsObj.Stringify();

        // Make request to Webapp
        StartCoroutine(MakeLoginRequest(url, json));
    }

    IEnumerator MakeLoginRequest(string url, string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                invalidCredentialsProvidedEvent();
            }
            else
            {
                // If successful, go to the Main Menu!
                Debug.Log("success!");
                myrunMovenet.StartMovenet();
                // "N" indicates movenet is starting but hasn't opened yet
                // "F" used to indicate that player has turned movenet off
                PlayerPrefs.SetString("MovenetConnected", "N");
                GameManagerScript.UpdateGameState(GameState.MainMenu);
            }
        }
    }

    void invalidCredentialsProvidedEvent() {
        // invalidCredentialsProvided = true;
        invalidCredentialsUI.SetActive(true);
    }

    public void invalidCredentialsProvidedCloseEvent() {
        // invalidCredentialsProvided = false;
        invalidCredentialsUI.SetActive(false);
    }

}
