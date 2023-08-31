using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance { get; private set; }

    private enum PlayerInteractions
    {
        PVP,
        NONE
    }

    [SerializeField] private PlayerInteractions playerInteractions = PlayerInteractions.NONE;

    [SerializeField] private string playerLayerName = "Player";

    [SerializeField]
    private GameObject sceneCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (playerInteractions == PlayerInteractions.PVP)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName), LayerMask.NameToLayer(playerLayerName), false);
        }
        else
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName), LayerMask.NameToLayer(playerLayerName), true);
        }
    }

    public void SetSceneCamera(bool _state)
    {
        if (sceneCamera == null)
        {
            return;
        }
        sceneCamera.SetActive(_state);
    }
}
