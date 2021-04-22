using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;

    private PlayerWeapons currentWeapon;
    private WeaponManager weaponManager;

    void Start ()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No Camera referenced");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();

    }

    void Update ()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if(PauseMenu.IsOn)
            return;

        if(currentWeapon.fireRate <= 0f)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                //Debug.Log ("Shooting");
                Shoot();
            }
        }
        else 
        {
            if(Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }
            else if(Input.GetButtonUp ("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
        
    }

    // Is called on the server when a player shoots 
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    // Is called on all clients when we need to do a shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    // Is called on the server when we hit something, Takes in the hit point
    [Command]
    void CmdOnHit (Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    // Called on all clients, Here is where we spawn in cool effects
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, .5f);
    }

    [Client]
    void Shoot ()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        // We are shooting, call the Onshoot method on the server 
        CmdOnShoot();

        Debug.Log("Shooting!");
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask) )
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }

            // We hit something , call the OnHit method on the server
            CmdOnHit (_hit.point, _hit.normal);
        }
    }

    [Command]
    void CmdPlayerShot (string _playerID, int _damage)
    {
        Debug.Log (_playerID + " has been shot.");

        Player _player = GameManager.GetPlayer(_playerID);
       _player.RpcTakeDamage(_damage);
    }
}
