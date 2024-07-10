using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FishNet.Connection;
using FishNet.Object;

public class Client : NetworkBehaviour
{
    [SerializeField] private GameObject characterSelectionPrefab;
    public int characterId=-1;
    private PlayerManager playerManager;

    private GameObject cs;
    void Awake(){
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public override void OnStartClient(){
        base.OnStartClient();

        characterId=OwnerId;

        playerManager.Clients.Add(this);

        if(base.IsOwner){
            Spawn(LocalConnection);
        }
    }
    

    public override void OnStopClient(){
        base.OnStopClient();

        playerManager.Clients.Remove(this);
        ServerManager.Despawn(cs);
    }
    public void backToLobby(){
        if(base.IsOwner){
            Cursor.lockState = CursorLockMode.None;
            Spawn(LocalConnection);
        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void Spawn(NetworkConnection conn){
        cs = Instantiate(characterSelectionPrefab,transform.position,transform.rotation);
        Spawn(cs,conn);
    }

}
