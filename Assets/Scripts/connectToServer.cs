using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private bool isConnecting = false;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connecting to Photon...");
            isConnecting = true;
            StartCoroutine(CheckConnection());
        }
        else
        {
            Debug.Log("Already connected to Photon. Joining lobby...");
            PhotonNetwork.JoinLobby();
        }
    }

    private IEnumerator CheckConnection()
    {
        float startTime = Time.time;
        while (!PhotonNetwork.IsConnected && Time.time - startTime < 10f) // Wait max 10 sec
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Successfully connected to Photon!");
        }
        else
        {
            Debug.LogError("Failed to connect to Photon after 10 seconds.");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby! Loading next scene...");
        SceneManager.LoadScene("Lobby"); // Ensure the scene name is correct
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon: " + cause);
        isConnecting = false;
    }
}
