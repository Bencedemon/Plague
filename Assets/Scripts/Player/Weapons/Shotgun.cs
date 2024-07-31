using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : AWeapon
{
    public override void FireWeapon(){
        currentAmmoCount--;
        if(muzzleFlashSelf!=null)
            muzzleFlashSelf.Play();

        audioSource.clip = weaponProperty.shoot[Random.Range(0,weaponProperty.shoot.Length)];
        audioSource.Play();

        RaycastHit hit;
        if(Physics.Raycast(_cameraTransform.position,_cameraTransform.forward,out hit,weaponProperty.maxRange, layerMask)){
            if(bulletTrail!=null){
                TrailRenderer trail = Instantiate(bulletTrail,muzzleFlashSelf.transform.position,Quaternion.identity);
                StartCoroutine(trail.transform.GetComponent<BulletTrail>().SpawnTrail(trail,hit.point));
            }
            if(hit.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                Vector3 direction = (hit.point-_cameraTransform.position).normalized;
                enemy.HitboxTakeDamage(weaponProperty.damage*playerStats.strength,direction,playerStats);
                SpawnParticle(weaponProperty.hitParticle_enemy,hit.point,hit.normal);
            }else
                SpawnParticle(weaponProperty.hitParticle_wall,hit.point,hit.normal);
        }else{
            if(bulletTrail!=null){
                TrailRenderer trail = Instantiate(bulletTrail,muzzleFlashSelf.transform.position,Quaternion.identity);
                StartCoroutine(trail.transform.GetComponent<BulletTrail>().SpawnTrail(trail,_cameraTransform.position+_cameraTransform.forward*weaponProperty.maxRange));
            }
        }
        for (int i = 0; i < 9; i++)
        {
            float x = Random.Range(-weaponProperty.spread,weaponProperty.spread);
            float y = Random.Range(-weaponProperty.spread,weaponProperty.spread);
            float z = Random.Range(-weaponProperty.spread,weaponProperty.spread);

            Vector3 directionWithSpread = _cameraTransform.forward + new Vector3(x,y,z);

            if(Physics.Raycast(_cameraTransform.position,directionWithSpread,out hit,weaponProperty.maxRange, layerMask)){
                if(bulletTrail!=null){
                    TrailRenderer trail = Instantiate(bulletTrail,muzzleFlashSelf.transform.position,Quaternion.identity);
                    StartCoroutine(trail.transform.GetComponent<BulletTrail>().SpawnTrail(trail,hit.point));
                }
                //Debug.Log("Hit: "+hit.transform.name);
                if(hit.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                    Vector3 direction = (hit.point-_cameraTransform.position).normalized;
                    enemy.HitboxTakeDamage(weaponProperty.damage*playerStats.strength,direction,playerStats);
                    //Debug.Log("Enemy hit: "+hit.transform.name+" Damage: "+weaponProperty.damage);
                    SpawnParticle(weaponProperty.hitParticle_enemy,hit.point,hit.normal);
                }else
                    SpawnParticle(weaponProperty.hitParticle_wall,hit.point,hit.normal);
            }else{
                if(bulletTrail!=null){
                    TrailRenderer trail = Instantiate(bulletTrail,muzzleFlashSelf.transform.position,Quaternion.identity);
                    StartCoroutine(trail.transform.GetComponent<BulletTrail>().SpawnTrail(trail,_cameraTransform.position+directionWithSpread*weaponProperty.maxRange));
            }
        }
        }
    }
    public override void Reload(){
        if(inReload) return;
        if(weaponProperty.maxAmmo==0) return;
        if(currentTotalAmmo<=0) return;
        if(currentAmmoCount<weaponProperty.maxAmmo){
            weaponSelf.SetFloat("reloadSpeed",playerStats.reloadSpeed);
            inAction=true;
            inReload=true;
            Debug.Log("Reload");
            if(currentAmmoCount==0){
                AnimateWeaponReload(true);
                weaponSelf.SetTrigger("ReloadEmpty");
            }
            else{
                AnimateWeaponReload(false);
                weaponSelf.SetTrigger("Reload");
            }
        }
    }
    
    public override void ReloadWeapon(){
        weaponSelf.SetBool("reloading",true);
        audioSource.clip = weaponProperty.reload;
        audioSource.Play();

        currentAmmoCount++;
        currentTotalAmmo--;

        if(currentAmmoCount==weaponProperty.maxAmmo){
            weaponSelf.SetBool("reloading",false);
        }
        else
        if(currentTotalAmmo<=0){
            weaponSelf.SetBool("reloading",false);
        }
    }
}
