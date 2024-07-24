using UnityEngine;

[CreateAssetMenu(fileName = "MusicPlaylist", menuName = "ScriptableObjects/MusicPlaylist", order = 3)]
public class MusicPlaylist : ScriptableObject
{
    public int playlistId;
    public string playlistName;
    public Sprite icon;
    public AudioClip menuMusic;
    public AudioClip[] fightMusic,calmMusic;
}
