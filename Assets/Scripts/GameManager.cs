using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using TMPro;

public class GameManager : NetworkBehaviour
{
    private PlayerManager playerManager;

    [SerializeField] private NetworkObject enemySpawnerPrefab;

    public TMP_Text countDownText;

    private PlayerMovement[] playerMovements;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public override void OnStartClient(){
        base.OnStartClient();
        
        playerMovements = FindObjectsOfType<PlayerMovement>();
        StartCoroutine(CountDown());
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
            //NetworkObject enemySpawner = Instantiate(enemySpawnerPrefab, new Vector3(-57f,0.0306921005f,179f), Quaternion.identity);
            //ServerManager.Spawn(enemySpawner);
        }
    }
}
