using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutEditor : MonoBehaviour
{
    private PlayerProfileManager playerProfileManager;

    [Space]
    [SerializeField] private WeaponProperty[] primaries;
    [SerializeField] private Image primaryIcon;
    [SerializeField] private GameObject primariesPanel;

    [Space]
    [SerializeField] private WeaponProperty[] secondaries;
    [SerializeField] private Image secondaryIcon;
    [SerializeField] private GameObject secondariesPanel;

    [Space]
    [SerializeField] private WeaponProperty[] meeles;
    [SerializeField] private Image meeleIcon;
    [SerializeField] private GameObject meelesPanel;



    // Start is called before the first frame update
    void Awake()
    {
        playerProfileManager=FindObjectOfType<PlayerProfileManager>();
        
        primaryIcon.sprite = primaries[playerProfileManager.playerProfile.primaryId].iconSmall;
        secondaryIcon.sprite = secondaries[playerProfileManager.playerProfile.secondaryId].iconSmall;
        meeleIcon.sprite = meeles[playerProfileManager.playerProfile.meeleId].iconSmall;
    }


    public void showPrimariesPanel(){
        secondariesPanel.SetActive(false);
        meelesPanel.SetActive(false);

        primariesPanel.SetActive(!primariesPanel.activeSelf);
    }
    public void showSecondariesPanel(){
        primariesPanel.SetActive(false);
        meelesPanel.SetActive(false);
        
        secondariesPanel.SetActive(!secondariesPanel.activeSelf);
    }
    public void showMeelesPanel(){
        primariesPanel.SetActive(false);
        secondariesPanel.SetActive(false);
        
        meelesPanel.SetActive(!meelesPanel.activeSelf);
    }

    public void selectPrimarie(int id){
        playerProfileManager.playerProfile.primaryId=id;
        primaryIcon.sprite = primaries[id].iconSmall;
        primariesPanel.SetActive(false);

        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }
    public void selectSecondary(int id){
        playerProfileManager.playerProfile.secondaryId=id;
        secondaryIcon.sprite = secondaries[id].iconSmall;
        secondariesPanel.SetActive(false);
        
        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }
    public void selectMeele(int id){
        playerProfileManager.playerProfile.meeleId=id;
        meeleIcon.sprite = meeles[id].iconSmall;
        meelesPanel.SetActive(false);
        
        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }
}
