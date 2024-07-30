using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerProjectile : NetworkBehaviour
{
    
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Trap trap;
    public override void OnStartClient(){
        base.OnStartClient();
        if(IsServerInitialized){
            trap.collider.enabled = true;
            rb.AddForce(transform.forward*10f, ForceMode.Impulse);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        //rb.detectCollisions = false;
    }
}
