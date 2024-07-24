using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerProfile
{
    public int pictureId;
    public string playerName;
    public int playerLevel;

    public int primaryId,secondaryId,meeleId;

    public int crosshairId;
    public float[] crosshairColor = new float[4];

    public int playlistId;

    public PlayerProfile(int _pictureId,string _playerName,int _playerLevel,int _primaryId,int _secondaryId,int _meeleId,int _crosshairId,float[] _crosshairColor,int _playlistId){
        pictureId = _pictureId;
        playerName = _playerName;
        playerLevel = _playerLevel;
        
        primaryId = _primaryId;
        secondaryId = _secondaryId;
        meeleId = _meeleId;

        crosshairId = _crosshairId;
        crosshairColor = _crosshairColor;

        playlistId = _playlistId;
    }
}
