using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField] private List<AWeapon> weapons = new List<AWeapon>();

    private AWeapon currentWeapon;

    private readonly SyncVar<int> _currentWeaponIndex = new();

    void Awake(){
        _currentWeaponIndex.OnChange += OnCurrentWeaponIndexChanged;;
    }


    public override void OnStartClient(){
        base.OnStartClient();
        if(!base.IsOwner){
            enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        currentWeapon=weapons[0];
        currentWeapon.gameObject.SetActive(true);
    }

    public void Shoot(InputAction.CallbackContext context){
        if(context.performed){
            FireWeapon();
        }
    }

    public void SwitchWeapon_1(InputAction.CallbackContext context){
        if(context.performed){
            SwitchWeapon(0);
        }
    }
    public void SwitchWeapon_2(InputAction.CallbackContext context){
        if(context.performed){
            SwitchWeapon(1);
        }
    }
    public void SwitchWeapon_3(InputAction.CallbackContext context){
        if(context.performed){
            SwitchWeapon(2);
        }
    }

    private void SwitchWeapon(int _id){
        SetWeaponIndex(_id);
    }

    [ServerRpc] private void SetWeaponIndex(int _id) => _currentWeaponIndex.Value = _id;

    private void OnCurrentWeaponIndexChanged(int oldIndex, int newIndex, bool asServer){
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        currentWeapon=weapons[newIndex];
        currentWeapon.gameObject.SetActive(true);
    }

    private void FireWeapon(){
        currentWeapon.Fire();
    }
}
