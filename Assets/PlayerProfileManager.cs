using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    public PlayerProfile playerProfile;

    public Sprite[] sprites;
    void Awake()
    {
        if(SaveSystem.LoadData()!=null){
            playerProfile = SaveSystem.LoadData();
            playerProfile.pictureId=Random.Range(0,2);
            playerProfile.playerName="Player "+Random.Range(1000,9999);
            playerProfile.playerLevel=Random.Range(1,100);
        }
        else{
            playerProfile.pictureId=1;
            playerProfile.playerName="Player "+Random.Range(1000,9999);
            playerProfile.playerLevel=1;
            SaveSystem.SaveData(playerProfile);
        }
    }

}
