using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public abstract class Item : NetworkBehaviour
{
    public GameObject parent;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip[] audioClips;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
        {
            if(playerStats.IsOwner){}
                PickUp(other);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject _gameObject){
        ServerManager.Despawn(_gameObject);
    }

    public abstract void PickUp(Collider player);
}
