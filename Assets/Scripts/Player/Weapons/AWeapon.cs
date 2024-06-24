using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public abstract class AWeapon : NetworkBehaviour
{
    public int damage;
    public float maxRange = 20f;
    public LayerMask layerMask;
    public Transform _cameraTransform;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private NetworkObject hitParticle;



    public void Fire(){
        AnimateWeapon();

        if(Physics.Raycast(_cameraTransform.position,_cameraTransform.forward, out RaycastHit hit,maxRange, layerMask)){
            Debug.Log("Shoot");
            SpawnParticle(hitParticle,hit.point,hit.normal);

            if(hit.transform.TryGetComponent(out Enemy enemy)){
                enemy.TakeDamage(damage);
            }
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
        if(muzzleFlash!=null)
            muzzleFlash.Play();
    }



}
