using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Connection;
using FishNet.Object;

public class PlayerMovement : NetworkBehaviour
{
    public static Dictionary<int, PlayerMovement> Players = new Dictionary<int, PlayerMovement>();


    public CharacterController controller;
    public PlayerInput playerInput;
    
    [Header("Layers")]
    public int playerSelfLayer = 6;
    public int playerOtherLayer = 7;
    public int weaponSelfLayer = 8;
    public int weaponOtherLayer = 9;
    public GameObject[] body, hand;

    [Header("Movement")]
    public bool canMove=false;
    public float moveSpeed = 10f;
    public float speedMultiplier = 1.5f;
    public float jumpPower;
    private bool isSprinting=false,isWalking=false;


    public float gravity = -9.81f;

    Vector3 velocity;

    public float x;
    public float z;

    [Header("Pause")]
    public GameObject pauseMenu;

    [Header("Camera")]
    public GameObject playerCamera;

    [Header("Animator")]
    public Animator animator;

    Vector3 move;

    //private GameData gameData;

    public override void OnStartClient(){
        base.OnStartClient();

        Players.Add(OwnerId,this);

        if(!base.IsOwner){
            playerInput.enabled = false;
            this.enabled = false;

            foreach (GameObject item in hand)
            {
                item.layer = weaponOtherLayer;
            }
            foreach (GameObject item in body)
            {
                item.layer = playerOtherLayer;
            }
        }else{
            playerCamera.SetActive(true);
        }
    }

    public override void OnStopClient(){
        base.OnStopClient();

        Players.Remove(OwnerId);
    }

    void Start(){
        //gameData = FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if(!canMove) return;

        if(isSprinting){
            speedMultiplier=3f;
        }else if(isWalking){
            speedMultiplier=1f;
        }else{
            speedMultiplier=1.5f;
        }

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


        Vector3 localVelocity = transform.InverseTransformDirection(controller.velocity);
        animator.SetFloat("VelocityX",x);
        animator.SetFloat("VelocityZ",z);
        animator.SetFloat("speed",speedMultiplier);
        animator.SetBool("Grounded",controller.isGrounded);

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
    public void Walking(InputAction.CallbackContext context){
        if(context.performed){
            isWalking=true;
        }
        if(context.canceled){
            isWalking=false;
        }
    }
    public void Jump(InputAction.CallbackContext context){
        if(context.performed && controller.isGrounded){
            if(!canMove) return;
            velocity.y+=jumpPower;
            animator.SetTrigger("Jump");
        }
    }
}
