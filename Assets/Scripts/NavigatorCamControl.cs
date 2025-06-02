using UnityEngine;
using Photon.Pun;

public class NavigatorCamControl : MonoBehaviourPun
{
    public Camera navigatorCam;

    void Start()
    {
        if (!photonView.IsMine)
        {
            navigatorCam.enabled = false;
        }else{
            navigatorCam.enabled = true;
        }
    }
}
