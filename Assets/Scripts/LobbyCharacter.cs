using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyCharacter : MonoBehaviour
{
    public CharacterSelection characterSelection;

    public Image icon;
    public TMP_Text name;
    public TMP_Text level;
    public Image ready;

    public Sprite[] sprites;

    private PlayerProfileManager playerProfileManager;

    void Awake(){
        playerProfileManager=FindObjectOfType<PlayerProfileManager>();
    }
    void FixedUpdate(){
        icon.sprite = playerProfileManager.sprites[characterSelection.pp.Value];
        name.text = characterSelection.name.Value;
        level.text = "Level "+characterSelection.level.Value;
        if(characterSelection.ready.Value) ready.color= new Color32(0,255,0,255); else ready.color= new Color32(255,0,0,255);
    }
}
