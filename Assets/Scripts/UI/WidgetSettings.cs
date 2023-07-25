using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WidgetSettings : MonoBehaviour
{
    public GameObject[] menusToSpawn;
    [SerializeField] GameObject objWidgetSettings;
    [SerializeField] GameObject btnOptionsDefault;
    [SerializeField] GameObject btnVideoDefault;
    [SerializeField] GameObject btnAudioDefault;
    [SerializeField] GameObject btnGameplayerDefault;

    private int activeMenu;

    FeFunctions feFunctions;
    WidgetPause pause;

    private void Start()
    {
        if (GameManager.gm.curScene == 0)
        {
            feFunctions = GetComponentInParent<FeFunctions>();
        }
        else
        {
            pause = GetComponentInParent<WidgetPause>();
        }
    }

    private void Update()
    {
        if (GameManager.gm.curScene == 0)
        {
            if (Gamepad.current.bButton.wasPressedThisFrame && activeMenu != 0) //this doesnt reset, it just does the else
            {
                ResetSubmenu();
                menusToSpawn[0].SetActive(true);
                activeMenu = 0;
                GameManager.gm.eventSys.SetSelectedGameObject(feFunctions.canvasFE.newGameBtn);
            }
            else if (Gamepad.current.bButton.wasPressedThisFrame && activeMenu == 0)
            {
                feFunctions.ActiveButtonsAll(true); //buttons are interactible again
                feFunctions.ReselectNewGame(); //Controller can navigate buttons again
                objWidgetSettings.SetActive(false);
            }
        }
        else
        {
            if (Gamepad.current.bButton.wasPressedThisFrame && activeMenu != 0) //this doesnt reset, it just does the else
            {
                ResetSubmenu();
                menusToSpawn[0].SetActive(true);
                activeMenu = 0;
                GameManager.gm.eventSys.SetSelectedGameObject(btnOptionsDefault);
            }
            else if (Gamepad.current.bButton.wasPressedThisFrame && activeMenu == 0)
            {
                pause.ActiveButtonsAll(true); //buttons are interactible again
                pause.ReselectResume(); //Controller can navigate buttons again
                objWidgetSettings.SetActive(false);
            }
        }
    }

    public void OnVideoClicked()
    {
        ResetSubmenu();
        menusToSpawn[1].SetActive(true);
        activeMenu = 1;
        if (feFunctions == null)
        {
            GameManager.gm.eventSys.SetSelectedGameObject(btnVideoDefault);
        }
        else
        {
            GameManager.gm.eventSys.SetSelectedGameObject(btnVideoDefault);
        }
    }

    public void OnAudioClicked()
    {
        ResetSubmenu();
        menusToSpawn[2].SetActive(true);
        activeMenu = 2;
        if (feFunctions == null)
        {
            GameManager.gm.eventSys.SetSelectedGameObject(btnAudioDefault);
        }
        else
        {
            GameManager.gm.eventSys.SetSelectedGameObject(btnAudioDefault);
        }
    }

    public void OnGameplayPressed()
    {
        ResetSubmenu();
        menusToSpawn[3].SetActive(true);
        activeMenu = 3;
        if (feFunctions == null)
        {
            GameManager.gm.eventSys.SetSelectedGameObject(btnGameplayerDefault);
        }
        else
        {
            GameManager.gm.eventSys.SetSelectedGameObject(btnGameplayerDefault);
        }
    }

    public void ResetSubmenu()
    {
        for (int i = 0; i < menusToSpawn.Length; i++)
        {
            menusToSpawn[i].SetActive(false);
        }
    }
}
