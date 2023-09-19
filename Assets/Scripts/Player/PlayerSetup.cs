using UnityEngine;
using FishNet.Object;
using System.Collections;
using FishNet.Connection;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField] private Camera playerCamera;

    // Initialize the player on clients
    public override void OnStartClient()
    {
        base.OnStartClient();
        Initialize();
    }

    private void Initialize()
    {
        if (base.IsOwner)
        {
            // enable scene camera
            LocalSceneManager.instance.SetSceneCamera(false);
            playerCamera.enabled = true;
            if(playerCamera.TryGetComponent(out AudioListener _audioListener))
            {
                _audioListener.enabled = true;
            }
        } else
        {
            // disable player camera
            playerCamera.enabled = false;
            if (playerCamera.TryGetComponent(out AudioListener _audioListener))
            {
                _audioListener.enabled = true;
            }
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (base.IsOwner)
        {
            // enable scene camera
            LocalSceneManager.instance.SetSceneCamera(true);
        }
    }
}
