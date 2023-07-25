using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleSequence : MonoBehaviour
{

    [SerializeField] Renderer[] cavePaintings;
    [SerializeField] float timeToTransition, animationDuration, timeBeforeShowingCredits, lengthOfCreditSequence;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeforeLightingRoutine());
    }

    private IEnumerator LightUpRoutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < timeToTransition)
        {
            for (int i = 0; i < cavePaintings.Length; i++)
            {
                cavePaintings[i].material.SetFloat("_Alpha", Mathf.InverseLerp(0, timeToTransition, elapsedTime));
            }
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        for (int i = 0; i < cavePaintings.Length; i++)
        {
            cavePaintings[i].material.SetFloat("_Alpha", 1);
        }
        StartCoroutine(AfterLightingRoutine());
    }

    private IEnumerator BeforeLightingRoutine()
    {
        float elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        StartCoroutine(LightUpRoutine());
    }

    private IEnumerator AfterLightingRoutine()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeBeforeShowingCredits)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        StartCoroutine(CreditsRoutine());
    }

    private IEnumerator CreditsRoutine()
    {
        Instantiate(Resources.Load("Widgets/FE_Widgets/Widget_CreditsHiNate") as
            GameObject, GameManager.gm.canvasManager.canvasHUD.transform);

        float elapsedTime = 0;
        while (elapsedTime < lengthOfCreditSequence)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_FrontEndScene", 3f, 3f));
    }
}
