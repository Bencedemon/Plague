using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public abstract class AWeapon : NetworkBehaviour
{
    public WeaponProperty weaponProperty;
    [Space]
    [SerializeField] private PlayerMovement playerMovement;
    [Space]
    public int currentAmmoCount;
    [Space]
    public LayerMask layerMask;
    public Transform _cameraTransform;

    [Header("Self")]
    [SerializeField] private Animator weaponSelf;
    [SerializeField] private ParticleSystem muzzleFlashSelf;


    [Header("Other")]
    [SerializeField] private Animator weaponOther;
    [SerializeField] private ParticleSystem muzzleFlashOther;

    [Space]
    [SerializeField] public Animator characterAnim;
    
    [SerializeField] private NetworkObject hitParticle;

    public bool inAction = false;
    private bool inReload = false;

    public bool automaticShoot=false;

    void Start(){
        currentAmmoCount=weaponProperty.maxAmmo;
    }
    public void ActionEnd(){
        inAction = false;
        inReload = false;
        if(weaponProperty.weaponType==WeaponProperty.WeaponType.auomatic && automaticShoot){
            if(currentAmmoCount==1){
                inAction=true;
                AnimateWeapon();
                weaponSelf.SetTrigger("FireLast");
            }else
            if(currentAmmoCount>0){
                inAction=true;
                AnimateWeapon();
                weaponSelf.SetTrigger("Fire");
            }
        }
    }

    public void Fire(){
        if(inAction) return;
        if(currentAmmoCount==1){
            inAction=true;
            AnimateWeapon();
            weaponSelf.SetTrigger("FireLast");
        }else
        if(currentAmmoCount>0 || weaponProperty.maxAmmo==0){
            inAction=true;
            AnimateWeapon();
            weaponSelf.SetTrigger("Fire");
        }
        else{
            Reload();
        }
    }

    public void FireWeapon(){
        currentAmmoCount--;
        if(muzzleFlashSelf!=null)
            muzzleFlashSelf.Play();

        float x = Random.Range(-weaponProperty.spread*playerMovement.speedMultiplier,weaponProperty.spread*playerMovement.speedMultiplier);
        float y = Random.Range(-weaponProperty.spread*playerMovement.speedMultiplier,weaponProperty.spread*playerMovement.speedMultiplier);
        float z = Random.Range(-weaponProperty.spread*playerMovement.speedMultiplier,weaponProperty.spread*playerMovement.speedMultiplier);

        Vector3 directionWithSpread = _cameraTransform.forward + new Vector3(x,y,z);

        if(Physics.Raycast(_cameraTransform.position,directionWithSpread, out RaycastHit hit,weaponProperty.maxRange, layerMask)){

            if(hit.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                enemy.HitboxTakeDamage(weaponProperty.damage);
                SpawnParticle(weaponProperty.hitParticle_enemy,hit.point,hit.normal);
                return;
            }
            SpawnParticle(weaponProperty.hitParticle_wall,hit.point,hit.normal);
        }
    }

    public void Reload(){
        if(inReload) return;
        if(weaponProperty.maxAmmo==0) return;
        if(currentAmmoCount<weaponProperty.maxAmmo){
            inAction=true;
            inReload=true;
            if(currentAmmoCount==0)
                weaponSelf.SetTrigger("ReloadEmpty");
            else
                weaponSelf.SetTrigger("Reload");
        }
    }

    public void ReloadWeapon(){
        currentAmmoCount=weaponProperty.maxAmmo;
    }
    public void Punch(){
        if(inAction) return;
        inAction=true;
        weaponSelf.SetTrigger("Punch");
    }

    public void PunchWeapon(){
        if(Physics.Raycast(_cameraTransform.position,_cameraTransform.forward, out RaycastHit hit,weaponProperty.punchRange, layerMask)){

            if(hit.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                enemy.HitboxTakeDamage(weaponProperty.punchDamage);
                SpawnParticle(weaponProperty.hitParticle_enemy,hit.point,hit.normal);
                return;
            }
            SpawnParticle(weaponProperty.hitParticle_wall,hit.point,hit.normal);
        }
    }

    
    [ServerRpc]
    public virtual void SpawnParticle(NetworkObject hitParticle,Vector3 _point, Vector3 _normal){
        NetworkObject particle = Instantiate(hitParticle,_point,Quaternion.LookRotation(_normal));
        ServerManager.Spawn(particle);
    }

    [ServerRpc]
    public virtual void AnimateWeapon(){
        AnimateWeaponObserver();
    }

    [ObserversRpc]
    private void AnimateWeaponObserver(){
        weaponOther.SetTrigger("Fire");
        if(muzzleFlashOther!=null)
            muzzleFlashOther.Play();
    }



}
