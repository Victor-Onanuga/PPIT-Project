
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private string weaponLayerName = "Weapon";

    [SerializeField] private Transform weaponHolder;

    [SerializeField] private PlayerWeapons primaryWeapon;

    private PlayerWeapons currentWeapon;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapons GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(PlayerWeapons _weapon)
    {
        currentWeapon = _weapon;

         GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
         _weaponIns.transform.SetParent(weaponHolder);
         if(isLocalPlayer)
             _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
         
    }
}
