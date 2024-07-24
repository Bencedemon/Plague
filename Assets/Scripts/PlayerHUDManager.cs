using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [Space]
    [SerializeField] private Image crosshair;

    [Space]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject nearDeadVolume;
    [SerializeField] private Animator borderAnimator;

    [Space]
    [SerializeField] private GameObject reviveIcon;
    [SerializeField] private TMP_Text reviveCount;

    [Space]
    private EnemySpawner enemySpawner;
    [Space]
    [SerializeField] private Animator enemyCountPanel;
    [SerializeField] private TMP_Text enemyCount;


    private LevelManager levelManager;
    [Space]
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private TMP_Text level;
    [SerializeField] private Image blood;

    private PlayerProfileManager playerProfileManager;

    void Awake(){
        enemySpawner=FindObjectOfType<EnemySpawner>();
        levelManager=FindObjectOfType<LevelManager>();
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();
        crosshair.sprite = playerProfileManager.crosshairs[playerProfileManager.playerProfile.crosshairId];
        crosshair.color = new Color(
            playerProfileManager.playerProfile.crosshairColor[0],
            playerProfileManager.playerProfile.crosshairColor[1],
            playerProfileManager.playerProfile.crosshairColor[2],
            playerProfileManager.playerProfile.crosshairColor[3]
        );
    }


    void FixedUpdate()
    {
        healthText.text="+"+playerStats._currentHealth.Value;

        reviveCount.text=""+playerStats.revives;
        reviveIcon.SetActive(playerStats.revives>0);

            
        borderAnimator.SetFloat("health", playerStats._currentHealth.Value);
        nearDeadVolume.SetActive(playerStats._currentHealth.Value<=10);

        if(enemySpawner!=null){
            enemyCountPanel.SetInteger("count",enemySpawner.enemyCount.Value);
            enemyCount.text=""+enemySpawner.enemyCount.Value;
        }

        if(levelManager!=null){
            //level.text="XP: "+levelManager._experiance.Value+" Level: "+levelManager._level.Value+" NextLevel: "+levelManager._nextLevel.Value;
            percentage.text=(int)((levelManager._experiance.Value/levelManager._nextLevel.Value)*100)+"%";
            if(levelManager._experiance.Value<=0){
                blood.fillAmount = 0f;
            }else{
                blood.fillAmount = levelManager._experiance.Value/levelManager._nextLevel.Value;
            }
        }

    }
}
