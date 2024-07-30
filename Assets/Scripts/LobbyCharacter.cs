using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyCharacter : MonoBehaviour
{
    public CharacterSelection characterSelection;

    public Image icon;
    public TMP_Text playerNameField;
    public TMP_Text level;
    public Image ready;

    public Animator animator;

    private PlayerProfileManager playerProfileManager;

    void Awake(){
        playerProfileManager=FindObjectOfType<PlayerProfileManager>();
        animator.SetInteger("pose",Random.Range(0, 3));
    }
    void FixedUpdate(){
        icon.sprite = playerProfileManager.sprites[characterSelection.pp.Value];
        playerNameField.text = characterSelection.playerName.Value;
        level.text = "Level "+characterSelection.level.Value;
        if(characterSelection.ready.Value) ready.color= new Color32(0,255,0,255); else ready.color= new Color32(255,0,0,255);
    }

}
