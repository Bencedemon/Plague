using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class ItemHolder : NetworkBehaviour
{
    [SerializeField] private NetworkObject itemPrefab;

    public NetworkObject _itemReference;
    public float cooldown=0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!IsServerInitialized) return;
        if(_itemReference!=null) return;
        if(cooldown<=0){
            _itemReference = Instantiate(itemPrefab, transform.position, transform.rotation);
            ServerManager.Spawn(_itemReference);
            cooldown=30f;
        }else{
            cooldown -= Time.fixedDeltaTime;
        }
    }
}
