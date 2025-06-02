using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public Transform driverSpawnPoint;
    public Transform navigatorSpawnPoint;

    void Start(){
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out object roleObj)){
            int role = (int)roleObj;

            if (role == RoleID.Driver){
                Debug.Log("LocalPlayer is Driver — Instantiating Prometheus...");
                GameObject car = PhotonNetwork.Instantiate("Prometheus", driverSpawnPoint.position, driverSpawnPoint.rotation);

                // Transfer ownership to self (Driver)
                PhotonView carView = car.GetComponent<PhotonView>();
                if (carView != null){
                    carView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    Debug.Log("Ownership transferred to Driver");
                }

            }else if (role == RoleID.Navigator){
                Debug.Log("LocalPlayer is Navigator — Instantiating Navigator POV...");
                PhotonNetwork.Instantiate("Navigator POV", navigatorSpawnPoint.position, navigatorSpawnPoint.rotation);
            }
        }else{
            Debug.LogError("PlayerRole not found in custom properties.");
        }
    }
}
