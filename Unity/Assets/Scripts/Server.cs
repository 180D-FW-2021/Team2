using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;

// referenced https://stackoverflow.com/questions/36526332/simple-socket-server-in-unity and
// https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-server-socket-example

// USAGE:
// IP address server is connected to will be printed in the console
// the port is the second argument so currently 8081

public class Server : MonoBehaviour
{
    public float forward;
    public bool left;
    public bool right;
    public bool jump;
    public bool duck;

    System.Threading.Thread SocketThread;
    volatile bool keepReading = false;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        startServer();

        forward = 0;
        left = false;
        right = false;
        jump = false;
        duck = false;
    }

    void startServer()
    {
        SocketThread = new System.Threading.Thread(networkCode);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    Socket listener;
    Socket handler;

    void networkCode()
    {
        string data;

        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // host running the application.
        // Debug.Log("Ip " + getIPAddress().ToString());
        IPAddress[] ipArray = Dns.GetHostAddresses("127.0.0.1");

        // IP address server is connected to will be printed in the console
        // the port is the second argument so currently 8081
        IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 8081);

        // Create a TCP/IP socket.
        listener = new Socket(ipArray[0].AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and 
        // listen for incoming connections.

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.
            while (true)
            {
                keepReading = true;

                // Program is suspended while waiting for an incoming connection.
                Debug.Log("Waiting for Connection");     //It works

                handler = listener.Accept();
                Debug.Log("Client Connected");     //It doesn't work
                data = null;

                // An incoming connection needs to be processed.
                while (keepReading)
                {
                    bytes = new byte[64];
                    int bytesRec = handler.Receive(bytes);
                    Debug.Log(bytesRec);
                    // Debug.Log("Received from Server");

                    if (bytesRec <= 0)
                    {
                        keepReading = false;
                        break;
                    }

                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                    if(bytesRec > 0) {
                        onDataReceived(data);
                    }
                    Debug.Log(data);

                    System.Threading.Thread.Sleep(1);
                }

                byte[] msg = Encoding.ASCII.GetBytes("Hello from server");
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                System.Threading.Thread.Sleep(1);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    void stopServer()
    {
        keepReading = false;

        //stop thread
        if (SocketThread != null)
        {
            SocketThread.Abort();
        }

        if (handler != null && handler.Connected)
        {
            handler.Disconnect(false);
            Debug.Log("Disconnected!");
        }
    }

    void OnDisable()
    {
        stopServer();
    }

    public void resetJumpVar()
    {
        jump = false;
    }

    public void resetPerspectiveVars()
    {
        left = false;
        right = false;
    }

    void onDataReceived(String data) {
        if (data == "f")
        {
            forward = 0.5f;
            resetPerspectiveVars();
        }
        if (data == "l")
        {
            left = true;
            right = false;
            forward = 0;
        }
        if (data == "r")
        {
            right = true;
            left = false;
            forward = 0;

        }
        if (data == "n")
        {
            forward = 0;
            resetPerspectiveVars();
        }
        if (data == "j")
        {
            jump = true;
            duck = false;
        }
        if (data == "d")
        {
            duck = true;
            jump = false;
        }
        if (data == "s")
        {
            jump = false;
            duck = false;
        }
    }

}
