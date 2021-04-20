// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
     [SerializeField] Behaviour[] componentToDisable;
     [SerializeField] string remoteLayerName = "RemotePlayer";
     [SerializeField] string dontDrawLayerName = "DontDraw";
     [SerializeField] GameObject playerGraphics;
     [SerializeField] GameObject playerUIPrefab;

     private GameObject playerUIInstance;

     Camera sceneCamera;

    void Start ()
    {
        // when player is spawned check to see if I am controlling the player
        // If yes disable selected components 
        if (!isLocalPlayer)
        {
           DisableComponents();
           AssignRemoteLayer();
        }
        else {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            // Disable player graphics for local player
            SetLayerRecursively (playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // Create player UI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            // Configure PlayerUI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if(ui == null)
                Debug.LogError("No PlayerUI component on PlayerUI prefab!");
            ui.SetController(GetComponent<PlayerController>());
        }
        GetComponent<Player>().Setup();
    }

    void SetLayerRecursively (GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Setting up and registering the player
    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    void AssignRemoteLayer ()
    {
        // Converting the Layer string to layer index 
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents ()
    {
         for (int i = 0; i < componentToDisable.Length; i++)
            {
                componentToDisable[i].enabled = false;
            }
    }

    // Unregistering the player
    void onDisable ()
    {
        Destroy(playerUIInstance);

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);

    }
}
