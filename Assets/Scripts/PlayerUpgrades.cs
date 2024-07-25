using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;

public class PlayerUpgrades : NetworkBehaviour
{
    [SerializeField] private UpgradesPanel upgradesPanel;
    
    private LevelManager levelManager;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerAbility playerAbility;

    void Awake(){
        levelManager=FindObjectOfType<LevelManager>();
        
    }

    public override void OnStartClient(){
        base.OnStartClient();
    }
    public override void OnStopClient(){
        base.OnStopClient();

        if(base.IsServerInitialized && leveling){
            levelManager.SetPlayerDone(1);
        }
    }


    private bool leveling = false;
    public void PlayerLevelingUp(){
        if(base.IsOwner && !leveling){
            leveling=true;
            playerInput.SwitchCurrentActionMap("InMenu");
            upgradesPanel.transform.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            upgradesPanel.ShowCrads();
        }
    }
    public void PlayerLevelingUpEnd(){
        if(base.IsOwner){
            leveling=false;
            playerInput.SwitchCurrentActionMap("InGame");
            upgradesPanel.transform.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            levelManager.SetPlayerDone(1);
        }
    }    

    public void SelectUpgrade(int _id){
        if(!base.IsOwner) return;
        switch (_id)
        {
            case 0:
                TheFool();
            break;
            case 1:
                TheMagician();
            break;
            case 2:
                TheHighPriestess();
            break;
            case 3:
                TheEmpress();
            break;
            case 4:
                TheEmperor();
            break;
            case 7:
                TheChariot();
            break;
            case 8:
                Strength();
            break;
            case 16:
                TheTower();
            break;
            case 19:
                TheSun();
            break;
            default:
                Debug.LogError("ID: "+_id+" does not exists!");
            break;
        }
        if(_id!=0)
            PlayerLevelingUpEnd();
    }


    private void TheFool(){
        upgradesPanel.ShowCradsTheFool();
    }

    private void TheMagician(){
        playerStats.reloadSpeed+=0.05f;
    }

    private void TheHighPriestess(){
        playerStats.revives++;
    }

    private void TheEmpress(){
        playerStats.healPower+=0.10f;
    }

    private void TheEmperor(){
        playerStats.damageReduction+=0.05f;
    }

    private void TheChariot(){
        playerStats.movementSpeed+=0.10f;
    }

    private void Strength(){
        playerStats.strength+=0.10f;
    }
    private void TheTower(){
        Debug.Log("Tower");
        playerAbility.SelectAbility(16,0.1f,0.1f,0.1f,0.1f);
    }
    private void TheSun(){
        playerStats.maxHealth+=10;
        playerStats.HealPlayer(10);
    }
}
