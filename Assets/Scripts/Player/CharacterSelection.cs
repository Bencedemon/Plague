using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;

public class CharacterSelection : NetworkBehaviour
{
    public int characterId=-1;
    [SerializeField] private GameObject characterSelectorPanel;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject startButton;

    public readonly SyncVar<int> pp = new();
    public readonly SyncVar<string> name = new();
    public readonly SyncVar<int> level = new();
    public readonly SyncVar<bool> ready = new(false);


    [SerializeField] private GameObject fadePanel;
    [SerializeField] private Animator fade;

    [SerializeField] private GameObject lobbyCharacters;
    [SerializeField] private Vector3[] points;
    private GameObject lobbyPlayer;

    //=====================
    [SerializeField] private FishNet.Example.Scened.SceneLoader sceneLoader;
    //=====================
    
    private PlayerProfileManager playerProfileManager;
    private PlayerManager playerManager;
    void Awake(){
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public override void OnStartClient(){
        base.OnStartClient();

        characterId=OwnerId;

        playerManager.Players.Add(this);
        LobbySpawn(OwnerId);

        if(base.IsOwner){
            canvasObject.SetActive(true);
            mainCamera.SetActive(true);
            fadePanel.SetActive(true);
            SetPP(playerProfileManager.playerProfile.pictureId);
            SetName(playerProfileManager.playerProfile.playerName);
            SetLevel(playerProfileManager.playerProfile.playerLevel);

            if(base.IsServerInitialized){
                startButton.SetActive(true);
            }
        }
    }
    

    public override void OnStopClient(){
        base.OnStopClient();

        playerManager.Players.Remove(this);
        ServerManager.Despawn(lobbyPlayer);
    }

    void FixedUpdate(){
        if(!base.IsOwner) return;
        if(!base.IsServerInitialized) return;

        bool canStart=false;
        for (int i = 0; i < playerManager.Players.Count; i++)
        {
            if(!playerManager.Players[i].ready.Value){
                startButton.GetComponent<Button>().interactable=false;
                return;
            }
        }

        startButton.GetComponent<Button>().interactable=true;
    }

    public void StartGame(){
        startButton.SetActive(false);
        sceneLoader.StartLoading("01_Game");
    }

    public void Ready(){
        SetReady(!ready.Value);
    }

    void LobbySpawn(int id){
        for (int i = 0; i < playerManager.Players.Count; i++)
        {
            if(playerManager.Players[i]==this){
                lobbyCharacters.transform.position=points[i];
            }
        }
    }

    
    [ServerRpc] private void SetPP(int _pp) => pp.Value = _pp;
    [ServerRpc] private void SetName(string _name) => name.Value = _name;
    [ServerRpc] private void SetLevel(int _level) => level.Value = _level;
    [ServerRpc] private void SetReady(bool _ready) => ready.Value = _ready;

}
