using UnityEngine.InputSystem;

public class Widget_Confirm : MenuBase
{
    WidgetPause pause;

    private void Update()
    {
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            OnYesPressed();
        }
        if (Gamepad.current.bButton.wasPressedThisFrame)
        {
            pause = GetComponentInParent<WidgetPause>();
            pause.ActiveButtonsAll(true);
            GameManager.gm.eventSys.SetSelectedGameObject(pause.resumeBtn);
            Destroy(this.GetComponent<Widget_Confirm>().gameObject);
        }
    }

    public void OnYesPressed()
    {
        StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_FrontEndScene", 1.5f, 3f));
    }

    public void DeleteSave()
    {
       
    }
}
