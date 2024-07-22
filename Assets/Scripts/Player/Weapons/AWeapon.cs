using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public abstract class AWeapon : NetworkBehaviour
{
    public WeaponProperty weaponProperty;
    [Space]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] public PlayerStats playerStats;
    [Space]
    public int currentAmmoCount;
    public int currentTotalAmmo;
    [Space]
    public LayerMask layerMask;
    public Transform _cameraTransform;

    [Header("Self")]
    [SerializeField] public Animator weaponSelf;
    [SerializeField] public ParticleSystem muzzleFlashSelf;
    [SerializeField] public AudioSource audioSource;


    [Header("Other")]
    [SerializeField] public Animator weaponOther;
    [SerializeField] public ParticleSystem muzzleFlashOther;
    [SerializeField] public AudioSource audioSourceOther;

    [Space]
    [SerializeField] public Animator characterAnim;
    

    [Space]
    public bool inAction = false;
    public bool inReload = false;

    public bool automaticShoot=false;

    void Start(){
        currentAmmoCount=weaponProperty.maxAmmo;
        currentTotalAmmo=weaponProperty.totalAmmo;
    }
    public void ActionEnd(){
        inAction = false;
        inReload = false;
        if(weaponProperty.weaponType==WeaponProperty.WeaponType.auomatic && automaticShoot){
            if(currentAmmoCount==1){
                inAction=true;
                AnimateWeaponShoot();
                weaponSelf.SetTrigger("FireLast");
            }else
            if(currentAmmoCount>0){
                inAction=true;
                AnimateWeaponShoot();
                weaponSelf.SetTrigger("Fire");
            }
        }
    }

    public void Fire(){
        if(inAction) return;
        if(currentAmmoCount==1){
            inAction=true;
            AnimateWeaponShoot();
            weaponSelf.SetTrigger("FireLast");
        }else
        if(currentAmmoCount>0 || weaponProperty.maxAmmo==0){
            inAction=true;
            AnimateWeaponShoot();
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

        audioSource.clip = weaponProperty.shoot[Random.Range(0,weaponProperty.shoot.Length)];
        audioSource.Play();

        float x = Random.Range(-weaponProperty.spread*playerMovement.speedMultiplier,weaponProperty.spread*playerMovement.speedMultiplier);
        float y = Random.Range(-weaponProperty.spread*playerMovement.speedMultiplier,weaponProperty.spread*playerMovement.speedMultiplier);
        float z = Random.Range(-weaponProperty.spread*playerMovement.speedMultiplier,weaponProperty.spread*playerMovement.speedMultiplier);

        Vector3 directionWithSpread = _cameraTransform.forward + new Vector3(x,y,z);

        if(Physics.Raycast(_cameraTransform.position,directionWithSpread, out RaycastHit hit,weaponProperty.maxRange, layerMask)){

            if(hit.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                Vector3 direction = (hit.point-_cameraTransform.position).normalized;
                enemy.HitboxTakeDamage(weaponProperty.damage*playerStats.strength,direction,playerStats);
                SpawnParticle(weaponProperty.hitParticle_enemy,hit.point,hit.normal);
                return;
            }
            SpawnParticle(weaponProperty.hitParticle_wall,hit.point,hit.normal);
        }
    }

    public virtual void Reload(){
        if(inReload) return;
        if(weaponProperty.maxAmmo==0) return;
        if(currentTotalAmmo<=0) return;
        if(currentAmmoCount<weaponProperty.maxAmmo){
            weaponSelf.SetFloat("reloadSpeed",playerStats.reloadSpeed);
            inAction=true;
            inReload=true;
            if(currentAmmoCount==0){
                AnimateWeaponReload(true);
                weaponSelf.SetTrigger("ReloadEmpty");
                audioSource.clip = weaponProperty.reloadLast;
                audioSource.Play();
            }
            else{
                AnimateWeaponReload(false);
                weaponSelf.SetTrigger("Reload");
                audioSource.clip = weaponProperty.reload;
                audioSource.Play();
            }
        }
    }

    public void ReloadWeapon(){
        int missingBullet = weaponProperty.maxAmmo-currentAmmoCount;
        if(currentTotalAmmo-missingBullet >= 0){
            currentAmmoCount+=missingBullet;
            currentTotalAmmo-=missingBullet;
        }else{
            currentAmmoCount+=currentTotalAmmo;
            currentTotalAmmo=0;
        }
    }
    public void Punch(){
        if(inAction) return;
        inAction=true;
        weaponSelf.SetTrigger("Punch");
        audioSource.clip = weaponProperty.punch[Random.Range(0,weaponProperty.punch.Length)];
        audioSource.Play();
    }

    public void PunchWeapon(){
        if(Physics.Raycast(_cameraTransform.position,_cameraTransform.forward, out RaycastHit hit,weaponProperty.punchRange, layerMask)){

            if(hit.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                enemy.HitboxTakeDamage(weaponProperty.punchDamage*playerStats.strength,-hit.normal,playerStats);
                SpawnParticle(weaponProperty.hitParticle_enemy,hit.point,hit.normal);
                return;
            }
            SpawnParticle(weaponProperty.hitParticle_wall,hit.point,hit.normal);
        }
    }

    public void Cocking(){
        audioSource.clip = weaponProperty.cock;
        audioSource.Play();
    }
    
    [ServerRpc]
    public virtual void SpawnParticle(NetworkObject hitParticle,Vector3 _point, Vector3 _normal){
        NetworkObject particle = Instantiate(hitParticle,_point,Quaternion.LookRotation(_normal));
        ServerManager.Spawn(particle);
    }

    [ServerRpc]
    public virtual void AnimateWeaponShoot(){
        AnimateWeaponShootObserver();
    }

    [ObserversRpc]
    private void AnimateWeaponShootObserver(){
        weaponOther.SetTrigger("Fire");
        if(muzzleFlashOther!=null)
            muzzleFlashOther.Play();
        audioSourceOther.clip = weaponProperty.shoot[Random.Range(0,weaponProperty.shoot.Length)];
        audioSourceOther.Play();
    }

    [ServerRpc]
    public virtual void AnimateWeaponReload(bool _empty){
        AnimateWeaponReloadObserver(_empty);
    }

    [ObserversRpc]
    private void AnimateWeaponReloadObserver(bool _empty){
        if(_empty){
            audioSourceOther.clip = weaponProperty.reloadLast;
        }else{
            audioSourceOther.clip = weaponProperty.reload;
        }
        audioSourceOther.Play();
    }



}
