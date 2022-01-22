using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;
using System;
using System.Threading.Tasks;

// Code for running a process
// https://answers.unity.com/questions/16675/running-an-external-exe-file-from-unity.html
// https://stackoverflow.com/questions/26236155/unity-c-sharp-run-shell-script
// Async code
// https://answers.unity.com/questions/1834871/why-i-cant-able-to-run-process-at-background-in-c.html
// Platform Directives
// https://docs.unity3d.com/Manual/PlatformDependentCompilation.html

public class RunPython : MonoBehaviour
{
    void Start()
    {
        string username = PlayerPrefs.GetString("Username");
        RunMovenet(username);
    }

    async public void RunMovenet(string username)
    {
        await Task.Run(() =>
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                // Uses LaunchGame.bat on WIndows machines only
                #if UNITY_STANDALONE_WIN
                UnityEngine.Debug.Log("Using Windows Script");
                psi.FileName = "..\\Setup_Scripts\\LaunchGame.bat";
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                #endif
                // Uses LaunchGame on Mac only
                #if UNITY_STANDALONE_OSX
                UnityEngine.Debug.Log("Using Mac Script");
                psi.FileName = "../Setup_Scripts/LaunchGame";
                psi.UseShellExecute = true;
                #endif
                
                psi.Arguments = username;
                psi.CreateNoWindow = true;

                Process p = Process.Start(psi);
                string strOutput = p.StandardOutput.ReadToEnd();
                

            }
            catch (Exception e)
            {
                print(e);
            }
        }
        );
    }
}
