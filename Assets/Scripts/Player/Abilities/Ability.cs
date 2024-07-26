using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public abstract class Ability : NetworkBehaviour
{
    [SerializeField] public CardProperty cardProperty;
    [SerializeField] public PlayerStats playerStats;

    [Space]
    public int strengthLevel=0; 
    public float strength=0f; 
    [Space]
    public int rangeLevel=0; 
    public float range=0f;
    [Space]
    public int durationLevel=0; 
    public float duration=0f; 
    [Space]
    public int cooldownLevel=0; 
    public float cooldown=0f;
    
    [HideInInspector] public float currentCooldown=0;

    public abstract void UseAbility();
}
