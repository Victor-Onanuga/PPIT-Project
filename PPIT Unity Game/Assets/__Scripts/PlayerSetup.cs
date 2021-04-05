// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.

using UnityEngine;
using UnityEngine.Networking;

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

        RegisterPlayer();
    }

    void RegisterPlayer ()
    {
        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
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

    void onDisable ()
    {
        if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(true);
            }
    }
}
