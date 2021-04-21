using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField] private int maxHealth = 100;

    [SyncVar] // Syncing the health variable on local and remote server
    private int currentHealth;

    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField] private GameObject[] disableGameObjectsOnDeath;

    [SerializeField] private GameObject deathEffect;

    [SerializeField] private GameObject spawnEffect;

    private bool firstSetup = true;

    public void SetupPlayer()
    {
        if(isLocalPlayer)
        {
            // Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
        

        CmdBroadCastNewPlayer();
    }

    [Command]
    private void CmdBroadCastNewPlayer()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];

            for(int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }

   /* void Update ()
    {
        if(!isLocalPlayer)
            return;
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999999);
        }
    }*/

    [ClientRpc] //Rpc makes sure a method is called on all different clients
    public void RpcTakeDamage (int _amount)
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        // Disable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        // Disable the collider
        Collider _col = GetComponent<Collider>();
        if(_col != null)
            _col.enabled = false;

        // Spawn Death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        
        // Destroy graphics instance after a second
        Destroy(_gfxIns, 1f);

        // Switch cameras
        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        // Disable Components
        Debug.Log(transform.name + " IS DEAD !!");

        // Call Respawn Method
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

        Debug.Log(transform.name + " respawned.");
    }

    public void SetDefaults ()
    {
        isDead = false;

        currentHealth = maxHealth;

        // Enable the components 
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        // Enable the gameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        // Enable the collider
        Collider _col = GetComponent<Collider>();
        if(_col != null)
            _col.enabled = true;

        // Create spawn effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        
        // Destroy graphics instance after a second
        Destroy(_gfxIns, 1f);
    }


}
