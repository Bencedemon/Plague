using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<CharacterSelection> Players = new List<CharacterSelection>();
    public List<GameObject> PlayerGameObject = new List<GameObject>();
    public List<GameManager> GameManagers = new List<GameManager>();

    public bool gameStarted = false;


    void FixedUpdate(){
        if(GameManagers.Count==0) return;
        int count=0;
        foreach (var item in PlayerGameObject)
        {
            if(item.GetComponent<PlayerStats>()._currentHealth.Value<=0){
                count++;
            }
        }
        if(count==GameManagers.Count){
            foreach (var manager in GameManagers)
            {
                manager.gameEnd();
            }
        }
    }
}
