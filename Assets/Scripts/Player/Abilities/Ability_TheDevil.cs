using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Ability_TheDevil : Ability
{
    [Space]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Trap fireBall;
    [SerializeField] private Transform _cameraTransform;

    void FixedUpdate(){
        if(currentCooldown>0){
            currentCooldown-=Time.fixedDeltaTime;
        }
    }
    public override void UseAbility(){
        Debug.Log("TheDevil");
        if(currentCooldown>0) return;
        if(Physics.Raycast(_cameraTransform.position,_cameraTransform.forward, out RaycastHit hit,7.5f, layerMask)){
            currentCooldown=cooldown;
            SpawnAbility(fireBall,hit.point,hit.normal,strength,range,duration);
        }
    }

    [ServerRpc]
    public virtual void SpawnAbility(Trap _fireBall,Vector3 _point, Vector3 _normal,float _damage, float _range, float _duration){
        Trap fireBall = Instantiate(_fireBall,_point,Quaternion.LookRotation(_normal));
        fireBall.Initialize(_damage,_range,_duration,playerStats);
        ServerManager.Spawn(fireBall.gameObject);
    }
}
