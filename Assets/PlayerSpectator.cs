using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpectator : MonoBehaviour
{
    [SerializeField] public GameObject spectator;
    
    private int playerId=0;
    private PlayerManager playerManager;

    void Awake(){
        playerManager=FindObjectOfType<PlayerManager>();
    }
    
    public void Next(InputAction.CallbackContext context){
        if(context.performed){
            if(playerId+1<playerManager.PlayerGameObject.Count)
                SpectatePlayer(playerId+1);
            else
                SpectatePlayer(0);
        }
    }
    public void Prev(InputAction.CallbackContext context){
        if(context.performed){
            if(playerId-1>=0)
                SpectatePlayer(playerId-1);
            else
                SpectatePlayer(playerManager.PlayerGameObject.Count-1);
        }
    }

    private void SpectatePlayer(int _id){
        playerId=_id;
        Debug.Log(""+playerId);
        foreach (var player in playerManager.PlayerGameObject)
        {
            player.GetComponent<PlayerSpectator>().spectator.SetActive(false);
        }
        playerManager.PlayerGameObject[playerId].GetComponent<PlayerSpectator>().spectator.SetActive(true);
    }
}
