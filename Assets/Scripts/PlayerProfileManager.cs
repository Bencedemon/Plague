using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    public bool randomData = false;

    public PlayerProfile playerProfile;

    public Sprite[] sprites;
    public Sprite[] crosshairs;
    void Awake()
    {
        if(SaveSystem.LoadData()==null){
            playerProfile.pictureId=1;
            playerProfile.playerName="Player "+Random.Range(1000,9999);
            playerProfile.playerLevel=1;

            playerProfile.primaryId=1;
            playerProfile.secondaryId=1;
            playerProfile.meeleId=0;

            playerProfile.crosshairId=1;

            playerProfile.crosshairColor = new float[3];
            for (int i = 0; i < 4; i++)
            {
                playerProfile.crosshairColor[i]=1;
            }

            SaveSystem.SaveData(playerProfile);
        }
        else if(randomData){
            playerProfile = SaveSystem.LoadData();
            playerProfile.pictureId=Random.Range(0,sprites.Length);
            playerProfile.playerName="Player "+Random.Range(1000,9999);
            playerProfile.playerLevel=Random.Range(1,100);

            playerProfile.primaryId=Random.Range(0,3);
            playerProfile.secondaryId=Random.Range(0,3);

            playerProfile.crosshairId=Random.Range(0,crosshairs.Length);
        }else{
            playerProfile = SaveSystem.LoadData();
        }
    }

}
