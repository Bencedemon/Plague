using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FishNet.Object;

public class Enemy : NetworkBehaviour
{
    public float health = 100f;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public Transform player;

    public Rigidbody[] rigidbodies;

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
        return closestPlayerPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    private void GoToPosition(Vector3 _position){
        navMeshAgent.SetDestination(_position);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(int damage){
        if(health<=0) return;
        health-=damage;
        if(health <= 0){
            DeathRagdoll();
        }
    }

    private void Die(){
        ServerManager.Despawn(gameObject);
    }

    [ObserversRpc]
    private void DeathRagdoll(){
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
        Destroy(gameObject,25f);
    }

}
