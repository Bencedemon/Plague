using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class EnemyDamage : NetworkBehaviour
{
    [SerializeField] private int damage = 10;

    public Transform attackPoint;

    public float attackRadius;

    public LayerMask layerMask;

    public Collider[] hitColliders;
    private float cooldown = 2f;

    void FixedUpdate(){
        if(!IsServer)
            return;
            
        if(cooldown>0){
            cooldown-=Time.fixedDeltaTime;
        }

        if(cooldown<=0){
            doDamage();
            cooldown=2f;
        }
    }
    
    public void doDamage(){
        if(!IsServer)
            return;

        hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius,layerMask);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.TryGetComponent(out PlayerStats playerStats)){
                playerStats.TakeDamage(damage);
            }
        }
    }
}
