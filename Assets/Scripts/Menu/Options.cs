using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Options : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject titleScreen;
    public GameObject titleScreenCamera;
    public GameObject options;
    public GameObject optionsCamera;
    //tabs
    [Header("Tabs")]
    public GameObject controlPanel;
    public GameObject videoPanel;
    public GameObject audioPanel;

	//Control
    [Header("Control")]
	public float mouseSensitivityValue;
	public Slider mouseSensitivitySlider;
	public TMP_Text mouseText;
	

	//Video
    [Header("Video")]
	public TMP_Dropdown resolutionDropdown;

	public Slider fovSlider;
	public TMP_Text fovSliderValue;

	public TMP_Dropdown qualityDropdown;

	public Toggle fullscreenToggle;

	public Toggle vsyncToggle;
	public TMP_Dropdown fpsDropdown;


	//Audio
    [Header("Audio")]
	public AudioMixer audioMixer;

	public Slider masterSlider;
	public TMP_Text masterSliderValue;

	public Slider musicSlider;
	public TMP_Text musicSliderValue;

	public Slider sfxSlider;
	public TMP_Text sfxSliderValue;

	//Save
    [Header("Save")]

	public Animator animator;
	
	
	Resolution[] resolutions;

    private PlayerDataManager playerDataManager;
    void Awake(){
        playerDataManager = FindObjectOfType<PlayerDataManager>();
    }
	
	// Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
		
		resolutionDropdown.ClearOptions();
		
		List<string> options = new List<string>();
		
		int currentResolutionIndex=0;
		for(int i=0;i<resolutions.Length;i++){
			string option = resolutions[i].width +" x "+resolutions[i].height;
			options.Add(option);
			
			if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height){
				currentResolutionIndex=i;
			}
		}
		
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = playerDataManager.playerData.resolution;
		resolutionDropdown.RefreshShownValue();

		qualityDropdown.value = playerDataManager.playerData.quality;
		qualityDropdown.RefreshShownValue();

		fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);

		vsyncToggle.SetIsOnWithoutNotify(playerDataManager.playerData.vsync);
		fpsDropdown.interactable=!playerDataManager.playerData.vsync;

		fpsDropdown.value = playerDataManager.playerData.fpsLimit/30-1; //This is stupid but works
		fpsDropdown.RefreshShownValue();

		mouseSensitivitySlider.SetValueWithoutNotify(playerDataManager.playerData.mouseSensitivity);
		mouseText.text=playerDataManager.playerData.mouseSensitivity+"";

		fovSlider.SetValueWithoutNotify(playerDataManager.playerData.fov);
		fovSliderValue.text=playerDataManager.playerData.fov+"";


		masterSlider.SetValueWithoutNotify(playerDataManager.playerData.masterVolume);
		masterSliderValue.text=((int)playerDataManager.playerData.masterVolume+80)+"";

		musicSlider.SetValueWithoutNotify(playerDataManager.playerData.musicVolume);
		musicSliderValue.text=((int)playerDataManager.playerData.musicVolume+80)+"";

		sfxSlider.SetValueWithoutNotify(playerDataManager.playerData.sfxVolume);
		sfxSliderValue.text=((int)playerDataManager.playerData.sfxVolume+80)+"";

		Screen.fullScreen = playerDataManager.playerData.fullScreen;
    }
	
	public void SensitivitySlider(float value){
		playerDataManager.playerData.mouseSensitivity=value;
		mouseText.text=value+"";
	}
	
	public void SetFullscreen(bool isFullscreen){
		playerDataManager.playerData.fullScreen=isFullscreen;
		Screen.fullScreen = isFullscreen;
	}
	
	public void SetResolution(int resolutionIndex){
		playerDataManager.playerData.resolution=resolutionIndex;
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width,resolution.height, Screen.fullScreen);
	}

	public void SetQuality(int index){
		playerDataManager.playerData.quality=index;
		QualitySettings.SetQualityLevel(index);
		QualitySettings.vSyncCount = (playerDataManager.playerData.vsync) ? 1 :  0;
	}

	public void FovSliderChange(float value){
		playerDataManager.playerData.fov=value;
		fovSliderValue.text=""+value;
	}
	public void SetVsync(bool isVsync){
		playerDataManager.playerData.vsync=isVsync;
		QualitySettings.vSyncCount = (isVsync) ? 1 :  0;
		fpsDropdown.interactable=!isVsync;
	}

	public void FpsDropdownChange(int value){
		int limit = int.Parse(fpsDropdown.options[value].text);

		playerDataManager.playerData.fpsLimit=limit;
        Application.targetFrameRate = limit;
	}
	public void MasterVolumeSliderChange(float volume){
		playerDataManager.playerData.masterVolume=volume;
		masterSliderValue.text=((int)volume+80)+"";
		audioMixer.SetFloat("masterVolume",volume);
	}
	public void MusicVolumeSliderChange(float volume){
		playerDataManager.playerData.musicVolume=volume;
		musicSliderValue.text=((int)volume+80)+"";
		audioMixer.SetFloat("musicVolume",volume);
	}
	public void SFXVolumeSliderChange(float volume){
		playerDataManager.playerData.sfxVolume=volume;
		sfxSliderValue.text=((int)volume+80)+"";
		audioMixer.SetFloat("sfxVolume",volume);
	}
    public void Control(){
        //audioSource.Play();
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlPanel.SetActive(true);
    }
    public void Video(){
        //audioSource.Play();
        controlPanel.SetActive(false);
        audioPanel.SetActive(false);
        videoPanel.SetActive(true);
    }
    public void Audio(){
        //audioSource.Play();
        videoPanel.SetActive(false);
        controlPanel.SetActive(false);
        audioPanel.SetActive(true);
    }
	public void SaveData(){
        //audioSource.Play();
		animator.Play("Fade");
		playerDataManager.SaveData();
	}
    public void Back(){
        //audioSource.Play();
        options.SetActive(false);
		if(optionsCamera!=null)
        	optionsCamera.SetActive(false);
        titleScreen.SetActive(true);
		if(titleScreenCamera!=null)
        	titleScreenCamera.SetActive(true);
    }
}
