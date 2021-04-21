// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
     [SerializeField] Behaviour[] componentToDisable;
     [SerializeField] string remoteLayerName = "RemotePlayer";
     [SerializeField] string dontDrawLayerName = "DontDraw";
     [SerializeField] GameObject playerGraphics;
     [SerializeField] GameObject playerUIPrefab;

     [HideInInspector] public GameObject playerUIInstance;

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

            GetComponent<Player>().SetupPlayer();
        }
        
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

        if(isLocalPlayer)
            // When  no longer active as a player switch to scene camera
            GameManager.instance.SetSceneCameraActive(true);

        GameManager.UnRegisterPlayer(transform.name);

    }
}
