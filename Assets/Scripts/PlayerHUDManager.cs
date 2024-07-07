using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerWeapon playerWeapon;

    public TMP_Text healthText;
    public TMP_Text ammoText;
    public GameObject nearDeadVolume;
    public Animator borderAnimator;

    [Space]
    private EnemySpawner enemySpawner;
    [Space]
    public Animator enemyCountPanel;
    public TMP_Text enemyCount;
    void Awake(){
        enemySpawner=FindObjectOfType<EnemySpawner>();
    }

    void FixedUpdate()
    {
        healthText.text=""+playerStats._currentHealth.Value;
        if(playerWeapon.currentWeapon.weaponProperty.maxAmmo!=0)
            ammoText.text=playerWeapon.currentWeapon.currentAmmoCount+"/"+playerWeapon.currentWeapon.weaponProperty.maxAmmo;
        else
            ammoText.text="";

            
        borderAnimator.SetFloat("health", playerStats._currentHealth.Value);
        nearDeadVolume.SetActive(playerStats._currentHealth.Value<=10);

        if(enemySpawner!=null){
            enemyCountPanel.SetInteger("count",enemySpawner.enemyCount.Value);
            enemyCount.text=""+enemySpawner.enemyCount.Value;
        }
    }
}
