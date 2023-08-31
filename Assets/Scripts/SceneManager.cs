using System.Collections;
using System.Collections.Generic;
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
}
