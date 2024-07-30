using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public abstract class Ability : NetworkBehaviour
{
    public CardProperty cardProperty;
    public PlayerStats playerStats;

    [Space]
    public int strengthLevel=0; 
    public float strength=0f; 
    public float strengthUpgrade = 0f;
    [Space]
    public int rangeLevel=0; 
    public float range=0f;
    public float rangeUpgrade = 0f;
    [Space]
    public int durationLevel=0; 
    public float duration=0f; 
    public float durationUpgrade = 0f;
    [Space]
    public int cooldownLevel=0; 
    public float cooldown=0f;
    public float cooldownUpgrade = 0f;
    
    [HideInInspector] public float currentCooldown=0;

    public abstract void UseAbility();
    public void UpgradeStrength(){
        strengthLevel++;
        strength+=strength*(strengthUpgrade/100);
    }
    public void UpgradeRange(){
        rangeLevel++;
        range+=rangeUpgrade;
    }
    public void UpgradeDuration(){
        durationLevel++;
        duration+=durationUpgrade;
    }
    public void UpgradeCooldown(){
        cooldownLevel++;
        cooldown-=cooldownUpgrade;
    }


}
