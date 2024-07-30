using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Connection;
using FishNet.Object;

public class MouseLook : NetworkBehaviour
{
    //private GameData gameData;

    public Transform player;

    float xRotation = 0f;
    public Transform cam;

    private float x,y;
    public float mouseX;
    public float mouseY;

    //public PlayerMovement playerMovement;


    [Header("Stats")]
    public PlayerStats playerStats;

    private PlayerDataManager playerDataManager;

    // Start is called before the first frame update
    void Start()
    {
        playerDataManager=FindObjectOfType<PlayerDataManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerStats._currentHealth.Value<=0) return;
        //camera.fieldOfView = playerDataManager.playerData.fov;
                
        //mouseX = Input.GetAxis("Mouse X") * playerDataManager.playerData.mouseSensitivity * Time.fixedDeltaTime;
        //mouseY = Input.GetAxis("Mouse Y") * playerDataManager.playerData.mouseSensitivity * Time.fixedDeltaTime;
        mouseX = x * Time.fixedDeltaTime * playerDataManager.playerData.mouseSensitivity;
        mouseY = y * Time.fixedDeltaTime * playerDataManager.playerData.mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
    public void Look(InputAction.CallbackContext context){
        if(playerStats._currentHealth.Value<=0) return;
        x = context.ReadValue<Vector2>().x;
        y = context.ReadValue<Vector2>().y;
    }
}
