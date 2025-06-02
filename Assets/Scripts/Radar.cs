using UnityEngine;
using Photon.Pun;

public class Radar : MonoBehaviour
{
    public string sensorName; // FL, FR, RL, RR
    private RadarManager radarManager;

    private void Start(){
        radarManager = GetComponentInParent<RadarManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Obstacles") || radarManager == null) return;
        Debug.Log($"[Radar] {sensorName} triggered by {other.name}");

        if (radarManager.photonView.Owner == PhotonNetwork.LocalPlayer){
            radarManager.RPC_RadarHit(sensorName, other.name); 
        }else{
            radarManager.photonView.RPC("RPC_RadarHit", radarManager.photonView.Owner, sensorName, other.name);
        }
    }

    private void OnTriggerExit(Collider other){
        if (!other.CompareTag("Obstacles") || radarManager == null) return;
        Debug.Log($"[Radar] {sensorName} exit by {other.name}");

        if (radarManager.photonView.Owner == PhotonNetwork.LocalPlayer){
            radarManager.HideRadarUI(sensorName); // call local method directly
        }else{
            radarManager.photonView.RPC("HideRadarUI", radarManager.photonView.Owner, sensorName);
        }
    }

}
