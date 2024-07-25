using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] public CardProperty cardProperty;
    [SerializeField] private PlayerStats playerStats;

    public float strength=0f; 
    public float range=0f;
    public float duration=0f; 
    public float cooldown=0f;

    public abstract void UseAbility();
}
