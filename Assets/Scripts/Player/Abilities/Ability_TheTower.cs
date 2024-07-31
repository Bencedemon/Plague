using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_TheTower : Ability
{
    [Space]
    public LayerMask layerMask;

    [SerializeField] private GameObject effect;
    private float currentDuration=0;
    void FixedUpdate(){
        if(currentCooldown>0){
            currentCooldown-=Time.fixedDeltaTime;
        }
    }
    public override void UseAbility(){
        Debug.Log("TheTower");
        if(currentCooldown>0) return;
        StartCoroutine(Curse());
    }
    private IEnumerator Curse()
    {
        effect.transform.localScale = new Vector3(range*2,range*2,range*2);
        effect.SetActive(true);
        currentCooldown=cooldown;
        currentDuration=duration;
        while (currentDuration>0)
        {
            effect.transform.localScale = new Vector3(range*2,range*2,range*2);
            currentDuration-=0.25f;
            yield return new WaitForSeconds(0.25f);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, layerMask);
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.transform.TryGetComponent(out Enemy_Hitbox enemy)){
                    if(enemy.tag=="EnemyCore"){
                        enemy.enemy.TakeDamage(strength,Vector3.zero,playerStats);
                    }
                }
            }
        }
        effect.SetActive(false);
        currentDuration=duration;
    }
}
