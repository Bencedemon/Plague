using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairEditor : MonoBehaviour
{
    [Space]
    [SerializeField] private Image crosshairTexture;
    
    [Space]
    [SerializeField] private GameObject CrosshairsPanel;
    [SerializeField] private Transform parent;
    [SerializeField] private CrosshairSelect crosshairPrefab;


    private PlayerProfileManager playerProfileManager;

    void Awake(){
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();

        crosshairTexture.sprite = playerProfileManager.crosshairs[playerProfileManager.playerProfile.crosshairId];

        for (int i = 0; i < playerProfileManager.crosshairs.Length; i++)
        {
            CrosshairSelect crosshairSelect = Instantiate(crosshairPrefab,parent);
            crosshairSelect.Initialize(this,i,playerProfileManager.crosshairs[i]);
        }
    }

    public void ChangeTexture(){
        CrosshairsPanel.SetActive(true);
    }
    public void TextureSelected(int _id){
        playerProfileManager.playerProfile.crosshairId = _id;
        crosshairTexture.sprite = playerProfileManager.crosshairs[_id];
        CrosshairsPanel.SetActive(false);
        
        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }

    public void ChangeColor(Color _color){
        crosshairTexture.color = _color;
        
        if(playerProfileManager==null) return;

        playerProfileManager.playerProfile.crosshairColor[0] = _color.r;
        playerProfileManager.playerProfile.crosshairColor[1] = _color.g;
        playerProfileManager.playerProfile.crosshairColor[2] = _color.b;
        playerProfileManager.playerProfile.crosshairColor[3] = _color.a;

        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }
}
