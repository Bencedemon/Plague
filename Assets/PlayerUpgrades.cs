using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerUpgrades : NetworkBehaviour
{
    public UpgradesPanel upgradesPanel;
    
    private LevelManager levelManager;

    void Awake(){
        levelManager=FindObjectOfType<LevelManager>();
        
    }

    public override void OnStartClient(){
        base.OnStartClient();
    }
    public void PlayerLevelingUp(){
        if(base.IsOwner){
            upgradesPanel.transform.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void PlayerLevelingUpEnd(){
        if(base.IsOwner){
            upgradesPanel.transform.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            levelManager.SetPlayerDone(1);
        }
    }    
}
