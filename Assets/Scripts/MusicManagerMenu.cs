using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerMenu : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    private PlayerProfileManager playerProfileManager;

    void Awake(){
        playerProfileManager = FindObjectOfType<PlayerProfileManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = playerProfileManager.musicPlaylists[playerProfileManager.playerProfile.playlistId].menuMusic;
        audioSource.Play();
    }

    public void ChangeMusic(int _id){
        audioSource.clip = playerProfileManager.musicPlaylists[_id].menuMusic;
        audioSource.Play();
    }

}
