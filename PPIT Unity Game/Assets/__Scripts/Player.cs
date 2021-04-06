using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar] // Syncing the health variable on local and remote server
    private int currentHealth;

    void Awake()
    {
        SetDefaults();
    }

    public void TakeDamage (int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    public void SetDefaults ()
    {
        currentHealth = maxHealth;
    }

    
}
