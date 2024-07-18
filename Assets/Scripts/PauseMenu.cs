using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Managing;
using FishNet.Transporting;


public class PauseMenu : NetworkBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    [SerializeField] private GameObject options;


    [SerializeField] private PlayerInput playerInput;


    [SerializeField] private GameObject backToLobby, backToMain;
    [Space]
    [SerializeField] private FishNet.Example.Scened.SceneLoader sceneLoader;
    [Space]
    [SerializeField] private GameObject upgradePanel;

    private NetworkManager _networkManager;


    void Awake(){
        _networkManager = FindObjectOfType<NetworkManager>();
    }

    public override void OnStartClient(){
        base.OnStartClient();

        if(!base.IsOwner) return;
        if(base.IsServer){
            backToLobby.SetActive(true);
            backToMain.SetActive(true);
        }else{
            backToLobby.SetActive(false);
            backToMain.SetActive(true);
        }
    }

    public void Resume(){
        playerInput.SwitchCurrentActionMap("InGame");
        pauseMenu.SetActive(false);
        if(!upgradePanel.activeSelf)
            Cursor.lockState = CursorLockMode.Locked;
    }
    public void Option(){
        pauseMenu.SetActive(false);
        options.SetActive(true);
    }
    public void Quit(){
        
    }
    public void BackToLobby(){
        sceneLoader.StartLoading("00_MainMenu");
    }
    public void BackToMain(){
        sceneLoader.StartLoading("00_MainMenu");
        
        if(base.IsServer)
            _networkManager.ServerManager.StopConnection(true);
            
        _networkManager.ClientManager.StopConnection();
    }
}
