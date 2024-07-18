using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Heal : Item
{
    [SerializeField] private int healPowers=10;
    public override void PickUp(Collider player){
        if (player.transform.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
        {
            if(playerStats._currentHealth.Value<=0) return;
            if(playerStats._currentHealth.Value<playerStats.maxHealth){
                playerStats.HealPlayer(healPowers);
                DespawnObject(gameObject);
            }
        }
    }
}
