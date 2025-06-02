using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DebugTextScript : MonoBehaviour
{
    public TMP_Text debugText;
    private static DebugTextScript instance;
    private string logBuffer = "";

    void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }else{
            Destroy(gameObject);
        }
    }

    void OnEnable(){
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable(){
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type){
        logBuffer += logString + "\n";

        if (logBuffer.Length > 2000) // trim if too long
            logBuffer = logBuffer.Substring(logBuffer.Length - 2000);

        if (debugText != null)
        {
            debugText.text = logBuffer;
        }
    }

    public static void Log(string message)
    {
        Debug.Log(message); // logs to Unity Console AND in-game text
    }

}
