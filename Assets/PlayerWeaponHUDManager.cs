using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerWeaponHUDManager : MonoBehaviour
{
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private  TMP_Text ammoText,maxAmmoText,slash,totalAmmoText;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Animator weaponIconAnimator;
    private int currentWeaponIndex = 0;
    private bool changing = false;

    void Start(){
        currentWeaponIndex=playerWeapon._currentWeaponIndex.Value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentWeaponIndex!=playerWeapon._currentWeaponIndex.Value){
            currentWeaponIndex=playerWeapon._currentWeaponIndex.Value;
            changing=true;
            weaponIconAnimator.SetTrigger("switch");
        }
        if(changing) return;

        if(playerWeapon.currentWeapon.weaponProperty.maxAmmo!=0){
            ammoText.text=""+playerWeapon.currentWeapon.currentAmmoCount;
            maxAmmoText.text=""+playerWeapon.currentWeapon.weaponProperty.maxAmmo;
            slash.text="/";
            totalAmmoText.text=""+playerWeapon.currentWeapon.currentTotalAmmo;
        }
        else{
            ammoText.text="";
            maxAmmoText.text="";
            slash.text="";
            totalAmmoText.text="";
        }
    }


    public void ChangeIcon(){
        weaponIcon.sprite=playerWeapon.currentWeapon.weaponProperty.icon;
        changing=false;
    }
}
