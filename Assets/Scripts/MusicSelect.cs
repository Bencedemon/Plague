using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicSelect : MonoBehaviour
{
    [SerializeField] private Image _texture;
    private MusicPlaylist musicPlaylist;

    private MusicEditor musicEditor;
    
    public void Initialize(MusicEditor _musicEditor,MusicPlaylist _musicPlaylist){
        musicEditor = _musicEditor;
        musicPlaylist = _musicPlaylist;

        _texture.sprite = _musicPlaylist.icon;
    }

    public void Select(){
        musicEditor.MusicSelected(musicPlaylist.playlistId);
    }
}
