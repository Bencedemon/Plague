using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreBoardManager : MonoBehaviour
{
    [SerializeField] private ScorePlayerCard scorePlayerCardPrefab;
    [SerializeField] private Transform scorePlayerCardParent;

    public int playerCount=0;
    public List<ScorePlayerCard> playerCards = new List<ScorePlayerCard>();

    [SerializeField] private GameObject board;

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
    }
    public void ScoreBoard(InputAction.CallbackContext context){
        if(context.performed){
            board.SetActive(!board.activeSelf);
        }
        //if(context.canceled){
            //board.SetActive(false);
        //}
    }
}
