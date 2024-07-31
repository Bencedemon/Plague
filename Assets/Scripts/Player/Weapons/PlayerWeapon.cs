using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    //[SerializeField] private List<AWeapon> weapons = new List<AWeapon>();
    [Header("Weapons")]
    [SerializeField] private AWeapon primary;
    [SerializeField] private AWeapon[] primaries;
    [Space]
    [SerializeField] private AWeapon secondary;
    [SerializeField] private AWeapon[] secondaries;
    [Space]
    [SerializeField] private AWeapon melee;
    [SerializeField] private AWeapon[] melees;

    [Space]
    [Space]
    public AWeapon currentWeapon;


    [Space]
    [Header("Stats")]
    public PlayerStats playerStats;

    public readonly SyncVar<int> _currentWeaponIndex = new();

    private PlayerProfileManager playerProfileManager;
    void Awake(){
        playerProfileManager=FindObjectOfType<PlayerProfileManager>();
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
        primary=primaries[playerProfileManager.playerProfile.primaryId];
        secondary=secondaries[playerProfileManager.playerProfile.secondaryId];
        melee=melees[playerProfileManager.playerProfile.meeleId];
        
        if(primary!=null){
            SetWeaponIndex(0);
            currentWeapon=primary;
        }else if(secondary!=null){
            SetWeaponIndex(1);
            currentWeapon=primary;
        }else{
            SetWeaponIndex(2);
            currentWeapon=melee;
        }
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.characterAnim.SetBool("rifleType",currentWeapon.weaponProperty.rifleType);
    }

    public void Shoot(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
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
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(!playerMovement.canMove) return;
            currentWeapon.Reload();
        }
    }

    public void Punch(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(!playerMovement.canMove) return;
            currentWeapon.Punch();
        }
    }

    public void SwitchWeapon_1(InputAction.CallbackContext context){
        if(primary==null) return;
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(currentWeapon.inAction) return;
            SwitchWeapon(0);
        }
    }
    public void SwitchWeapon_2(InputAction.CallbackContext context){
        if(secondary==null) return;
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(currentWeapon.inAction) return;
            SwitchWeapon(1);
        }
    }
    public void SwitchWeapon_3(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(currentWeapon.inAction) return;
            SwitchWeapon(2);
        }
    }

    private void SwitchWeapon(int _id){
        SetWeaponIndex(_id);
    }

    
    public void Scroll(InputAction.CallbackContext context){
        if(primary==null && secondary==null) return;
        if(playerStats._currentHealth.Value<=0) return;
        if(currentWeapon.inAction) return;
        float scroll = context.ReadValue<Vector2>().y;
        if(scroll>0){
            if(_currentWeaponIndex.Value<2){
                SwitchWeapon(_currentWeaponIndex.Value+1);
            }
            else{
                SwitchWeapon(0);
            }
        }else if(scroll<0){
            if(_currentWeaponIndex.Value-1>=0)
                SwitchWeapon(_currentWeaponIndex.Value-1);
            else
                SwitchWeapon(2);
        }
    }

    [ServerRpc] private void SetWeaponIndex(int _id) => _currentWeaponIndex.Value = _id;

    private void OnCurrentWeaponIndexChanged(int oldIndex, int newIndex, bool asServer){
        primary.gameObject.SetActive(false);
        secondary.gameObject.SetActive(false);
        melee.gameObject.SetActive(false);
        switch (newIndex)
        {
            case 0:
                currentWeapon=primary;
            break;
            case 1:
                currentWeapon=secondary;
            break;
            case 2:
                currentWeapon=melee;
            break;
            default:
                Debug.LogError(newIndex+" is out of range");
            break;
        }
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.characterAnim.SetBool("rifleType",currentWeapon.weaponProperty.rifleType);
    }

    public bool CanGetAmmo(){
        if(primary.currentTotalAmmo<primary.weaponProperty.totalAmmo){
            Debug.Log("PrimaryIsNotFull");
            return true;
        }
        if(secondary.currentTotalAmmo<secondary.weaponProperty.totalAmmo){
            Debug.Log("SecondaryIsNotFull");
            return true;
        }
        return false;
    }
    public void GetAmmo(){
        if(primary.weaponProperty.maxAmmo!=0){
            int ammo = primary.weaponProperty.totalAmmo/10;
            if(primary.currentTotalAmmo+ammo<=primary.weaponProperty.totalAmmo){
                primary.currentTotalAmmo += ammo;
            }else{
                primary.currentTotalAmmo = primary.weaponProperty.totalAmmo;
            }
        }
        
        if(secondary.weaponProperty.maxAmmo!=0){
            int ammo = secondary.weaponProperty.totalAmmo/10;
            if(secondary.currentTotalAmmo+ammo<=secondary.weaponProperty.totalAmmo){
                secondary.currentTotalAmmo += ammo;
            }else{
                secondary.currentTotalAmmo = secondary.weaponProperty.totalAmmo;
            }
        }
    }
}
