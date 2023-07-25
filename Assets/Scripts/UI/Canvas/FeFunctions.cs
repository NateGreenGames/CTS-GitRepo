using UnityEngine;

public class FeFunctions : MonoBehaviour
{
    public CanvasFE canvasFE;

    public void OnNewGame()
    {
        if (canvasFE.hasSaveFile)
        {
            Instantiate(Resources.Load("Widgets/FE_Widgets/" + "Widget_NewGame") as
            GameObject, canvasFE.spwnSettings.transform);
        }
        else
        {
            StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_Letter", 1.5f, 3f));
            canvasFE.OnYesPressed();
        }
        ActiveButtonsAll(false);
    }

    public void OnContinue()
    {
        canvasFE.LoadContinueGame(canvasFE.savedSceneChecker());
        ActiveButtonsAll(false);
    }

    public void OnSettings()
    {
        canvasFE.settingsMenu.SetActive(true);
        ActiveButtonsAll(false);
        GameManager.gm.eventSys.SetSelectedGameObject(canvasFE.videoSettingsBtn);
    }

    public void OnCredits()
    {
        Instantiate(Resources.Load("Widgets/FE_Widgets/Widget_Credits") as
            GameObject, this.transform);
        ActiveButtonsAll(false);
    }

    public void OnExitPressed()
    {
        Instantiate(Resources.Load("Widgets/FE_Widgets/" + "Widget_Exit") as
            GameObject, this.transform);
        ActiveButtonsAll(false);
    }

    public void ActiveButtons(int _buttonToHide, bool _activeness)
    {
        canvasFE.mainButtons[_buttonToHide].interactable = _activeness;
    }

    public void ActiveButtonsAll(bool _activeness)
    {
        for (int buttons = 0; buttons < canvasFE.mainButtons.Length; buttons++)
        {
            ActiveButtons(buttons, _activeness);
        }
        
        if (!GameManager.gm.playerProgressionUnlock.haveSavedFile)
        {
            canvasFE.mainButtons[1].interactable = false;
        }
        
    }

    public void ReselectNewGame()
    {
        GameManager.gm.eventSys.SetSelectedGameObject(canvasFE.newGameBtn);
    }

    public void WiggleButtonPanel()
    {
        canvasFE.grpMenuLayout.enabled = true;
    }
}
