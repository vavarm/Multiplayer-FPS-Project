using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public static SceneManager instance { get; private set; }

    private enum PlayerInteractions
    {
        PVP,
        NONE
    }

    [SerializeField]
    private PlayerInteractions playerInteractions;

    [SerializeField]
    private string playerLayerName = "Player";

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // disable the collision between players
        if (playerInteractions == PlayerInteractions.NONE)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName), LayerMask.NameToLayer(playerLayerName), true);
        }
    }
}
