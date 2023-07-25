using UnityEngine;
using System.Collections;

public class HUDFunctions : MonoBehaviour
{
    public GameObject spawnPt;

    private CanvasHUD canvasHUD;
    private GameManager gm;
    private void Start()
    {
        gm = GameManager.gm;
        canvasHUD = gm.canvasManager.canvasHUD;
        ToggleUIModes(false);
    }

    public void UpdatePrompt(string _promptText, int _promptImage)
    {
        canvasHUD.promptText.text = _promptText;
        canvasHUD.promptButton.sprite = canvasHUD.promptButtonRefs[_promptImage];
    }

    public void SetMissionText(string _header, string _task, bool _setBG)
    {
        if (_setBG)
        {
            canvasHUD.iPanelBG.color = new Color(canvasHUD.iPanelBG.color.r, canvasHUD.iPanelBG.color.g, canvasHUD.iPanelBG.color.b, canvasHUD.startingAlpha);
        }
        else if (!_setBG)
        {
            canvasHUD.iPanelBG.color = new Color(canvasHUD.iPanelBG.color.r, canvasHUD.iPanelBG.color.g, canvasHUD.iPanelBG.color.b, 0);
        }

        canvasHUD.headerText.text = _header;
        canvasHUD.taskText.text = _task;
        canvasHUD.panelHint.SetActive(true);
    }

    public void LockMouse() //do we even still use these??
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse() 
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OpenPauseMenu() //freeze movement, instantiate pause menu
    {
        canvasHUD.pauseOpen = true;
        gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
        canvasHUD.pauseMenu = Instantiate(Resources.Load("Widgets/INGAME_Widgets/Widget_Pause") as
            GameObject, spawnPt.transform);
        UnlockMouse();
    }

    public void ClosePauseMenu()
    {
        canvasHUD.pauseOpen = false;
        gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
        LockMouse();
        Destroy(canvasHUD.pauseMenu);
        canvasHUD.pauseMenu = null;
    }


    public IEnumerator DisplayCassetteWidget(AudioClip _clipToPlay)
    {
        gm.audioManager.PlayGlobalSound(_clipToPlay, 0.5f);
        GameObject cassetteWidget = Instantiate(Resources.Load("Widgets/INGAME_Widgets/Widget_Cassette") 
            as GameObject, gm.canvasManager.canvasHUD.transform);
        yield return new WaitForSeconds(_clipToPlay.length);
        Destroy(cassetteWidget);
    }

    public IEnumerator FadeoutDpad(float _duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _duration)
        {
            canvasHUD.dPadAlpha.alpha = Mathf.Lerp(1f, 0f, elapsedTime / _duration);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        canvasHUD.dPadAlpha.alpha = 0f;
    }

    public void ToggleUIModes(bool _wahoo)
    {
        if (_wahoo == true)
        {
            canvasHUD.inBoat.SetActive(true);
            canvasHUD.inPlayer.SetActive(false);
        }
        else if (_wahoo == false)
        {
            canvasHUD.inBoat.SetActive(false);
            canvasHUD.inPlayer.SetActive(true);
        }
    }
}
