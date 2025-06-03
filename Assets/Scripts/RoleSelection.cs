using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class RoleID
{
    public const int Driver = 1;
    public const int Navigator = 2;
}

public class RoleSelection : MonoBehaviourPunCallbacks
{
    public GameObject roleSelectionUI;
    public GameObject createJoinUI;

    public Button driverButton;
    public Button navigatorButton;

    public GameObject driverSelectedIndicator; 
    public GameObject navigatorSelectedIndicator; 

    private bool roleSelected = false;
    private bool gameStarted = false;

    void Start(){
        PhotonNetwork.AutomaticallySyncScene = true; //The scene will be synced accoridng to the MasterClient
    }


    public void SelectDriver()
    {
        SetPlayerRole(RoleID.Driver);
        UpdateSelectionVisuals(driverSelectedIndicator, navigatorSelectedIndicator);
        Debug.Log("Driver is selected");
    }

    public void SelectNavigator()
    {
        SetPlayerRole(RoleID.Navigator);
        UpdateSelectionVisuals(navigatorSelectedIndicator, driverSelectedIndicator);
        Debug.Log("Navigator is selected");

    }



    private void SetPlayerRole(int roleID){
        Hashtable props = new Hashtable{ { "PlayerRole", roleID }};
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        roleSelected = true;
        CheckTakenRolesAndUpdateUI();
        Debug.Log("Player selected role: " + roleID);
    }




    // If playerA choose Driver, this desicion will be notified via this callback
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(changedProps.ContainsKey("PlayerRole"))
        {
            CheckTakenRolesAndUpdateUI();
            // Connect to the next scene when both roles are selected
            CheckIfBothRolesSelectedAndStartGame();
        }

    }
    // A safety method to check if both roles are chosen && UI are updated for visual cue
    public override void OnPlayerEnteredRoom(Player newPlayer){
            CheckTakenRolesAndUpdateUI();
            CheckIfBothRolesSelectedAndStartGame(); 
    }

    private void CheckTakenRolesAndUpdateUI(){
        bool isDriverTaken = false;
        bool isNavigatorTaken = false;

        foreach(Player player in PhotonNetwork.PlayerList){ // Update role status through photon network and update instantly
            if( player.CustomProperties.TryGetValue("PlayerRole", out object role)){
                if((int)role == RoleID.Driver)isDriverTaken = true;
                if((int)role == RoleID.Navigator)isNavigatorTaken = true;
            }
        }
        // The corresponding button will not be interactive after being selected
        driverButton.interactable = !isDriverTaken;
        navigatorButton.interactable = !isNavigatorTaken;

    }

    private void CheckIfBothRolesSelectedAndStartGame(){
        //prevent multiple starts
        if (gameStarted) return;

        bool isDriverSelected = false;
        bool isNavigatorSelected = false;

        foreach( Player player in PhotonNetwork.PlayerList){
            if(player.CustomProperties.TryGetValue("PlayerRole", out object role)){
                int roleID = (int)role;

                if(roleID == RoleID.Driver) isDriverSelected = true;
                if(roleID == RoleID.Navigator) isNavigatorSelected = true;
            }
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && isDriverSelected && isNavigatorSelected){
            if (PhotonNetwork.IsMasterClient){
                gameStarted = true;
             Debug.Log("Both roles selected. Waiting 0.5s before loading...");
             Invoke("LoadGameScene", 0.5f);  // <- Delay to let roles sync
         }
     }
    }

    private void LoadGameScene(){
        PhotonNetwork.LoadLevel("GameScene");
    }

    private void UpdateSelectionVisuals(GameObject selectedIndicator, GameObject otherIndicator)
    {
        if (selectedIndicator != null) selectedIndicator.SetActive(true);
        if (otherIndicator != null) otherIndicator.SetActive(false);
    }

    //Check and update current roles status
    //Clear previous selected roles and reset selections
    public override void OnJoinedRoom(){
        Debug.Log("Room Joined !");
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerRole"))
        {
            Hashtable props = new Hashtable { { "PlayerRole", null } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            Debug.Log("Resetting old roles on joined room");
        }
        ResetSelectionIndicators();
        CheckTakenRolesAndUpdateUI();
    }


    public void OnBackButtonPressed()
    {
        //Clear chosen roles before leaving the room
        //Notify other people as well
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerRole"))
        {
            Hashtable props = new Hashtable { { "PlayerRole", null } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        
        PhotonNetwork.LeaveRoom();

        Debug.Log("Leaving room and resetting role...");
    }


    //Resetting UIs and button state
    private void ResetSelectionIndicators()
    {
        if (driverSelectedIndicator != null) 
        driverSelectedIndicator.SetActive(false);

        if (navigatorSelectedIndicator != null) 
        navigatorSelectedIndicator.SetActive(false);

        if(driverButton != null) 
        driverButton.interactable = true;
        if(navigatorButton != null) 
        navigatorButton.interactable = true;
    }



    public override void OnLeftRoom()
    {
        Debug.Log("Left the room, returning to LobbyUI.");

        ResetSelectionIndicators(); // Reset the visual indicators

        gameStarted = false;

        if (roleSelectionUI != null)
            roleSelectionUI.SetActive(false);

        if (createJoinUI != null)
            createJoinUI.SetActive(true);
    }

    private void ResetAllRolesUI()
    {
        ResetSelectionIndicators();
        driverButton.interactable = true;
        navigatorButton.interactable = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("A player left the room. Resetting role selection UI.");
        ResetAllRolesUI();
    }
}
