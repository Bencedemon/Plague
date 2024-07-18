using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScorePlayerCard : MonoBehaviour
{
    [SerializeField] private Image pp;
    [SerializeField] private TMP_Text name,health,kills;

    private PlayerStats playerStats;
    private PlayerProfileManager playerProfileManager;

    public void Initialize(PlayerStats _playerStats){
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();
        playerStats = _playerStats;

        pp.sprite = playerProfileManager.sprites[playerStats.pictureId.Value];
        name.text = ""+playerStats.playerName.Value;
        health.text = "100";
        kills.text = "0";
    }

    void FixedUpdate(){
        if(playerStats!=null){
            pp.sprite = playerProfileManager.sprites[playerStats.pictureId.Value];
            name.text = ""+playerStats.playerName.Value;
            health.text = ""+playerStats._currentHealth.Value;
            kills.text = ""+playerStats.kills.Value;
        }else{
            name.text = "Disconected";
        }
    }


}
