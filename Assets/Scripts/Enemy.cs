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

    public override void OnStartClient(){
        base.OnStartClient();

        if(!IsServerInitialized){
            enabled = false;
            return;
        }
    }


    void FixedUpdate(){
        if(navMeshAgent!=null)
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
        health-=damage;

        if(health < 0){
            Die();
        }
    }

    private void Die(){
        ServerManager.Despawn(gameObject);
    }

}
