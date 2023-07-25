using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class WidgetPause : MenuBase
{
    [SerializeField] GameObject widgetSettings;
    public GameObject resumeBtn;
    public GameObject videoSettingsBtn;
    public Button[] pauseButtons;

    private void Start()
    {
        GameManager.gm.eventSys.SetSelectedGameObject(resumeBtn);
    }

    public void OnResume()
    {
        if (GameManager.gm.playerReference.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement)) //dont get component a bunch next to each other
        {
            if (!playerMovement.movementEnabled)
            {
                playerMovement.movementEnabled = true;
                GameManager.gm.canvasManager.canvasHUD.hudFunctions.LockMouse();
            }
        }
        GameManager.gm.canvasManager.canvasHUD.pauseOpen = false;
        Destroy(this.gameObject);
    }

    public void OnExitGame()
    {
        Instantiate(Resources.Load("Widgets/INGAME_Widgets/Widget_Confirm") as GameObject, this.transform);
        ActiveButtonsAll(false);
    }

    public void OnSettings()
    {
        widgetSettings.SetActive(true);
        ActiveButtonsAll(false);
        GameManager.gm.eventSys.SetSelectedGameObject(videoSettingsBtn);
    }

    private void OnDestroy()
    {
        GameManager.gm.canvasManager.canvasHUD.hudFunctions.LockMouse();
    }

    public void ActiveButtonsAll(bool _activeness)
    {
        for (int buttons = 0; buttons < pauseButtons.Length; buttons++)
        {
            ActiveButtons(buttons, _activeness);
        }
    }

    public void ActiveButtons(int _buttonToHide, bool _activeness)
    {
        pauseButtons[_buttonToHide].interactable = _activeness;
    }

    public void ReselectResume()
    {
        GameManager.gm.eventSys.SetSelectedGameObject(resumeBtn);
    }
}
