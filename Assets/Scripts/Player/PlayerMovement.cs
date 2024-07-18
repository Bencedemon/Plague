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
    

    [Header("Movement")]
    public bool canMove=false;
    public float moveSpeed = 10f;
    public float speedMultiplier = 1f;
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

    [Header("Stats")]
    public PlayerStats playerStats;

    [Header("Light")]
    [SerializeField] private GameObject lightObject;

    //private GameData gameData;

    public override void OnStartClient(){
        base.OnStartClient();

        Players.Add(OwnerId,this);
        FindObjectOfType<PlayerManager>().PlayerGameObject.Add(this.gameObject);

        if(!base.IsOwner){
            this.enabled = false;
        }else{
            playerInput.enabled = true;
            playerCamera.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public override void OnStopClient(){
        base.OnStopClient();

        Players.Remove(OwnerId);
        FindObjectOfType<PlayerManager>().PlayerGameObject.Remove(this.gameObject);
    }

    void Start(){
        //gameData = FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if(playerStats._currentHealth.Value<=0) return;
        if(!canMove) return;

        if(x==0 && z==0){
            speedMultiplier=0f;
        }else if(isSprinting && z>0){
            speedMultiplier=2f;
        }else if(isWalking){
            speedMultiplier=0.5f;
        }else{
            speedMultiplier=1f;
        }

        if(velocity.y < 0 && controller.isGrounded){
            velocity.y = -2f;
        }

        move = transform.right * x + transform.forward * z;


        controller.Move(move * moveSpeed * speedMultiplier * Time.fixedDeltaTime);

        velocity.y += gravity * Time.fixedDeltaTime;

        controller.Move(velocity * Time.fixedDeltaTime);

        if(transform.position.y<-50){
            transform.position=new Vector3(-70,1.1f,180);
        }


        Vector3 localVelocity = transform.InverseTransformDirection(controller.velocity);
        animator.SetFloat("VelocityX",x);
        animator.SetFloat("VelocityZ",z);
        animator.SetFloat("speed",speedMultiplier);
        animator.SetBool("Grounded",controller.isGrounded);

    }
    public void Move(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        x = context.ReadValue<Vector2>().x;
        z = context.ReadValue<Vector2>().y;
    }
    public void Escape(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            playerInput.SwitchCurrentActionMap("InMenu");
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void Sprinting(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed && controller.isGrounded){
            isSprinting=true;
        }
        if(context.canceled){
            isSprinting=false;
        }
    }
    public void Walking(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed && controller.isGrounded){
            isWalking=true;
        }
        if(context.canceled){
            isWalking=false;
        }
    }
    public void Jump(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed && controller.isGrounded){
            if(!canMove) return;
            velocity.y+=jumpPower;
            animator.SetTrigger("Jump");
        }
    }
    public void Light(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            LightServer(!lightObject.activeSelf);
        }
    }
    [ServerRpc]
    private void LightServer(bool _active){
        LightObserver(_active);
    }

    [ObserversRpc]
    private void LightObserver(bool _active){
        lightObject.SetActive(_active);
    }

    [ServerRpc]
    private void StopTime(){
        if(Time.timeScale == 1f){
            StopTimeObserver(0f);
        }else{
            StopTimeObserver(1f);
        }
    }

    [ObserversRpc]
    private void StopTimeObserver(float _timeScale){
        Time.timeScale = _timeScale;
    }



    public bool isMoving(){
        if(x!=0 || z!=0 && controller.isGrounded) return true;
        else return false;
    }
}
