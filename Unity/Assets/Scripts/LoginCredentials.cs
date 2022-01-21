 using UnityEngine;

[System.Serializable]
public class LoginCredentials
{
    public string username;
    public string password;

    public string Stringify() 
    {
        return JsonUtility.ToJson(this);
    }
}