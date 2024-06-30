using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool _enable =true;

    [SerializeField, Range(0,0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0,30)] private float _frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;


    public float _toggleSpeed = 1.0f;
    private Vector3 _startPos;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private PlayerMovement _playerMovement;


    // Start is called before the first frame update
    void Awake()
    {
        _startPos=_camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_enable) return;

        CheckMotion();
        ResetPosition();
        //_camera.LookAt(FocusTarget());
    }

    private void CheckMotion(){
        float speed = new Vector3(_playerMovement.x,0,_playerMovement.z).magnitude;

        if(_playerMovement.speedMultiplier<1) return;
        if(speed<=_toggleSpeed) return;
        if(!_controller.isGrounded) return;

        PlayMotion(FootStepMotion());
    }
    private void PlayMotion(Vector3 motion){
        _camera.localPosition += motion; 
    }

    private Vector3 FootStepMotion(){
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency*_playerMovement.speedMultiplier)*_amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency*_playerMovement.speedMultiplier/2)*_amplitude*2;
        return pos;
    }

    private void ResetPosition(){
        if(_camera.localPosition==_startPos)    return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition,_startPos,1*Time.deltaTime);
    }

    private Vector3 FocusTarget(){
        Vector3 pos = new Vector3(transform.position.x,transform.position.y-_cameraHolder.localPosition.y,transform.position.z);
        pos+= _cameraHolder.forward*15.0f;
        return pos;
    }
}
