using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FishNet.Object;

public class Enemy : NetworkBehaviour
{
    public float health = 100f;
    public float attackRange=2f;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public Transform player;

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
        if(navMeshAgent!=null && health>0)
            GoToPosition(GetClosestPlayerPosition());
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
    public void TakeDamage(int damage, Vector3 direction, Enemy_Hitbox _rigidbody,PlayerStats _playerStats){
        if(health<=0) return;
        _playerStats.SetDamageDealt(damage);
        health-=damage;
        if(health <= 0){
            if(enemySpawner!=null){
                enemySpawner.currentEnemyCount--;
                enemySpawner.SetEnemyCount(-1);
                _playerStats.SetKills(1);
            }
            DeathRagdoll(damage,direction,_rigidbody);
        }
    }

    private void Die(){
        ServerManager.Despawn(gameObject);
    }

    [ObserversRpc]
    private void DeathRagdoll(int damage, Vector3 direction, Enemy_Hitbox _rigidbody){
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
        }
        _rigidbody.rigidbody.AddForce(direction*damage*50f);
        Destroy(gameObject,25f);
    }

}
