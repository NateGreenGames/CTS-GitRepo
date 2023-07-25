using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;


public class Widget_NewGame : MenuBase
{
    public SaveSettings saveSettings;
    public GameObject yesBtn;

    private FeFunctions feFunctions;
    private CanvasFE canvasFE;


    private void Start()
    {
        saveSettings = GameManager.gm.GetComponent<SaveSettings>();
        feFunctions = GetComponentInParent<FeFunctions>();
        canvasFE = GetComponentInParent<CanvasFE>();
    }

    private void Update()
    {
        if (Gamepad.current.aButton.wasPressedThisFrame || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnYesPressed();
        }
        if (Gamepad.current.bButton.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Backspace))
        {
            feFunctions.ActiveButtonsAll(true); //buttons are interactible again
            feFunctions.ReselectNewGame(); //Controller can navigate buttons again
            Destroy(this.GetComponent<Widget_NewGame>().gameObject); //remove widget
        }

        
    }

    private void OnYesPressed()
    {
        //Something something reset save file
        LoadNewGame();
        //Load into Cabin Scene
        StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_Letter", 1.5f, 3f));
    }

    private void LoadNewGame()
    {
        saveSettings.Delete(saveSettings.player);
        saveSettings.NewGame();
    }
}
