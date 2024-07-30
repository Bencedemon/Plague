using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Connection;
using FishNet.Object;

public class MouseLookSpectator : NetworkBehaviour
{
    //private GameData gameData;

    public Transform player;

    float xRotation = 0f;
    public Transform cam;

    public float mouseX;
    public float mouseY;

    //public PlayerMovement playerMovement;


    //[Header("Stats")]
    //public PlayerStats playerStats;
    //private PlayerDataManager playerDataManager;

    // Start is called before the first frame update
    void Start()
    {
        //playerDataManager=FindObjectOfType<PlayerDataManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //camera.fieldOfView = playerDataManager.playerData.fov;
                
        //mouseX = Input.GetAxis("Mouse X") * playerDataManager.playerData.mouseSensitivity * Time.fixedDeltaTime;
        //mouseY = Input.GetAxis("Mouse Y") * playerDataManager.playerData.mouseSensitivity * Time.fixedDeltaTime;
        mouseX = Input.GetAxis("Mouse X") * 100 * Time.fixedDeltaTime;
        mouseY = Input.GetAxis("Mouse Y") * 100 * Time.fixedDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
