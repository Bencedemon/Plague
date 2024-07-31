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
            _itemReference = Instantiate(itemPrefab, transform.position+new Vector3(0,1,0), transform.rotation);
            ServerManager.Spawn(_itemReference);
            cooldown=Random.Range(15f,60f);
        }else{
            cooldown -= Time.fixedDeltaTime;
        }
    }
}
