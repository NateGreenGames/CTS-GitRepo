using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island2TimeZone : MonoBehaviour
{

    [SerializeField] float transitionSpeed;
    [SerializeField] float timeToTransitionTo;

    [SerializeField] bool isHidden;



    private void Start()
    {
        if (isHidden)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(TimeLerp());
        Enviro.EnviroManager.instance.Time.Settings.simulate = false;
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        Enviro.EnviroManager.instance.Time.Settings.simulate = true;
    }


    private IEnumerator TimeLerp()
    {
        while (Enviro.EnviroManager.instance.Time.hours != timeToTransitionTo)
        {
            Enviro.EnviroManager.instance.Time.SetTimeOfDay(Enviro.EnviroManager.instance.Time.GetTimeOfDay() + transitionSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
