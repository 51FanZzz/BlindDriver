using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// To only display UIs for the driver but not the navigator
public class DriverBlackScreenUI : MonoBehaviourPunCallbacks
{
    void Start(){
    if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out object role)){
        if((int)role == RoleID.Driver){
            gameObject.SetActive(true);
        }else{
            gameObject.SetActive(false);
        }
        }else{
        Debug.LogWarning("Player role is not found");
        gameObject.SetActive(false);
    }
  } 
}
