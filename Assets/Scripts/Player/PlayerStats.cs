using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public class PlayerStats : NetworkBehaviour
{
    public int maxHealth = 100;

    //public int _currentHealth;
    public readonly SyncVar<int> _currentHealth = new(100);

    [SerializeField] private GameObject hud,scoreBoard, mouseLook;
    public int revives = 0;
    

    [Header("PlayerProfile")]
    public readonly SyncVar<int> pictureId = new();
    public readonly SyncVar<string> playerName = new();
    public readonly SyncVar<int> kills = new(0);
    public readonly SyncVar<int> damageDealt = new(0);
    public readonly SyncVar<int> damageTaken = new(0);
    public readonly SyncVar<int> deaths = new(0);
    private PlayerPerformance playerPerformance;
    private PlayerProfileManager playerProfileManager;


    [Space]
    [SerializeField] private PlayerSpectator playerSpectator;
    [SerializeField] private PlayerInput playerInput;

    [Space]
    [SerializeField] private GameObject deadBodyPrefab;
    [SerializeField] private GameObject body;
    public GameObject deadBodyReference; 

    private PlayerManager playerManager;
    void Awake(){
        //_currentHealth = maxHealth;
        playerPerformance=FindObjectOfType<PlayerPerformance>();
        playerManager=FindObjectOfType<PlayerManager>();
        kills.OnChange += OnKillsChanged;;
        damageDealt.OnChange += OnDamageDealtChanged;;
        damageTaken.OnChange += OnDamageTakenChanged;;
        deaths.OnChange += OnDeathsChanged;;
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
            SetDamageTaken(_currentHealth.Value);
            SetHealth(0);
            SpawnDeadBody();
            Die(Owner);
        }else{
            SetDamageTaken(damage);
            SetDeaths(1);
            SetHealth(_currentHealth.Value-damage);
        }
    }

    [TargetRpc]
    private void Die(NetworkConnection conn){
        playerSpectator.spectator.SetActive(true);
        playerInput.SwitchCurrentActionMap("InSpectate");
        mouseLook.SetActive(false);
        Debug.Log("Dead");
    }

    public void ReviveFallenAlly(InputAction.CallbackContext context){
        if(_currentHealth.Value<=0) return;
        if(revives<=0) return;
        if(context.performed){
            PlayerStats[] stats = FindObjectsOfType<PlayerStats>();
            foreach (var stat in stats)
            {
                if(stat._currentHealth.Value<=0 && Vector3.Distance(stat.transform.position,transform.position)<=3f){
                    revives--;
                    stat.ReviveServer();
                    return;
                }
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ReviveServer(){
        PlayerBody(true);
        ServerManager.Despawn(deadBodyReference);
        Revive(Owner);
    }

    [TargetRpc]
    public void Revive(NetworkConnection conn){
        SetHealth(100);
        playerSpectator.spectator.SetActive(false);
        foreach (var player in playerManager.PlayerGameObject)
        {
            player.GetComponent<PlayerSpectator>().spectator.SetActive(false);
        }
        mouseLook.SetActive(true);
        playerInput.SwitchCurrentActionMap("InGame");
        Debug.Log("Revived");
    }

    public void HealPlayer(int heal){
        if(_currentHealth.Value+heal >= maxHealth){
            SetHealth(maxHealth);
        }else{
            SetHealth(_currentHealth.Value+heal);
        }
    }

    [ServerRpc(RequireOwnership=false)]
    void SpawnDeadBody(){
        PlayerBody(false);
        deadBodyReference = Instantiate(deadBodyPrefab,transform.position,transform.rotation);
        Spawn(deadBodyReference);
    }
    [ObserversRpc]
    private void PlayerBody(bool _active){
        body.SetActive(_active);
    }

    
    [ServerRpc(RequireOwnership = false)] private void SetHealth(int _health) => _currentHealth.Value = _health;
    [ServerRpc] private void SetPictureId(int _pictureId) => pictureId.Value = _pictureId;
    [ServerRpc] private void SetPlayerName(string _playerName) => playerName.Value = _playerName;
    [ServerRpc(RequireOwnership = false)] public void SetKills(int _kills) => kills.Value += _kills;
    [ServerRpc(RequireOwnership = false)] public void SetDamageDealt(int _damageDealt) => damageDealt.Value += _damageDealt;
    [ServerRpc(RequireOwnership = false)] public void SetDamageTaken(int _damageTaken) => damageTaken.Value += _damageTaken;
    [ServerRpc(RequireOwnership = false)] public void SetDeaths(int _deaths) => deaths.Value += _deaths;

    private void OnKillsChanged(int oldValue, int newValue, bool asServer){
        if(base.IsOwner){
            playerPerformance.kills=newValue;
        }
    }
    private void OnDamageDealtChanged(int oldValue, int newValue, bool asServer){
        if(base.IsOwner){
            playerPerformance.damageDealt=newValue;
        }
    }
    private void OnDamageTakenChanged(int oldValue, int newValue, bool asServer){
        if(base.IsOwner){
            playerPerformance.damageTaken=newValue;
        }
    }
    private void OnDeathsChanged(int oldValue, int newValue, bool asServer){
        if(base.IsOwner){
            playerPerformance.deaths=newValue;
        }
    }
}
