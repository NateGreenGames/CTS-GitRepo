using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour
{

    [SerializeField] float waveSpeed;
    [SerializeField] RectTransform[] waveObjects;


    [SerializeField] RectTransform boatObject;
    [SerializeField] float boatRotationFrequency;
    [SerializeField] float boatRotationAmplitude;

    private bool isAnimating;

    private void OnEnable()
    {
        isAnimating = true;
    }
    private void OnDisable()
    {
        isAnimating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating) Animate();
    }


    private void Animate()
    {
        //Rotates the boat back and forth using a sine wave.
        boatObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * boatRotationFrequency) * boatRotationAmplitude);

        //Iterates over the array of wave objects, moving them if they are positioned before the cuttoff, or teleporting them to the beginning of the line of they are after the cutoff.
        for (int i = 0; i < waveObjects.Length; i++)
        {
            if(waveObjects[i].anchoredPosition.x <= -136.5f)
            {
                waveObjects[i].anchoredPosition = new Vector2(91.5f, waveObjects[i].anchoredPosition.y);
            }
            else
            {
                waveObjects[i].anchoredPosition = new Vector2(waveObjects[i].anchoredPosition.x - (waveSpeed * Time.deltaTime), waveObjects[i].anchoredPosition.y);
            }
        }
    }
}
