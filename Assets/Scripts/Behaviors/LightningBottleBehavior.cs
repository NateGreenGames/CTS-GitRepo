using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBottleBehavior : MonoBehaviour
{
    public GameObject lightningParticle;
    private bool isCharged;

    private void Start()
    {
        isCharged = false;
        if (!isCharged)
        {
            lightningParticle.SetActive(false);
        }
        else lightningParticle.SetActive(true);
    }
}
