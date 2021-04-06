// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
     [SerializeField] Behaviour[] componentToDisable;
     [SerializeField]  string remoteLayerName = "RemotePlayer";

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
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);

    }
}
