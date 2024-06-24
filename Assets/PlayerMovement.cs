using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Connection;
using FishNet.Object;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;
    public PlayerInput playerInput;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float speedMultiplier = 1f;
    private bool isSprinting=false;


    public float gravity = -9.81f;

    Vector3 velocity;

    public float x;
    public float z;

    [Header("Pause")]
    public GameObject pauseMenu;

    [Header("Camera")]
    public GameObject playerCamera;

    Vector3 move;

    //private GameData gameData;

    public override void OnStartClient(){
        base.OnStartClient();
        if(!base.IsOwner){
            playerInput.enabled = false;
            playerCamera.SetActive(false);
            this.enabled = false;
        }
    }
    void Start(){
        //gameData = FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(velocity.y < 0 && controller.isGrounded){
            velocity.y = -2f;
        }

        move = transform.right * x + transform.forward * z;


        controller.Move(move * moveSpeed * speedMultiplier * Time.fixedDeltaTime);

        velocity.y += gravity * Time.fixedDeltaTime;

        controller.Move(velocity * Time.fixedDeltaTime);

        if(transform.position.y<-50){
            transform.position=new Vector3(19.5f,2.5f,50);
        }

    }
    public void Move(InputAction.CallbackContext context){
        x = context.ReadValue<Vector2>().x;
        z = context.ReadValue<Vector2>().y;
    }
    public void Escape(InputAction.CallbackContext context){
        if(context.performed){
        }
    }
    public void Sprinting(InputAction.CallbackContext context){
        if(context.performed){
            isSprinting=true;
        }
        if(context.canceled){
            isSprinting=false;
        }
    }
    public void ShootL(InputAction.CallbackContext context){
        if(context.performed){
            
        }
    }

    public void ShootR(InputAction.CallbackContext context){
        if(context.performed){

        }
        if(context.canceled){
            
        }
    }
}
