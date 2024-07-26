using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Trap : NetworkBehaviour
{
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private NetworkObject explosionParticle;
    [SerializeField] private Collider collider;

    private float strength=0f; 
    private float range=0f;
    private float duration=0f;
    private PlayerStats playerStats;

    public override void OnStartClient(){
        base.OnStartClient();
        if(IsServer){
            collider.enabled = true;
        }
    }

    public void Initialize(float _strength, float _range, float _duration, PlayerStats _playerStats){
        strength = _strength;
        range = _range;
        duration = _duration;
        playerStats = _playerStats;

        Destroy(gameObject,_duration);
    }

    private bool activated=false;
    private void OnTriggerEnter(Collider other)
    {
        if(!base.IsServer) return;
        if(!activated){
            activated=true;
            Debug.Log("activated");
            SpawnParticle(explosionParticle,transform.position,gameObject);
            ActivateTrap();
        }
    }

    private void ActivateTrap(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                if(enemy.tag=="EnemyCore"){
                    enemy.enemy.TakeDamage(strength,Vector3.zero,playerStats,enemy);
                }
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnParticle(NetworkObject _explosion,Vector3 _point,GameObject _gameObject){
        Debug.Log("Destroy");
        NetworkObject explosion = Instantiate(_explosion,_point, Quaternion.identity);
        ServerManager.Spawn(explosion);
        ServerManager.Despawn(_gameObject);
    }
}
