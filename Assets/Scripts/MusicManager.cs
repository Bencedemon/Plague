using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource fight,calm;
    
    private PlayerProfileManager playerProfileManager;

    void Awake(){
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fight.clip = playerProfileManager.musicPlaylists[playerProfileManager.playerProfile.playlistId].fightMusic[0];
        calm.clip = playerProfileManager.musicPlaylists[playerProfileManager.playerProfile.playlistId].calmMusic[0];
    }
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
