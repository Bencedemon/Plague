using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class FootStepManager : NetworkBehaviour
{

    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] steps;

    [Header("PlayerMovement")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float baseCooldown=1f;
    private float coolDown=1f;


    void Start(){
        coolDown=baseCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!base.IsOwner) return;
        if(playerMovement.isMoving()){
            coolDown-=Time.fixedDeltaTime*playerMovement.speedMultiplier;
            if(coolDown<=0){
                PlayFootstepServer(Random.Range(0,steps.Length));
                audioSource.Play();
                coolDown=baseCooldown;
            }
        }
    }

    [ServerRpc]
    private void PlayFootstepServer(int _id){
        PlayFootstepObserver(_id);
    }

    [ObserversRpc]
    private void PlayFootstepObserver(int _id){
        audioSource.clip=steps[_id];
        audioSource.Play();
    }
}
