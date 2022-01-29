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
        RunMovenet(username, Application.dataPath);
    }

    async public void RunMovenet(string username, string path)
    {
        await Task.Run(() =>
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                // Executable path for Windows machines only
                #if UNITY_STANDALONE_WIN
                UnityEngine.Debug.Log("Using Windows Script");
                psi.FileName =  path + "\\..\\..\\..\\Movenet\\src\\position_tracking.exe";
                #endif
                // Executable path for Mac only
                #if UNITY_STANDALONE_OSX
                UnityEngine.Debug.Log("Using Mac Script");
                psi.FileName = "../Movenet/src/position_tracking";
                #endif

                // Paths for development
                #if UNITY_EDITOR_WIN
                psi.FileName =  path + "\\..\\..\\Movenet\\src\\position_tracking.exe";
                #endif
                #if UNITY_EDITOR_OSX
                psi.FileName = "../Movenet/src/position_tracking";
                #endif


                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.CreateNoWindow = true;
                psi.Arguments = "--username " + username;

                Process p = Process.Start(psi);
                p.ErrorDataReceived += onError;
                // printing pose output overloads console
                // p.OutputDataReceived += onOutput;
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

            }
            catch (Exception e)
            {
                print(e);
            }
        }
        );
    }
    // log stdout/stderr for debugging purposes
     private void onOutput(object sender, DataReceivedEventArgs e) {
        UnityEngine.Debug.Log(e.Data);
    }

    private void onError(object sender, DataReceivedEventArgs e) {
        UnityEngine.Debug.Log(e.Data);
    }
}
