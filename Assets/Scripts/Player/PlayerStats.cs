using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    public int _currentHealth;

    [SerializeField] private GameObject hud;

    void Awake(){
        _currentHealth = maxHealth;
    }

    public override void OnStartClient(){
        base.OnStartClient();
        if(!IsOwner){
            enabled = false;
            return;
        }else{
            hud.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(int damage){
        if(_currentHealth-damage <= 0){
            _currentHealth=0;
            Die();
        }else{
            _currentHealth -= damage;
        }
        LocalTakeDamage(Owner,_currentHealth);
    }
    [TargetRpc]
    private void LocalTakeDamage(NetworkConnection conn, int newHealth){
        _currentHealth=newHealth;
    }

    private void Die(){
        Debug.Log("Dead");
    }
}
