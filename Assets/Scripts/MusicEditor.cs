using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicEditor : MonoBehaviour
{
    [SerializeField] private Image picture;

    [Space]
    [SerializeField] private GameObject musicsPanel;
    [SerializeField] private Transform parent;
    [SerializeField] private MusicSelect musicPrefab;

    [Space]
    [SerializeField] private MusicManagerMenu musicManagerMenu;

    private PlayerProfileManager playerProfileManager;

    void Awake(){
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();

        picture.sprite = playerProfileManager.musicPlaylists[playerProfileManager.playerProfile.playlistId].icon;

        for (int i = 0; i < playerProfileManager.musicPlaylists.Length; i++)
        {
            MusicSelect musicSelect = Instantiate(musicPrefab,parent);
            musicSelect.Initialize(this,playerProfileManager.musicPlaylists[i]);
        }
    }
    public void ChangeMusic(){
        musicsPanel.SetActive(true);
    }
    public void MusicSelected(int _id){
        playerProfileManager.playerProfile.playlistId = _id;
        picture.sprite = playerProfileManager.musicPlaylists[_id].icon;

        musicManagerMenu.ChangeMusic(_id);

        musicsPanel.SetActive(false);
        
        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }
    public void ClosePanel(){
        musicsPanel.SetActive(false);
    }

}
