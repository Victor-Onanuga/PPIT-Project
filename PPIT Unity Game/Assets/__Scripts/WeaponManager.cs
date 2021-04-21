using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private string weaponLayerName = "Weapon";

    [SerializeField] private Transform weaponHolder;

    [SerializeField] private PlayerWeapons primaryWeapon;

    private PlayerWeapons currentWeapon;
    private WeaponGraphics currentGraphics;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapons GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(PlayerWeapons _weapon)
    {
        currentWeapon = _weapon;

         GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
         _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if(currentGraphics == null)
            Debug.LogError ("No WeaponGraphics component on the weapon object: " + _weaponIns.name);

        // if local player change layer of the weapon 
         if(isLocalPlayer)
            Utility.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
         
    }
}
