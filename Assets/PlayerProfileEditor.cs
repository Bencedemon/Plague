using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerProfileEditor : MonoBehaviour
{
    [Header("Profile Picture")]
    [SerializeField] private Image profilePicture;
    
    [Space]
    [SerializeField] private GameObject ProfilePicturesPanel;
    [SerializeField] private Transform parent;
    [SerializeField] private ProfilePictureSelect profilePicturePrefab;

    [Space]
    [Header("Name")]
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private GameObject playerNameObject;
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private GameObject playerNameInputObject;

    [Space]
    [Header("Level")]
    [SerializeField] private TMP_Text playerLevel;

    private PlayerProfileManager playerProfileManager;

    void Awake(){
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();

        profilePicture.sprite = playerProfileManager.sprites[playerProfileManager.playerProfile.pictureId];

        for (int i = 0; i < playerProfileManager.sprites.Length; i++)
        {
            ProfilePictureSelect profilePictureSelect = Instantiate(profilePicturePrefab,parent);
            profilePictureSelect.Initialize(this,i,playerProfileManager.sprites[i]);
        }

        playerName.text = playerProfileManager.playerProfile.playerName + "";

        playerLevel.text = "Level " + playerProfileManager.playerProfile.playerLevel;
    }

    public void ChangeTexture(){
        ProfilePicturesPanel.SetActive(true);
    }
    public void TextureSelected(int _id){
        playerProfileManager.playerProfile.pictureId = _id;
        profilePicture.sprite = playerProfileManager.sprites[_id];
        ProfilePicturesPanel.SetActive(false);
        
        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }
    public void ChangeName(){
        playerNameInput.text = playerProfileManager.playerProfile.playerName;
        playerNameObject.SetActive(false);
        playerNameInputObject.SetActive(true);
    }
    public void SaveName(string _name){
        playerProfileManager.playerProfile.playerName = _name;
        playerName.text = _name + "";

        playerNameInputObject.SetActive(false);
        playerNameObject.SetActive(true);

        SaveSystem.SaveData(playerProfileManager.playerProfile);
    }
}
