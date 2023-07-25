using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Widget_LetterTextBehavior : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    public float fadeTime;

    private void Start()
    {
        fadeGroup = gameObject.GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn());
    }


    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while(elapsedTime < fadeTime)
        {
            fadeGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            fadeGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_CabinScene", 3f, 3f));
    }
}
