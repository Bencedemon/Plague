using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;

public class PlayerDataManager : MonoBehaviour
{
    public PlayerData playerData;

    public AudioMixer audioMixer;
    
    void Awake(){
        string path = Application.persistentDataPath + "/PlayerSettings.json";
        if(File.Exists(path)){
            playerData = LoadData();
        }else{
            playerData.mouseSensitivity=100;

            playerData.fov=60;
            playerData.quality=1;

            playerData.masterVolume=0;
            playerData.musicVolume=-20;
            playerData.sfxVolume=-20;

            playerData.fpsLimit=60;
        }
    }
    void Start(){
        string path = Application.persistentDataPath + "/PlayerSettings.json";
        if(File.Exists(path)){
            audioMixer.SetFloat("masterVolume",playerData.masterVolume);
            audioMixer.SetFloat("musicVolume",playerData.musicVolume);
            audioMixer.SetFloat("sfxVolume",playerData.sfxVolume);
		    QualitySettings.SetQualityLevel(playerData.quality);
            Application.targetFrameRate = playerData.fpsLimit;
		    QualitySettings.vSyncCount = (playerData.vsync) ? 1 :  0;
        }else{
            audioMixer.SetFloat("masterVolume",0);
            audioMixer.SetFloat("musicVolume",-20);
            audioMixer.SetFloat("sfxVolume",-20);
            Application.targetFrameRate = 60;
		    QualitySettings.vSyncCount = 0;
        }
    }
    private PlayerData LoadData(){
        string path = Application.persistentDataPath + "/PlayerSettings.json";
        if(File.Exists(path)){
            string data = File.ReadAllText(path);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
            return playerData;
        }else{
            return null;
        }
    }
    public void SaveData(){
        string data = JsonUtility.ToJson(playerData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerSettings.json", data);
    }
}
