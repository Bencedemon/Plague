using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Exit : NetworkBehaviour
{

    private int players=0;
    [Space]
    [SerializeField] private FishNet.Example.Scened.SceneLoader sceneLoader;

    private PlayerManager playerManager;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();
    }
    public override void OnStartClient(){
        base.OnStartClient();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!IsServerInitialized) return;
        if(other.tag=="Player"){
            players++;

            int count = 0;
            foreach (var player in playerManager.PlayerGameObject)
            {
                if(player.transform.GetComponent<PlayerStats>()._currentHealth.Value>0){
                    count++;
                }
            }
            if(count == players && count != 0){
                EndGame();
            }
        }

    }
    void OnTriggerExit(Collider other)
    {
        if(!IsServerInitialized) return;
        if(other.tag=="Player"){
            players--;
        }
    }

    private void EndGame(){
        sceneLoader.StartLoading("00_MainMenu");
    }
}
