using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int _currentHealth;

    void Awake(){
        _currentHealth = maxHealth;
    }

    public override void OnStartClient(){
        base.OnStartClient();
        if(!IsOwner){
            enabled = false;
            return;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(int damage){
        _currentHealth -= damage;

        if(_currentHealth <= 0){
            Die();
        }
    }

    private void Die(){
        Debug.Log("Dead");
    }
}
