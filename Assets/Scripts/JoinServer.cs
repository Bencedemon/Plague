using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FishNet.Managing;
using FishNet.Transporting.Tugboat;

public class JoinServer : MonoBehaviour
{
    public GameObject menu;
    public GameObject joinPanel;
    

    public TMP_Text connectionNotification;
    public Tugboat tugboat;
    public TMP_InputField inputField;

    public NetworkManager _networkManager;
    public bool canJoin = false;
	

    public void Join(){
        tugboat.SetClientAddress(inputField.text);
        
        _networkManager.ClientManager.StartConnection();
        
        StopCoroutine(TryConnection());
        StartCoroutine(TryConnection());

    }

    private IEnumerator TryConnection(){
        CharacterSelection[] characterSelections;
        int tries=0;
        while(tries<=30){
            connectionNotification.text = "Tries: "+tries;
            yield return new WaitForSeconds(1f);
            tries++;
            characterSelections=FindObjectsOfType<CharacterSelection>();

            if(characterSelections.Length>0){
                openLobby();
                break;
            }
        }
        connectionNotification.text = "Connection not found";
    }

    private void openLobby(){
        joinPanel.SetActive(false);
    }

    public void Back(){
        StopCoroutine(TryConnection());
        joinPanel.SetActive(false);
        menu.SetActive(true);
    }
}
