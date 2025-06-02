using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // or TMPro if using TMP

public class CamSwitchingDebug : MonoBehaviour
{
    public Text debugText; // or public TMP_Text debugText;

    private string fullLog = "";

    public void Log(string message)
    {
        fullLog += message + "\n";
        debugText.text = fullLog;
    }


}
