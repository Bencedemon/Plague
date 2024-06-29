using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public GameObject[] menuItems;

    public GameObject back;
    public GameObject backCamera;

    public GameObject menuCamera;

    [Header("ButtonManager")]
    public Button[] buttons;
    private int selectedButtonId=0;
    private bool selected;

    public void OpenItem(int id){
        selectedButtonId=0;
        selected=false;
        gameObject.SetActive(false);
        menuItems[id].SetActive(true);
    }

    public void Back(){
        selectedButtonId=0;
        selected=false;
        gameObject.SetActive(false);
        menuCamera.SetActive(false);
        back.SetActive(true);
        backCamera.SetActive(true);
    }

    public void backInput(InputAction.CallbackContext context){
        if(context.performed){
            buttons[buttons.Length-1].Select();
            Back();
        }
    }
    public void hoverButton(int _id){
        selected=true;
        selectedButtonId=_id;
        buttons[selectedButtonId].Select();
    }
}
