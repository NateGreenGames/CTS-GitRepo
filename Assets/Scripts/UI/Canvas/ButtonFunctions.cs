using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ButtonFunctions : MonoBehaviour
{
    public AudioSource buttonSounds;
    public AudioClip[] hoverClips; //set up sound
    public AudioClip[] selectClips; //conclusion sound
    private int soundToPlayClick = 0;
    private int soundToPlayHover = 0;

    public void HoverSound()
    {
        buttonSounds.PlayOneShot(hoverClips[soundToPlayHover]);
        soundToPlayHover++;
        if (soundToPlayHover > hoverClips.Length - 1) soundToPlayHover = 0;
    }
    public void ClickSound()
    {
        buttonSounds.PlayOneShot(selectClips[soundToPlayClick]);
        soundToPlayClick++;
        if (soundToPlayClick > selectClips.Length - 1) soundToPlayClick = 0;
    }
}
