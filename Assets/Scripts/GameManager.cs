using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using TMPro;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private  EnemySpawner enemySpawner;
    [Space]
    [SerializeField] private TMP_Text countDownText;

    [SerializeField] private GameObject playerPrefab;
    [Space]
    [SerializeField] private FishNet.Example.Scened.SceneLoader sceneLoader;

    private bool gameStarted = false;
    private bool gameEnded = false;

    private PlayerMovement[] playerMovements;
    private PlayerManager playerManager;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();

    }
    public override void OnStartClient(){
        base.OnStartClient();

        Spawn(LocalConnection);
    }

    void FixedUpdate(){
        if(playerManager==null) return;
        if(!gameStarted){
            if(playerManager.PlayerGameObject.Count==playerManager.Clients.Count){
                gameStarted=true;
                StartCoroutine(CountDown());
            }
        }else if(!gameEnded){
            int count=0;
            foreach (var player in playerManager.PlayerGameObject)
            {
                if(player.GetComponent<PlayerStats>()._currentHealth.Value<=0){
                    count++;
                }
            }
            if(count==playerManager.PlayerGameObject.Count){
                gameEnded=true;
                if(base.IsServerInitialized){
                    EndGame();
                }
            }
        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void Spawn(NetworkConnection conn){
        GameObject player = Instantiate(playerPrefab,new Vector3(-70+Random.Range(-3,3),1.04f,180+Random.Range(-3,3)),Quaternion.identity);
        Spawn(player,conn);
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

        playerMovements = FindObjectsOfType<PlayerMovement>();

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

    private void EndGame(){
        sceneLoader.StartLoading("00_MainMenu");
    }
}
