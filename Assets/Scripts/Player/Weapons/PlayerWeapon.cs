using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private List<AWeapon> weapons = new List<AWeapon>();

    public AWeapon currentWeapon;

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
            if(!playerMovement.canMove) return;
            currentWeapon.Fire();
            if(currentWeapon.weaponProperty.weaponType==WeaponProperty.WeaponType.auomatic){
                currentWeapon.automaticShoot=true;
            }
        }
        if(context.canceled){
            if(currentWeapon.weaponProperty.weaponType==WeaponProperty.WeaponType.auomatic){
                currentWeapon.automaticShoot=false;
            }
        }
    }
    public void Reload(InputAction.CallbackContext context){
        if(context.performed){
            if(!playerMovement.canMove) return;
            currentWeapon.Reload();
        }
    }

    public void Punch(InputAction.CallbackContext context){
        if(context.performed){
            if(!playerMovement.canMove) return;
            currentWeapon.Punch();
        }
    }

    public void SwitchWeapon_1(InputAction.CallbackContext context){
        if(context.performed){
            if(currentWeapon.inAction) return;
            SwitchWeapon(0);
        }
    }
    public void SwitchWeapon_2(InputAction.CallbackContext context){
        if(context.performed){
            if(currentWeapon.inAction) return;
            SwitchWeapon(1);
        }
    }
    public void SwitchWeapon_3(InputAction.CallbackContext context){
        if(context.performed){
            if(currentWeapon.inAction) return;
            SwitchWeapon(2);
        }
    }

    private void SwitchWeapon(int _id){
        SetWeaponIndex(_id);
    }

    
    public void Scroll(InputAction.CallbackContext context){
        if(currentWeapon.inAction) return;
        float scroll = context.ReadValue<Vector2>().y;
        if(scroll>0){
            if(_currentWeaponIndex.Value+1<weapons.Count)
                SwitchWeapon(_currentWeaponIndex.Value+1);
            else
                SwitchWeapon(0);
        }else if(scroll<0){
            if(_currentWeaponIndex.Value-1>=0)
                SwitchWeapon(_currentWeaponIndex.Value-1);
            else
                SwitchWeapon(weapons.Count-1);
        }
    }

    [ServerRpc] private void SetWeaponIndex(int _id) => _currentWeaponIndex.Value = _id;

    private void OnCurrentWeaponIndexChanged(int oldIndex, int newIndex, bool asServer){
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        currentWeapon=weapons[newIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.characterAnim.SetBool("rifleType",currentWeapon.weaponProperty.rifleType);
    }
}
