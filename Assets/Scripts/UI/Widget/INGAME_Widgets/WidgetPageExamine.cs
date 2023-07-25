using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetPageExamine : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    public string noteText;

    private void Start()
    {
        HudMessage = "Read Note";
        isInteractable = true;
    }

    public void OnLookingAt()
    {

    }

    public void OnInteract()
    {
        WidgetPage widgetRef;

        widgetRef = Instantiate(Resources.Load("Widgets/INGAME_Widgets/Widget_Page") as
            GameObject, GameManager.gm.canvasManager.canvasHUD.transform).GetComponent<WidgetPage>();
        widgetRef.UpdateNoteText(noteText);

        GameManager.gm.canvasManager.canvasHUD.hudFunctions.UnlockMouse();
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
    }
}
