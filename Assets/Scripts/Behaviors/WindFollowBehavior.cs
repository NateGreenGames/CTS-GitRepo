using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFollowBehavior : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, GameManager.gm.crestReference.wavesReference._waveDirectionHeadingAngle - 90, transform.rotation.eulerAngles.z));
    }

    public void ChangeAlphaZero()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        Color startColor = mainModule.startColor.color;
        startColor.a = 0f; 
        mainModule.startColor = startColor;
    }

    public void ChangeAlphaOne()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        Color startColor = mainModule.startColor.color;
        startColor.a = 1f; 
        mainModule.startColor = startColor;
    }
}
