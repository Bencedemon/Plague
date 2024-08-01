using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FishNet.Object;

public class Enemy : NetworkBehaviour
{
    public float health = 100f;
    public float attackRange=2f;
    [SerializeField] private bool stopOnAttack=false;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public Transform player;

    [SerializeField] private NetworkObject organPrefab;


    public Rigidbody[] rigidbodies;
    public EnemySpawner enemySpawner;

    public override void OnStartClient(){
        base.OnStartClient();

        if(!IsServerInitialized){
            enabled = false;
            return;
        }
    }


    void FixedUpdate(){
        if(!IsServerInitialized) return;
        if(navMeshAgent!=null && health>0){
            if(!stopOnAttack){
                GoToPosition(GetClosestPlayerPosition());
            }
        }
    }

    private Vector3 GetClosestPlayerPosition(){
        Vector3 closestPlayerPosition = Vector3.zero;
        float closestDistance = Mathf.Infinity;
        foreach (var player in PlayerMovement.Players.Values)
        {
            if(Vector3.Distance(player.transform.position, transform.position) < closestDistance){
                closestDistance = Vector3.Distance(player.transform.position, transform.position);
                closestPlayerPosition = player.transform.position;
            }
        }

        animator.SetBool("walking", closestDistance<attackRange);

        return closestPlayerPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    private void GoToPosition(Vector3 _position){
        navMeshAgent.SetDestination(_position);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(float damage, Vector3 direction,PlayerStats _playerStats,Enemy_Hitbox _rigidbody=null){
        if(health<=0) return;
        _playerStats.SetDamageDealt(damage);
        health-=damage;
        if(health <= 0){
            if(enemySpawner!=null){
                enemySpawner.currentEnemyCount--;
                enemySpawner.SetEnemyCount(-1);
                _playerStats.SetKills(1);
            }
            NetworkObject organ = Instantiate(organPrefab, transform.position, Quaternion.identity);
            ServerManager.Spawn(organ);
            DeathRagdoll(damage,direction,_rigidbody);
        }
    }

    private void Die(){
        NetworkObject organ = Instantiate(organPrefab, transform.position, Quaternion.identity);
        ServerManager.Spawn(organ);
        ServerManager.Despawn(gameObject);
    }

    [ObserversRpc]
    private void DeathRagdoll(float damage, Vector3 direction, Enemy_Hitbox _rigidbody){
        Vector3 originalVelocity;
        
        if(animator.applyRootMotion)
            originalVelocity = animator.velocity;
        else
            originalVelocity = navMeshAgent.velocity;

        animator.enabled=false;
        navMeshAgent.enabled=false;
        foreach (var body in rigidbodies)
        {
            //body.useGravity=true;
            body.isKinematic=false;
            body.velocity = originalVelocity;
            body.gameObject.layer=16;
        }
        if(_rigidbody!=null)
            _rigidbody.rb.AddForce(direction*damage*25f);
        Destroy(gameObject,25f);
    }

}
