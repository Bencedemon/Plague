using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    //public int _currentHealth;
    public readonly SyncVar<int> _currentHealth = new(100);

    [SerializeField] private GameObject hud,scoreBoard, mouseLook;
    

    [Header("PlayerProfile")]
    public readonly SyncVar<int> pictureId = new();
    public readonly SyncVar<string> playerName = new();
    public readonly SyncVar<int> kills = new(0);
    private PlayerProfileManager playerProfileManager;


    [Space]
    [SerializeField] private PlayerSpectator playerSpectator;
    [SerializeField] private PlayerInput playerInput;

    [Space]
    [SerializeField] private GameObject deadBodyPrefab;
    public GameObject deadBodyReference; 

    void Awake(){
        //_currentHealth = maxHealth;
    }

    public override void OnStartClient(){
        base.OnStartClient();
        if(IsOwner){
            playerProfileManager=FindObjectOfType<PlayerProfileManager>();
            SetPictureId(playerProfileManager.playerProfile.pictureId);
            SetPlayerName(playerProfileManager.playerProfile.playerName);
            hud.SetActive(true);
            scoreBoard.SetActive(true);
            mouseLook.SetActive(true);
        }
    }

    public void TakeDamage(int damage){
        if(_currentHealth.Value<=0) return;
        if(_currentHealth.Value-damage <= 0){
            SetHealth(0);
            Spawn();
            Die(Owner);
        }else{
            SetHealth(_currentHealth.Value-damage);
        }
    }
/*
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
    }*/

    [TargetRpc]
    private void Die(NetworkConnection conn){
        playerSpectator.spectator.SetActive(true);
        playerInput.SwitchCurrentActionMap("InSpectate");
        mouseLook.SetActive(false);
        Debug.Log("Dead");
    }

    [ServerRpc(RequireOwnership=false)]
    void Spawn(){
        deadBodyReference = Instantiate(deadBodyPrefab,transform.position,transform.rotation);
        Spawn(deadBodyReference);
    }

    
    [ServerRpc(RequireOwnership = false)] private void SetHealth(int _health) => _currentHealth.Value = _health;
    [ServerRpc] private void SetPictureId(int _pictureId) => pictureId.Value = _pictureId;
    [ServerRpc] private void SetPlayerName(string _playerName) => playerName.Value = _playerName;
    [ServerRpc(RequireOwnership = false)] public void SetKills(int _kills) => kills.Value += _kills;
}
