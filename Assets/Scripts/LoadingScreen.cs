using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Connection;
using FishNet.Broadcast;
using FishNet.Transporting;

public class LoadingScreen : MonoBehaviour
{
    
    [SerializeField] private GameObject loadingScreenPanel;
    
    public struct LoadingScreenBroadcast : IBroadcast
    {
        public bool PanelState;
    }
    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<LoadingScreenBroadcast>(OnLoadinScreenBroadcast);
    }
    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<LoadingScreenBroadcast>(OnLoadinScreenBroadcast);
    }

    public void OnLoadinScreenActivate()
    {
        LoadingScreenBroadcast lsb = new LoadingScreenBroadcast()
        {
            PanelState = true
        };
        
        InstanceFinder.ServerManager.Broadcast(lsb);
    }
    private void OnLoadinScreenBroadcast(LoadingScreenBroadcast lsb, Channel channel)
    {
        loadingScreenPanel.SetActive(lsb.PanelState);
    }
}
