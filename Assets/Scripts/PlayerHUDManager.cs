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

    void FixedUpdate()
    {
        healthText.text=""+playerStats._currentHealth;
        if(playerWeapon.currentWeapon.weaponProperty.maxAmmo!=0)
            ammoText.text=playerWeapon.currentWeapon.currentAmmoCount+"/"+playerWeapon.currentWeapon.weaponProperty.maxAmmo;
        else
            ammoText.text="";
    }
}
