using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayAndBob : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] MouseLook mouseLook;

    [Header("Settings")]
    public bool sway = true;
    public bool swayRotation = true;
    public bool bobOffset = true;
    public bool bobSway = true;


    private Vector3 offset;

    void Awake(){
        offset = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!playerMovement.canMove) return;
        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        CompositePositionRotation();
    }

    Vector2 walkInput;
    Vector2 lookInput;


    void GetInput(){
        walkInput.x=playerMovement.x;
        walkInput.y=playerMovement.z;
        walkInput = walkInput.normalized;

        lookInput.x = mouseLook.mouseX;
        lookInput.y = mouseLook.mouseY;
    }

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    void Sway(){
        if(sway == false){swayPos = Vector3.zero; return;}

        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance,maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance,maxStepDistance);

        swayPos = invertLook;
    }

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    void SwayRotation(){
        if(swayRotation == false){swayEulerRot = Vector3.zero; return;}

        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep,maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep,maxRotationStep);

        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin {get => Mathf.Sin(speedCurve);}
    float curveCos {get => Mathf.Cos(speedCurve);}
    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;

    void BobOffset(){
        speedCurve += Time.fixedDeltaTime * playerMovement.speedMultiplier;

        if(bobOffset == false){bobPosition = Vector3.zero;return;}

        bobPosition.x = 
            (curveCos*bobLimit.x*1)
            - (walkInput.x * travelLimit.x);
        bobPosition.y = 
            (curveSin*bobLimit.y)
            - (walkInput.x * travelLimit.y);

        bobPosition.z =
            - (walkInput.y * travelLimit.z);
    }

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    void BobRotation(){
        if(bobSway==false){bobEulerRotation=Vector3.zero;return;}

        bobEulerRotation.x=(walkInput != Vector2.zero ? multiplier.x*(Mathf.Sin(2*speedCurve)):
                                                        multiplier.x*(Mathf.Sin(2*speedCurve)/2));
        bobEulerRotation.y=(walkInput != Vector2.zero ? multiplier.y*curveCos               :0);
        bobEulerRotation.z=(walkInput != Vector2.zero ? multiplier.z*curveCos*walkInput.x   :0);
    }


    [Header("Composite")]
    float smooth = 10f;
    float smoothRot = 12f;

    void CompositePositionRotation(){
        transform.localPosition=
            Vector3.Lerp(transform.localPosition,
            swayPos + bobPosition + offset,
            Time.fixedDeltaTime * smooth);
        transform.localRotation=
            Quaternion.Slerp(transform.localRotation,
            Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation),
            Time.fixedDeltaTime*smoothRot);
    }
}
