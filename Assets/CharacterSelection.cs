using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using FishNet.Connection;
using FishNet.Object;

public class CharacterSelection : NetworkBehaviour
{
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private GameObject characterSelectorPanel;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject mainCamera;

    public override void OnStartClient(){
        base.OnStartClient();
        if(base.IsOwner){
            canvasObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    public void SpawnPlayer(int id)
    {
        characterSelectorPanel.SetActive(false);
        Spawn(id,LocalConnection);
    }

    [ServerRpc(RequireOwnership=false)]
    void Spawn(int spawnIndex,NetworkConnection conn){
        GameObject player = Instantiate(characters[spawnIndex],transform.position,quaternion.identity);
        Spawn(player,conn);
    }


}
