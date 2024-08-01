using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class EnemySounds : NetworkBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceFootstep;
    [SerializeField] private AudioClip[] attack,idle,death,foot; // 0 - Attack, 1 - Idle, 2 - Death, 3 - foot


    private bool stopSounds=false;
    private float idleSoundCooldown=0f;

    void FixedUpdate(){
        if(!IsServerInitialized) return;
        if(stopSounds) return;
        if(enemy.health<=0){
            stopSounds=true;
            PlaySoundServer(2,Random.Range(0,death.Length),Random.Range(0.8f,1.2f));
        }else
        if(idleSoundCooldown<=0){
            idleSoundCooldown=Random.Range(5f,10f);
            PlaySoundServer(1,Random.Range(0,idle.Length),Random.Range(0.8f,1.2f));
        }else{
            idleSoundCooldown-=Time.fixedDeltaTime;
        }
    }

    public void PlayStepSound(){
        audioSourceFootstep.clip = foot[Random.Range(0,foot.Length)];
        audioSourceFootstep.pitch = Random.Range(0.8f,1.2f);
        audioSourceFootstep.Play();
    }

    public void PlayAttackSound(){
        audioSource.clip = attack[Random.Range(0,attack.Length)];
        audioSource.pitch = Random.Range(0.8f,1.2f);
        audioSource.Play();
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void PlaySoundServer(int _type, int _id, float _pitch){
        PlaySoundObserver(_type,_id,_pitch);
    }
    [ObserversRpc]
    private void PlaySoundObserver(int _type, int _id, float _pitch){
        switch (_type)
        {
            case 0:
                audioSource.clip = attack[_id];
            break;
            case 1:
                audioSource.clip = idle[_id];
            break;
            case 2:
                audioSource.clip = death[_id];
            break;
            default:
                audioSource.clip = idle[_id];
            break;
        }
        audioSource.pitch = _pitch;
        audioSource.Play();
        Debug.Log("Sound played");
    }
}
