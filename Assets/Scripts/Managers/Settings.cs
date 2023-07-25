using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    PlayerMovement movement;

    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] Slider lookSlider;

    public const string MIXER_MUSIC = "MusVol";
    public const string MIXER_SFX = "SoundVol";

    //RESOLUTION STUFF
    [SerializeField] TMP_Dropdown resField1;
    [SerializeField] TMP_Dropdown resField2;
    [SerializeField] TMP_Dropdown resField3;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        lookSlider.onValueChanged.AddListener(SetPlayerSensitivity);
    }

    private void Start()
    { 
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, .4f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, .4f);
        //lookSlider.value = PlayerPrefs.GetFloat()
    }

    public void SetPlayerSensitivity(float _sliderVal)
    {
        movement.sensitivity = _sliderVal;
        Debug.Log(movement.sensitivity);
    }

    public void SetMusicVolume(float _volume)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(_volume) * 20);
    }

    public void SetSFXVolume(float _volume)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(_volume) * 20);
    }

    public void SetFullscreen(bool setFullscreen)
    {
        Screen.fullScreen = setFullscreen;
    }
    
    public void SetRes(int _ResVal)
    {
        //Screen.SetResolution(_ResVal);
    }

    public void ApplyNewResolution()
    {
        //resField1.value
        //resField2.value
        //resField2.value
        //Screen.SetResolution();
    }
}
