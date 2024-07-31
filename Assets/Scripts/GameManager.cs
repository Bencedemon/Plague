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
    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips_tick;
    [SerializeField] private AudioClip audioClipBell;

    [SerializeField] private Animator fadeIn;

    private bool gameStarted = false;
    private bool gameEnded = false;

    private PlayerMovement[] playerMovements;
    private PlayerManager playerManager;
    private DiscordManager discordManager;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();
        discordManager=FindObjectOfType<DiscordManager>();
        if(discordManager!=null){
            discordManager.details="In Game";
            if(playerManager.Clients.Count==1){
                discordManager.state="Playing Solo (1 of 4)";
            }else{
                discordManager.state="Playing Multiplayer ("+playerManager.Clients.Count+" of 4)";
            }
            discordManager.largeImage = "ingame";
            discordManager.largeText = "Currently in Game";
        }
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
                fadeIn.SetTrigger("FadeIn");
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
        GameObject player = Instantiate(playerPrefab,new Vector3(-70+Random.Range(-3,3),1.04f,180+Random.Range(-3,3)),Quaternion.Euler(0, Random.Range(45f,135f), 0));
        Spawn(player,conn);
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        countDownText.text = "5";
        audioSource.clip=audioClips_tick[Random.Range(0,audioClips_tick.Length)];
        audioSource.Play();
        
        yield return new WaitForSeconds(1f);
        countDownText.text = "4";
        audioSource.clip=audioClips_tick[Random.Range(0,audioClips_tick.Length)];
        audioSource.Play();
        
        yield return new WaitForSeconds(1f);
        countDownText.text = "3";
        audioSource.clip=audioClips_tick[Random.Range(0,audioClips_tick.Length)];
        audioSource.Play();
        
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        audioSource.clip=audioClips_tick[Random.Range(0,audioClips_tick.Length)];
        audioSource.Play();

        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        audioSource.clip=audioClips_tick[Random.Range(0,audioClips_tick.Length)];
        audioSource.Play();
        
        yield return new WaitForSeconds(1f);

        playerMovements = FindObjectsOfType<PlayerMovement>();

        foreach (var player in playerMovements)
        {
            player.canMove=true;
        }

        countDownText.text = "";
        audioSource.clip=audioClipBell;
        audioSource.Play();

        if(base.IsServerInitialized){
            //countDownText.text = "GameStarted";
            StartCoroutine(enemySpawner.StartNewWave());
        }
    }

    private void EndGame(){
        sceneLoader.StartLoading("00_MainMenu");
    }
}
