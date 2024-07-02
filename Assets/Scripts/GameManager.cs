using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using TMPro;

public class GameManager : NetworkBehaviour
{
    private PlayerManager playerManager;

    private EnemySpawner enemySpawner;

    public TMP_Text countDownText;

    private PlayerMovement[] playerMovements;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public override void OnStartClient(){
        base.OnStartClient();
        
        playerMovements = FindObjectsOfType<PlayerMovement>();
        StartCoroutine(CountDown());
    }

    [ServerRpc]
    public void StartGame(){

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
            enemySpawner.SpawnEnemy();
        }
    }
}
