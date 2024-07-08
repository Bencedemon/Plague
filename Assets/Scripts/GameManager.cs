using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using TMPro;

public class GameManager : NetworkBehaviour
{
    private PlayerManager playerManager;

    private EnemySpawner enemySpawner;

    [SerializeField] private PlayerStats playerStats;

    [Space]

    [SerializeField] private TMP_Text countDownText;


    [Space]
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text damageDealtText;
    [SerializeField] private TMP_Text damageTakenText;
    [SerializeField] private TMP_Text deathsText;

    private PlayerMovement[] playerMovements;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public override void OnStartClient(){
        base.OnStartClient();
        
        playerMovements = FindObjectsOfType<PlayerMovement>();
        playerManager.GameManagers.Add(this);

        if(base.IsOwner)
            StartCoroutine(CountDown());
    }
    public override void OnStopClient(){
        base.OnStopClient();

        playerManager.GameManagers.Remove(this);
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        countDownText.text = "5";
        yield return new WaitForSeconds(1f);
        countDownText.text = "4";
        yield return new WaitForSeconds(1f);
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);

        foreach (var player in playerMovements)
        {
            player.canMove=true;
        }

        countDownText.text = "";

        if(base.IsServerInitialized){
            countDownText.text = "GameStarted";
            StartCoroutine(enemySpawner.StartNewWave());
        }
    }

    public void gameEnd(){
        if(base.IsOwner){
            killsText.text = ""+playerStats.kills.Value;

            Cursor.lockState = CursorLockMode.None;
            endPanel.SetActive(true);
        }
    }

    public void Continue(){
        DespawnPlayer();
        foreach (var item in playerManager.Players)
        {
            item.backToLobby();
        }
    }

    [ServerRpc]
    private void DespawnPlayer(){
        enemySpawner.ResetEnemySpawner();
        if(playerStats.deadBodyReference!=null){
            ServerManager.Despawn(playerStats.deadBodyReference);
        }
        ServerManager.Despawn(playerStats.transform.gameObject);
    }
}
