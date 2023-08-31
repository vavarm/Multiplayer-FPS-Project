using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField] private GameObject playerCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            // disable player camera
            playerCamera.SetActive(false);
            return;
        }
        else if (isServerOnly)
        {
            // disable player camera
            playerCamera.SetActive(false);
        }
        // isHost or isLocalPlayer
        // disable scene camera
        SceneManager.instance.SetSceneCamera(false);
    }

    private void OnDisable()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        // enable scene camera
        SceneManager.instance.SetSceneCamera(true);
    }
}
