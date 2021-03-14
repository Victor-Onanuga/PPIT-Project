using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
     [SerializeField] Behaviour[] componentToDisable;

     Camera sceneCamera;

    void Start ()
    {
        // when player is spawned check to see if I am controlling the player
        // If yes disable selected components 
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentToDisable.Length; i++)
            {
                componentToDisable[i].enabled = false;
            }
        }
        else {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
            
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
