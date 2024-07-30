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
	
    [Space]
    public GameObject joinButton,cancelButton;

    public void Join(){
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
        inputField.interactable=false;

        tugboat.SetClientAddress(inputField.text);
        
        _networkManager.ClientManager.StartConnection();
        
        StopCoroutine(TryConnection());
        StartCoroutine(TryConnection());

    }
    public void Cancel(){
        StopCoroutine(TryConnection());
        cancelButton.SetActive(false);
        joinButton.SetActive(true);
        inputField.interactable=true;
        connectionNotification.text = "";

        _networkManager.ClientManager.StopConnection();
    }

    private IEnumerator TryConnection(){
        CharacterSelection[] characterSelections;
        int tries=0;
        bool connected = false;
        connectionNotification.text = "Tries: "+tries;
        while(tries<=30){
            yield return new WaitForSeconds(1f);
            tries++;
            connectionNotification.text = "Tries: "+tries;
            characterSelections=FindObjectsOfType<CharacterSelection>();

            if(characterSelections.Length>0){
                openLobby();
                connected=true;
                break;
            }
        }
        if(!connected){
            cancelButton.SetActive(false);
            joinButton.SetActive(true);
            inputField.interactable=true;
            _networkManager.ClientManager.StopConnection();
            connectionNotification.text = "Connection not found";
        }
    }

    private void openLobby(){
        joinPanel.SetActive(false);
    }

    public void Back(){
        StopCoroutine(TryConnection());
        cancelButton.SetActive(false);
        joinButton.SetActive(true);
        inputField.interactable=true;
        connectionNotification.text = "";

        _networkManager.ClientManager.StopConnection();
        joinPanel.SetActive(false);
        menu.SetActive(true);
    }
}
