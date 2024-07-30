using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject titleScreenCamera;
    
    [Header("Multiplayer")]
    public GameObject multiplayerScreen;
    public GameObject multiplayerCamera;

    [Header("Profile")]
    public GameObject profileScreen;
    public GameObject profileScreenCamera;

    [Header("Battlepass")]
    public GameObject battlePass;

    [Header("Options")]
    public GameObject options;
    public GameObject optionsCamera;


    [Header("ButtonManager")]
    public Button[] buttons;

    [Header("Version")]
    public TMP_Text version;

    [Header("ConnectionLost")]
    public GameObject connectionLostPanel;
    
    void Awake(){
        titleScreenCamera.SetActive(true);
        version.text="v "+Application.version;
    }
    public void PlayGame(){
        connectionLostPanel.SetActive(false);
        titleScreen.SetActive(false);
        multiplayerScreen.SetActive(true);
        multiplayerCamera.SetActive(true);
    }
    public void Profile(){
        titleScreen.SetActive(false);
        profileScreen.SetActive(true);
        profileScreenCamera.SetActive(true);
    }
    public void BattlePass(){
        //audioSource.Play();
        titleScreen.SetActive(false);
        battlePass.SetActive(true);
    }
    public void Option(){
        //audioSource.Play();
        titleScreen.SetActive(false);
        options.SetActive(true);
        optionsCamera.SetActive(true);
    }
    public void Quit(){
        //audioSource.Play();
        Application.Quit();
    }

    public void hoverButton(int _id){
        buttons[_id].Select();
    }
}
