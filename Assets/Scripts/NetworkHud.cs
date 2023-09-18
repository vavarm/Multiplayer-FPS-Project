using UnityEngine;
using UnityEngine.UI;
using FishNet.Managing;

public class NetworkHud : MonoBehaviour
{
    [SerializeField]
    private NetworkManager networkManager;

    [SerializeField]
    private Button serverButton;
    [SerializeField]
    private Button clientButton;
    [SerializeField]
    private Image serverIndicator;
    [SerializeField]
    private Image clientIndicator;

    private void Start()
    {
        if (networkManager == null)
            Debug.LogError("NetworkManager is null.", this);
        if (serverButton == null)
            Debug.LogError("Server button is null.", this);
        if (clientButton == null)
            Debug.LogError("Client button is null.", this);
        if (serverIndicator == null)
            Debug.LogError("Server indicator is null.", this);
        if (clientIndicator == null)
            Debug.LogError("Client indicator is null.", this);
        serverIndicator.color = Color.red;
        clientIndicator.color = Color.red;
        serverButton.onClick.AddListener(OnServerButtonClicked);
        clientButton.onClick.AddListener(OnClientButtonClicked);
    }

    private void Update()
    {
        if (networkManager.IsServer)
        {
            serverIndicator.color = Color.green;
        }
        else
        {
            serverIndicator.color = Color.red;
        }
        if (networkManager.IsClient)
        {
            clientIndicator.color = Color.green;
        }
        else
        {
            clientIndicator.color = Color.red;
        }
    }

    private void OnServerButtonClicked()
    {
        if (networkManager.IsServer)
        {
            networkManager.ServerManager.StopConnection(true);
        }
        else
        {
            networkManager.ServerManager.StartConnection();
        }
    }

    private void OnClientButtonClicked()
    {
        if(networkManager.IsClient)
        {
            networkManager.ClientManager.StopConnection();
        } else
        {
            networkManager.ClientManager.StartConnection();
        }
    }
}
