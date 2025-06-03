using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class MissionComplete : MonoBehaviourPunCallbacks
{
    public GameObject missionCompleteUI;
    public Button menuButton;

    private bool hasTriggered;

    public void Start()
    {
        //When menu button is clicked. return to lobby and deacticate UI.
        if( menuButton != null)
        {
            menuButton.onClick.AddListener(ReturnToLobby);
            missionCompleteUI.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other){
        // If Car is colliding with this trigger. UI will be set active
        if (!hasTriggered && other.CompareTag("Car") ){
            Debug.Log("Final zone reached!");

            if (missionCompleteUI != null){
                missionCompleteUI.SetActive(true);
                hasTriggered = true;
            }
        }
    }

    void ReturnToLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene("Lobby");
        }
        
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room. Returning to Lobby Scene...");
        SceneManager.LoadScene("Lobby");
    }

}
