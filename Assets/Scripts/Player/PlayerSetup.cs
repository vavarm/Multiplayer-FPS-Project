using UnityEngine;
using FishNet.Object;
using System.Collections;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField] private GameObject playerCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Initialize();
    }

    private void Initialize()
    {
        if (!base.IsOwner)
        {
            // Disable player camera
            playerCamera.SetActive(false);
        } else
        {
            // isHost or isLocalPlayer
            // Disable scene camera
            LocalSceneManager.instance.SetSceneCamera(false);
        }
    }

    private void OnDisable()
    {
        if (!base.IsOwner)
        {
            return;
        }
        // enable scene camera
        LocalSceneManager.instance.SetSceneCamera(true);
    }
}
