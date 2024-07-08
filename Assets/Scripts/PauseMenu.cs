using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    
    public GameObject options;


    public PlayerInput playerInput;

    public void Resume(){
        playerInput.SwitchCurrentActionMap("InGame");
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Option(){
        pauseMenu.SetActive(false);
        options.SetActive(true);
    }
    public void Quit(){
        
    }
}
