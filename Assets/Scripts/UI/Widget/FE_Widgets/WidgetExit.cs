using UnityEngine;
using UnityEngine.InputSystem;

public class WidgetExit : MenuBase
{
    FeFunctions feFunctions;

    private void Start()
    {
        feFunctions = GetComponentInParent<FeFunctions>();
    }

    private void Update()
    {
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            OnYesPressed();
        }
        if (Gamepad.current.bButton.wasPressedThisFrame)
        {
            feFunctions.ActiveButtonsAll(true); //buttons are interactible again
            feFunctions.ReselectNewGame(); //Controller can navigate buttons again
            Destroy(this.GetComponent<WidgetExit>().gameObject);
        }
    }

    public void OnYesPressed()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }
}
