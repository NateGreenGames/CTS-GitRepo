using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadLetter : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }
    [SerializeField] private AudioClip letterDialogue;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        isInteractable = true;
        HudMessage = "Read";
    }

    public void OnInteract()
    {
        if (gm.skipLetter)
        {
            isInteractable = false;
            StartCoroutine(gm.fadeManager.ChangeScenes("CTS_CabinScene", 3f, 3f));
            gm.playerProgressionUnlock.isAstrolabeUnlocked = true;
        }
        else StartCoroutine(Ordering());

    }

    public void OnLookingAt()
    {
        //Do nothing
    }


    private IEnumerator Ordering()
    {
        Widget_LetterTextBehavior LTBref = Instantiate(Resources.Load("Widgets/" + "Widget_LetterText") as GameObject, gm.canvasManager.canvasHUD.transform).GetComponent<Widget_LetterTextBehavior>();
        isInteractable = false;
        yield return StartCoroutine(gm.canvasManager.canvasHUD.hudFunctions.DisplayCassetteWidget(letterDialogue));
        StartCoroutine(LTBref.FadeOut());
        gm.playerProgressionUnlock.isAstrolabeUnlocked = true;
    }
}
