using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using TMPro;
using FishNet.Managing;
using FishNet.Transporting;

public class HostServer : MonoBehaviour
{
    public GameObject menu;
    public GameObject hostPanel;
    
    public FishNet.Transporting.Tugboat.Tugboat tugboat;

    public NetworkManager _networkManager;
    public void LocalHost(){
        tugboat.SetClientAddress("localhost");
        _networkManager.ServerManager.StartConnection();
        _networkManager.ClientManager.StartConnection();
        openLobby();
    }

    private void openLobby(){
        hostPanel.SetActive(false);
    }

    public void Back(){
        hostPanel.SetActive(false);
        menu.SetActive(true);
    }
}
