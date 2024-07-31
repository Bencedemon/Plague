using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStart : MonoBehaviour
{
    [Header("Start")]

    [SerializeField] private Animator fade;

    [Header("GameEnd")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text kills,damageDealt,damageTaken,deaths,xp;
    private PlayerPerformance playerPerformance;
    
    [Space]
    [SerializeField] private GameObject mainMenu;

    private Client[] clients;

    private DiscordManager discordManager;

    void Awake(){
        playerPerformance=FindObjectOfType<PlayerPerformance>();
        discordManager=FindObjectOfType<DiscordManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(discordManager!=null){
            discordManager.details="In Menu";
            discordManager.state="";
            discordManager.largeImage = "inmenu";
            discordManager.largeText = "Main Menu";
        }
        Cursor.lockState = CursorLockMode.None;
        clients = FindObjectsOfType<Client>();
        if(clients.Length == 0){
            Debug.Log("First Start");
            mainMenu.SetActive(true);
        }else{
            Debug.Log("Not First Start");
            mainMenu.SetActive(false);
            endScreen.SetActive(true);
            kills.text = ""+playerPerformance.kills;
            damageDealt.text = ""+playerPerformance.damageDealt;
            damageTaken.text = ""+playerPerformance.damageTaken;
            deaths.text = ""+playerPerformance.deaths;

            int experiance = playerPerformance.kills*10+(int)playerPerformance.damageDealt;
            xp.text = "+"+experiance+" xp";
        }
        fade.Play("FadeIn");
    }

    public void Continue(){
        endScreen.SetActive(false);
        playerPerformance.kills=0;
        playerPerformance.damageDealt=0;
        playerPerformance.damageTaken=0;
        playerPerformance.deaths=0;
        foreach (var client in clients)
        {
            client.backToLobby();
        }
    }
}
