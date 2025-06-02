using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RadarManager : MonoBehaviourPun
{
    public AudioClip radarBeep;

    public GameObject radarUI_fl;
    public GameObject radarUI_fr;
    public GameObject radarUI_rl;
    public GameObject radarUI_rr;

    private Dictionary<string, float> radarCooldownTimers = new Dictionary<string, float>();
    private float cooldownTime = 1.0f; // to avoid the beeping is repeating

    void Start(){
        radarUI_fl.SetActive(false);
        radarUI_fr.SetActive(false);
        radarUI_rl.SetActive(false);
        radarUI_rr.SetActive(false);
    }

    [PunRPC]
public void RPC_RadarHit(string sensorName, string obstacleName){
    Debug.Log($"[REMOTE] {sensorName} radar detected: {obstacleName}");

    if (radarCooldownTimers.ContainsKey(sensorName) && Time.time < radarCooldownTimers[sensorName]){
        Debug.Log($"[Cooldown] Skipped {sensorName} — on cooldown");
        return;
    }

    radarCooldownTimers[sensorName] = Time.time + cooldownTime;

    if (radarBeep != null){
        AudioSource.PlayClipAtPoint(radarBeep, transform.position);
        Debug.Log("[Sound] Radar beep played");
    }

    switch(sensorName){
        case "FL":
            radarUI_fl?.SetActive(true);
            break;
        case "FR":
            radarUI_fr?.SetActive(true);
            break;
        case "RL":
            radarUI_rl?.SetActive(true);
            break;
        case "RR":
            radarUI_rr?.SetActive(true);
            break;
    }

    //  auto-hide the UIs after 1.5s
    StartCoroutine(AutoHide(sensorName, 1.5f));
}


    [PunRPC]
    public void HideRadarUI(string sensorName){
        switch(sensorName){
            case"FL":
                radarUI_fl?.SetActive(false); 
                break;
            case"FR":
                radarUI_fr?.SetActive(false); 
                break;
            case "RL": 
                radarUI_rl?.SetActive(false); 
                break;
            case "RR": 
                radarUI_rr?.SetActive(false); 
                break;
        }
    }


    private IEnumerator AutoHide(string sensorName, float delay){
    yield return new WaitForSeconds(delay);
    photonView.RPC("HideRadarUI", RpcTarget.All, sensorName);
}





}
