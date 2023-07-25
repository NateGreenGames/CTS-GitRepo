using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WidgetPage : MenuBase
{
    CanvasHUD canvasHUD;

    public TextMeshProUGUI tNote;

    private void Start()
    {
        canvasHUD = GetComponentInParent<CanvasHUD>();
    }

    public void UpdateNoteText(string _text)
    {
        tNote.text = _text;
    }

    private void OnDestroy()
    {
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
    }
}
