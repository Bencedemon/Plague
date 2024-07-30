using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class LevelManager : NetworkBehaviour
{
    public readonly SyncVar<float> _experiance = new(0);
    public readonly SyncVar<int> _level = new(1);
    public readonly SyncVar<float> _nextLevel = new(100);

    private bool levelingUp = false;
    private bool levelingUpEnd = false;

    public readonly SyncVar<int> _playerDone = new(0);

    [Header("Time")]
    public float slowDonwLength = 5f;


    private PlayerManager playerManager;
    void Awake(){
        playerManager=FindObjectOfType<PlayerManager>();
    }
    public override void OnStartClient(){
        base.OnStartClient();
        if(base.IsServerInitialized){
            _experiance.OnChange += OnExperianceChanged;;
            _playerDone.OnChange += OnPlayerDoneChanged;;
            SetNextLevel(100*playerManager.Clients.Count);
        }
    }

    void Update(){
        if(!base.IsServerInitialized) return;
        if(Time.timeScale<1 && levelingUpEnd){
            speedUpTime((1f / slowDonwLength)*Time.unscaledDeltaTime);
        }else
        if(Time.timeScale>0 && levelingUp){
            slowDownTime((1f / slowDonwLength)*Time.unscaledDeltaTime);
        }
    }

    [ServerRpc(RequireOwnership = false)] public void AddExperiance(float _xp) => _experiance.Value += _xp;
    [ServerRpc(RequireOwnership = false)] public void SetExperiance(float _xp) => _experiance.Value = _xp;
    [ServerRpc(RequireOwnership = false)] public void RemoveExperiance(float _xp) => _experiance.Value -= _xp;
    [ServerRpc(RequireOwnership = false)] public void SetLevel(int _lvl) => _level.Value += _lvl;
    [ServerRpc(RequireOwnership = false)] public void SetNextLevel(float _nlvl) => _nextLevel.Value = _nlvl;
    [ServerRpc(RequireOwnership = false)] public void SetPlayerDone(int _done) => _playerDone.Value += _done;
    [ServerRpc(RequireOwnership = false)] public void ResetPlayerDone() => _playerDone.Value = 0;

    
    private void OnExperianceChanged(float oldValue, float newValue, bool asServer){
        if(newValue>=_nextLevel.Value && !levelingUp){
            levelingUp=true;
            Debug.Log("LevelUp");
            LevelUp();
        }
    }

    private void LevelUp(){
        float extra = _experiance.Value-_nextLevel.Value;
        SetExperiance(extra);
        SetLevel(+1);
        SetNextLevel((100+(20*_level.Value))*playerManager.Clients.Count);
    }

    
    [ServerRpc(RequireOwnership = false)] 
    private void slowDownTime(float _time){
        slowDownTimeObserver(_time);
    }
    [ObserversRpc]
    private void slowDownTimeObserver(float _time){
        if(Time.timeScale-_time<=0){
            Time.timeScale=0;
        }else{
            Time.timeScale -= _time;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f,1f);
        }

        if(Time.timeScale<=0){
            foreach (var player in playerManager.PlayerGameObject)
            {
                if (player.TryGetComponent<PlayerUpgrades>(out PlayerUpgrades playerUpgrade))
                {
                    playerUpgrade.PlayerLevelingUp();
                    levelingUp=false;
                }
            }
        }
    }
    private void OnPlayerDoneChanged(int oldValue, int newValue, bool asServer){
        if(newValue==playerManager.PlayerGameObject.Count && !levelingUpEnd){
            levelingUpEnd=true;
        }
    }

    [ServerRpc(RequireOwnership = false)] 
    private void speedUpTime(float _time){
        speedUpTimeObserver(_time);
    }
    [ObserversRpc]
    private void speedUpTimeObserver(float _time){
        if(Time.timeScale+_time>=1){
            Time.timeScale=1;
        }else{
            Time.timeScale += _time;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f,1f);
        }

        if(Time.timeScale>=1){
            levelingUpEnd=false;
            ResetPlayerDone();
        }
    }
}
