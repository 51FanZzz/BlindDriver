using UnityEngine;
using Photon.Pun;

public class CameraSwitchTrigger : MonoBehaviourPun
{
    public Camera newCamera;
    public Camera mainCamera;

    private bool hasSwitched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasSwitched) return;

        if (other.CompareTag("Car"))
        {
            PhotonView carView = other.GetComponentInParent<PhotonView>();
            if (carView != null && carView.IsMine)
            {
                // Call RPC to switch camera locally for this player
                photonView.RPC("SwitchCamera", RpcTarget.All);
                hasSwitched = true;
            }
        }
    }

    [PunRPC]
    void SwitchCamera()
    {
        Debug.Log("Switching camera via RPC for local player.");
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (newCamera != null) newCamera.gameObject.SetActive(true);
    }
}
