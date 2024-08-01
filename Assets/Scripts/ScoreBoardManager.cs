using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ScoreBoardManager : MonoBehaviour
{
    [Header("ScoreBoard")]
    [SerializeField] private ScorePlayerCard scorePlayerCardPrefab;
    [SerializeField] private Transform scorePlayerCardParent;

    public int playerCount=0;
    public List<ScorePlayerCard> playerCards = new List<ScorePlayerCard>();

    [SerializeField] private GameObject board;

    [Header("Stats")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text maxHealthValue,movementSpeedValue,strengthValue,armorValue,reloadSpeedValue,healPowerValue;

    [Header("Ability")]
    [SerializeField] private PlayerAbility playerAbility;
    [Space]
    [SerializeField] private Sprite[] damageCards; 
    [SerializeField] private GameObject damagePanel;
    [SerializeField] private Image damageImage;
    [SerializeField] private TMP_Text damageText;
    [Space]
    [SerializeField] private Sprite[] cooldownCards; 
    [SerializeField] private GameObject cooldownPanel;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TMP_Text cooldownText;
    [Space]
    [SerializeField] private Sprite[] durationCards; 
    [SerializeField] private GameObject durationPanel;
    [SerializeField] private Image durationImage;
    [SerializeField] private TMP_Text durationText;
    [Space]
    [SerializeField] private Sprite[] rangeCards; 
    [SerializeField] private GameObject rangePanel;
    [SerializeField] private Image rangeImage;
    [SerializeField] private TMP_Text rangeText;


    private PlayerManager playerManager;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerCount!=playerManager.PlayerGameObject.Count){
            for (int i = playerCount; i < playerManager.PlayerGameObject.Count; i++)
            {
                ScorePlayerCard newCard = Instantiate(scorePlayerCardPrefab,scorePlayerCardParent);
                playerCards.Add(newCard);
                newCard.Initialize(playerManager.PlayerGameObject[i].GetComponent<PlayerStats>());
            }
            playerCount=playerManager.PlayerGameObject.Count;
        }

        maxHealthValue.text = ""+playerStats.maxHealth;

        movementSpeedValue.text = playerStats.movementSpeed*100+"%";
        strengthValue.text = playerStats.strength*100+"%";
        float damageReduction = Calculator.CalcDamageReduction(playerStats.armor)*100;
        armorValue.text = playerStats.armor+" ("+damageReduction.ToString("F0")+"%)";
        reloadSpeedValue.text = playerStats.reloadSpeed*100+"%";
        healPowerValue.text = playerStats.healPower*100+"%";
    }
    public void ScoreBoard(InputAction.CallbackContext context){
        if(context.performed){
            UpdateAbility();
            board.SetActive(true);
        }
        if(context.canceled){
            board.SetActive(false);
        }
    }

    private void UpdateAbility(){
        if(playerAbility.ability==null){
            damagePanel.SetActive(false);
            cooldownPanel.SetActive(false);
            durationPanel.SetActive(false);
            rangePanel.SetActive(false);
        }else{
            if(playerAbility.ability.strength==0){
                damagePanel.SetActive(false);
            }else{
                damageImage.sprite = damageCards[playerAbility.ability.strengthLevel];
                damageText.text = "Ability Damage: "+playerAbility.ability.strength;
                damagePanel.SetActive(true);
            }
            if(playerAbility.ability.cooldown==0){
                cooldownPanel.SetActive(false);
            }else{
                cooldownImage.sprite = cooldownCards[playerAbility.ability.cooldownLevel];
                cooldownText.text = "Ability Cooldown: "+playerAbility.ability.cooldown+"s";
                cooldownPanel.SetActive(true);
            }
            if(playerAbility.ability.duration==0){
                durationPanel.SetActive(false);
            }else{
                durationImage.sprite = durationCards[playerAbility.ability.durationLevel];
                durationText.text = "Ability Duration: "+playerAbility.ability.duration+"s";
                durationPanel.SetActive(true);
            }
            if(playerAbility.ability.range==0){
                rangePanel.SetActive(false);
            }else{
                rangeImage.sprite = rangeCards[playerAbility.ability.rangeLevel];
                rangeText.text = "Ability Range: "+playerAbility.ability.range+"m";
                rangePanel.SetActive(true);
            }
        }
    }
}
