using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public GameObject RoleSelectionUI;
    public GameObject lobbyUI; 
    public GameObject loadingLobbyUI;
    public GameObject settingsUI;
    public GameObject playSceneUI;

    private bool isLobbyReady = false;

    void Start()
    {
        //Initially, RoleSelectionUI and lobbyUI are inactive until the lobby is loaded
        //Playscene will be the only one activated.
        if (RoleSelectionUI != null) RoleSelectionUI.SetActive(false);

        if (lobbyUI != null)lobbyUI.SetActive(false);

        if (loadingLobbyUI != null)loadingLobbyUI.SetActive(false);

        if (settingsUI != null)settingsUI.SetActive(false);

        if (playSceneUI != null)playSceneUI.SetActive(true);

        isLobbyReady = false;

    }

    public void OnPlayButtonPressed()
    {
        if (playSceneUI != null) playSceneUI.SetActive(false);

        if (loadingLobbyUI != null)loadingLobbyUI.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon since PLAY button is pressed...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby! Ready for room operations.");
        isLobbyReady = true;

        // When the lobby is loaded, active LobbyUI and disactivate loadingLobbyUI
        if(loadingLobbyUI != null)
            loadingLobbyUI.SetActive(false);
        if(lobbyUI != null)
            lobbyUI.SetActive(true);
    }

    public void CreateRoom()
    {
        if (!isLobbyReady)
        {
            Debug.LogWarning("Lobby is not ready yet!");
            return;
        }

        if (!string.IsNullOrEmpty(createInput.text))
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }
        else
        {
            Debug.LogWarning("CreateRoom: Room name is empty.");
        }
    }

    public void JoinRoom()
    {
        if (!isLobbyReady)
        {
            Debug.LogWarning("Lobby is not ready yet!");
            return;
        }

        if (!string.IsNullOrEmpty(joinInput.text))
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            Debug.LogWarning("JoinRoom: Room name is empty.");
        }
    }

    public override void OnJoinedRoom()
    {

        Debug.Log("JOINED ROOM!");
        if (RoleSelectionUI != null)
            RoleSelectionUI.SetActive(true);

        if (lobbyUI != null)
            lobbyUI.SetActive(false);
    }



    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join room failed: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create room failed: " + message);
    }


    // Called by the "Back" button
    public void LeaveRoomAndReturnToLobby()
    {
        StartCoroutine(LeaveRoomRoutine());
    }

    private IEnumerator LeaveRoomRoutine(){
    // Hide all UIs and activate loadingLobbyUI
        if(RoleSelectionUI != null)
            RoleSelectionUI.SetActive(false);
        if(lobbyUI != null)
            lobbyUI.SetActive(false);
        if(loadingLobbyUI != null)
            loadingLobbyUI.SetActive(true);

        isLobbyReady = false;

    if (PhotonNetwork.InRoom){
        PhotonNetwork.LeaveRoom();
        Debug.Log("Leaving room...");
    }

    // Wait until fully left room
    yield return new WaitUntil(() => !PhotonNetwork.InRoom);

    // Reconnect if needed
    if (!PhotonNetwork.IsConnected){
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
    }

        // Rejoin lobby
        if (!PhotonNetwork.InLobby){
            PhotonNetwork.JoinLobby();
            Debug.Log("Joining lobby...");
            yield return new WaitUntil(() => PhotonNetwork.InLobby);
        }
        Debug.Log("Returned to lobby. Ready to create or join rooms again.");
    }

    public void OpenSettingsUI(){
        if (settingsUI != null){
           settingsUI.SetActive(true);

           // Optionally hide other UIs while in settings
           if (lobbyUI != null) lobbyUI.SetActive(false);
           if (RoleSelectionUI != null) RoleSelectionUI.SetActive(false);
        }
    }

    public void ReturnToEnteringLobby(){
        // Close all lobby-related UIs
        if (lobbyUI != null)lobbyUI.SetActive(false);
        if (RoleSelectionUI != null)RoleSelectionUI.SetActive(false);
        if (settingsUI != null)settingsUI.SetActive(false);
        if (loadingLobbyUI != null)loadingLobbyUI.SetActive(false);

        if (playSceneUI != null)playSceneUI.SetActive(true);
        }

        // Back button in SettingsUI to go back to LobbyUI and reconnect to lobby
        public void BackFromSettings(){
        // Hide Settings UI
        if (settingsUI != null)
            settingsUI.SetActive(false);

        // Show Lobby UI again
        if (lobbyUI != null)
            lobbyUI.SetActive(true);

        // Ensure you're still in the Photon lobby
        if (!PhotonNetwork.InLobby){
            Debug.Log("Rejoining Photon lobby...");
            PhotonNetwork.JoinLobby();
        }
    }

    

}
