using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadingPanelGroup;
    [SerializeField] private GameObject loadingScreenObject;
    [Range(1, 20)][SerializeField] float minimumLoadtime;
    private bool isLoading = false;
    AsyncOperation operation;





    //This coroutines are split because I needed to include to yield return statements to get what I was looking for.
    //The way this is implimented in the internal functions is such: FadeToBlack => AfterFade => LoadAsync => FadeFromBlack.
    public IEnumerator ChangeScenes(string _scene, float _fadeInDuration, float _fadeOutduration)
    {
        //If isLoading is equal to false, start loading
        if(isLoading == false)
        {
            yield return StartCoroutine(FadeToBlack(_fadeInDuration));
            StartCoroutine(AfterFade(_scene, _fadeOutduration));
        }
    }

    #region InternalFunctions
    private IEnumerator FadeToBlack(float _duration)
    {
        isLoading = true;
        float elapsedTime = 0;
        while (elapsedTime < _duration)
        {
            fadingPanelGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / _duration);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        fadingPanelGroup.alpha = 1f;
    }
    private IEnumerator LoadAsync(string _scene)
    {
        loadingScreenObject.SetActive(true);
        operation = SceneManager.LoadSceneAsync(_scene);

        float elapsedTime = 0;
        while (operation.progress < 1 || elapsedTime < minimumLoadtime)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        loadingScreenObject.SetActive(false);
    }
    private IEnumerator AfterFade(string _scene, float _fadeDuration)
    {
        yield return StartCoroutine(LoadAsync(_scene));
        StartCoroutine(FadeFromBlack(_fadeDuration));
    }
    private IEnumerator FadeFromBlack(float _duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < _duration)
        {
            fadingPanelGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / _duration);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        fadingPanelGroup.alpha = 0f;
        isLoading = false;
    }
    #endregion
}
