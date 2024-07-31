using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Client> Clients = new List<Client>();
    public List<CharacterSelection> Players = new List<CharacterSelection>();
    public List<GameObject> PlayerGameObject = new List<GameObject>();

    public bool gameStarted = false;

    [Space]
    [SerializeField] private DiscordManager discordManager;

}
