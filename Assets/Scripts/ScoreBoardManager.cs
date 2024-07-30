using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("ScoreBoard")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text maxHealthValue,movementSpeedValue,strengthValue,armorValue,reloadSpeedValue,healPowerValue;

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
            board.SetActive(true);
        }
        if(context.canceled){
            board.SetActive(false);
        }
    }
}
