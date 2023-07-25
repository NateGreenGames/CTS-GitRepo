using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.IO;

public class Island2NPC : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    public GameObject collectJar;
    public QuestManager questManager;

    private Animator anim;

    [SerializeField] float minFidgetDelay, maxFidgetDelay;


    public void OnInteract()
    {
        GetComponent<Outline>().enabled = false;
        isInteractable = false;
        GameManager.gm.canvasManager.canvasHUD.inPlayer.SetActive(false);
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
        if (questManager.localQuestId == 5) DialogueManager.StartConversation("CaliFirstVisit", this.transform, GameManager.gm.playerReference.transform);
        else if (questManager.localQuestId == 6) DialogueManager.StartConversation("CaliPostFirstVisit", this.transform, GameManager.gm.playerReference.transform);
        else if (questManager.localQuestId == 7) DialogueManager.StartConversation("CaliNeedsJar", this.transform, GameManager.gm.playerReference.transform);
        else if (questManager.localQuestId == 9 || questManager.localQuestId == 10) DialogueManager.StartConversation("CaliHasJar", this.transform, GameManager.gm.playerReference.transform);
        else if (questManager.localQuestId == 11) DialogueManager.StartConversation("CaliChargedJar", this.transform, GameManager.gm.playerReference.transform);
        else if (questManager.localQuestId >= 12) DialogueManager.StartConversation("CaliPostFirstVisit", this.transform, GameManager.gm.playerReference.transform);

    }

    public void OnLookingAt()
    {

    }


    public void OnConversationEnd(Transform actor)
    {
        StartCoroutine(JumpFix());
        Debug.Log(string.Format("Ending conversation with {0}", actor.name));
        if (questManager.localQuestId == 5) questManager.AdvanceStep();
        if (questManager.localQuestId == 7)
        {
            questManager.AdvanceStep();
        }
        if (questManager.localQuestId == 9) questManager.AdvanceStep(); //TODO: Make Cali move to near Jellyfish.
        if (questManager.localQuestId == 11) questManager.AdvanceStep();
        isInteractable = true;
        GetComponent<Outline>().enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        questManager = GameManager.gm.questManager;
        isInteractable = true;
        HudMessage = "Talk";
        StartCoroutine(RandomFidget());
    }

    IEnumerator JumpFix()
    {
        yield return new WaitForSeconds(1);
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
        GameManager.gm.canvasManager.canvasHUD.inPlayer.SetActive(true);
    }

    IEnumerator RandomFidget()
    {
        float timeToWait = Random.Range(minFidgetDelay, maxFidgetDelay);
        yield return new WaitForSeconds(timeToWait);
        anim.SetTrigger("Fidget");
        StartCoroutine(RandomFidget());
    }
}