using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float mouseSensitivity;

    public int resolution;
    public bool fullScreen;
    public int quality;
    public float fov;
    public bool vsync;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    public int fpsLimit;


    public PlayerData(float _mouseSensitivity,int _resolution,bool _fullScreen,int _quality,float _fov,bool _vsync,float _masterVolume,float _musicVolume,float _sfxVolume,int _fpsLimit){
        mouseSensitivity=_mouseSensitivity;
        resolution=_resolution;
        fullScreen=_fullScreen;
        quality=_quality;
        fov=_fov;
        vsync=_vsync;
        masterVolume=_masterVolume;
        musicVolume=_musicVolume;
        sfxVolume=_sfxVolume;
        fpsLimit=_fpsLimit;
    }
}
