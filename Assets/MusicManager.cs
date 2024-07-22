using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Animator musicAnimator;
    [SerializeField] private AudioSource fight,calm;
    private void stopMusicCalm(){
        calm.Stop();
    }
    private void stopMusicFight(){
        fight.Stop();
    }
    private void startMusicCalm(){
        calm.Play();
    }
    private void startMusicFight(){
        fight.Play();
    }
}
