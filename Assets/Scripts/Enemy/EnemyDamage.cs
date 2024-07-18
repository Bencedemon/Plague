using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class EnemyDamage : NetworkBehaviour
{
    
    [SerializeField] private Enemy enemy;

    [SerializeField] private int damage = 10;

    [SerializeField] private Transform attackPoint;

    [SerializeField] private float attackRadius;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Collider[] hitColliders;
    
    [SerializeField] private string[] attacks;


    private bool inAction = false;
    void FixedUpdate(){
        if(!IsServer)
            return;
            
        if(!inAction){
            hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius,layerMask);
            bool hasPlayer=false;
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.gameObject.TryGetComponent(out PlayerStats playerStats)){
                    hasPlayer=true;
                }
            }

            if(hasPlayer){
                DoAttackServer(Random.Range(0,attacks.Length));
            }
        }
    }

    [ServerRpc(RequireOwnership=false)]
    public void DoAttackServer(int _id){
        DoAttackObserver(_id);
    }
    [ObserversRpc]
    public void DoAttackObserver(int _id){
        inAction = true;
        enemy.animator.Play(attacks[_id]);
    }
    
    public void doDamage(){
        if(!IsServer)
            return;

        hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius,layerMask);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.TryGetComponent(out PlayerStats playerStats)){
                playerStats.TakeDamage(Random.Range(damage-5,damage+5));
            }
        }
    }
    public void ActionEnd(){
        inAction=false;
    }
}
