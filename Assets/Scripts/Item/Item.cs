using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public abstract class Item : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(base.IsServerInitialized)
        if(other.tag=="Player"){
            PickUp(other);
        }
    }
    [Server]
    public void DespawnObject(GameObject _gameObject){
        ServerManager.Despawn(_gameObject);
    }

    public abstract void PickUp(Collider player);
}
