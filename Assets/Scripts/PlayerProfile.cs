using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerProfile
{
    public int pictureId;
    public string playerName;
    public int playerLevel;

    public PlayerProfile(int _pictureId,string _playerName,int _playerLevel){
        pictureId = _pictureId;
        playerName = _playerName;
        playerLevel = _playerLevel;
    }
}
