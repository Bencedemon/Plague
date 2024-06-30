using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerProfilePanel : MonoBehaviour
{
    public PlayerProfileManager playerProfileManager;

    public Image pp;
    public TMP_Text nameText;
    public TMP_Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        pp.sprite = playerProfileManager.sprites[playerProfileManager.playerProfile.pictureId];
        nameText.text = playerProfileManager.playerProfile.playerName;
        levelText.text = "Level "+playerProfileManager.playerProfile.playerLevel;
    }

}
