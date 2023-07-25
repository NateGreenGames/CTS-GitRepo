using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//public enum eMixers {music,sound}

public class AudioManager : MonoBehaviour
{
    //Testing Method
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> buttonClips = new List<AudioClip>();

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    //Sound Clips
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip swimClip;
    [SerializeField] AudioClip[] walkClips;
    [SerializeField] AudioClip flareFireClip;
    [SerializeField] AudioClip flareReloadClip;
    [SerializeField] AudioClip boatSpeedClip;

    private void Awake()
    {
        LoadVolume();
    }

    void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, .4f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, .4f);
    }

    public void ButtonSFX()
    {
        AudioClip clip = buttonClips[Random.Range(0, buttonClips.Count)];
        audioSource.PlayOneShot(clip);
    }

    public void JumpSFX()
    {
        AudioClip clip = jumpClip;
        audioSource.PlayOneShot(clip);
    }

    public void SwimSFX()
    {
        AudioClip clip = swimClip;
        audioSource.PlayOneShot(clip);
    }

    public void WalkSFX()
    {
        AudioClip clip = walkClips[Random.Range(0, walkClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void FlareSFX(int _clip)
    {
        if (_clip == 0)
        {
            AudioClip clip = flareFireClip;
            audioSource.PlayOneShot(clip);
            // Debug.Log("FIRE IN THE HOLE!");
        }
        else if (_clip == 1)
        {
            AudioClip clip = flareReloadClip;
            audioSource.PlayOneShot(clip);
            // Debug.Log("RELOAD IN THE HOLE!");
        }
        
    }

    public void BoatMaxSpeedSFX()
    {
        AudioClip clip = boatSpeedClip;
        audioSource.PlayOneShot(clip);
    }

    public void PlayGlobalSound(AudioClip _audioToPlay, float _audioVolume)
    {
        audioSource.PlayOneShot(_audioToPlay, _audioVolume);
    }
}
