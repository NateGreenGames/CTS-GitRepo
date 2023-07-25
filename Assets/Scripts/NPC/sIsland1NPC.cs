using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.IO;

public class sIsland1NPC : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }
    public bool gryffnFirstVisit;
    public GameObject flarePack;
    public GameObject questManager;

    [SerializeField] float minFidgetDelay, maxFidgetDelay;

    private GameManager gm;
    private Animator anim;
    private sFlarePickup flarePURef;
    private Outline flareOLRef;
    private Outline m_OL;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        anim = GetComponent<Animator>();
        m_OL = GetComponent<Outline>();
        flarePURef = flarePack.GetComponent<sFlarePickup>();
        flarePURef.enabled = false;

        flareOLRef = flarePack.GetComponent<Outline>();
        flareOLRef.enabled = false;

        isInteractable = true;
        HudMessage = "Talk";
        gryffnFirstVisit = true;
        StartCoroutine(RandomFidget());

    }

    public void OnInteract()
    {
        m_OL.enabled = false;
        isInteractable = false;
        GameManager.gm.canvasManager.canvasHUD.inPlayer.SetActive(false);
        gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
        anim.Play("Base Layer.anim_Greeting");
        if (gm.questManager.localQuestId == 0)
        {
            //Enviro.EnviroManager.instance.Time.SetTimeOfDay(18f);
            DialogueManager.StartConversation("GryffnFirstVisit", this.transform, gm.playerReference.transform);
        }
        else if (gm.questManager.localQuestId == 1) DialogueManager.StartConversation("GryffnNeedsCrates", this.transform, gm.playerReference.transform);
        else if (gm.questManager.localQuestId == 2) DialogueManager.StartConversation("GryffnGetsCrates", this.transform, gm.playerReference.transform);
        else if (gm.questManager.localQuestId > 2) DialogueManager.StartConversation("GryffnPostFirstVisit", this.transform, gm.playerReference.transform);
    }

    public void OnLookingAt()
    {

    }


    public void OnConversationEnd(Transform actor)
    {
        StartCoroutine(JumpFix());
        Debug.Log(string.Format("Ending conversation with {0}", actor.name));
        if (gm.questManager.localQuestId == 0)
        {
            StartCoroutine(GiveFlareGun());
            flarePURef.enabled = true;
            flareOLRef.enabled = true;
            gm.questManager.AdvanceStep();
        }
        else if (gm.questManager.localQuestId == 2)
        {
            anim.Play("Base Layer.anim_Greeting");
            gm.questManager.AdvanceStep();
        }
        else if (gm.questManager.localQuestId > 0) anim.Play("Base Layer.anim_Greeting");

        isInteractable = true;
        m_OL.enabled = true;
    }

    IEnumerator JumpFix()
    {
        yield return new WaitForSeconds(2);
        gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
        GameManager.gm.canvasManager.canvasHUD.inPlayer.SetActive(true);
    }
    IEnumerator GiveFlareGun()
    {
        anim.Play("Base Layer.anim_GiveItem");
        yield return new WaitForSeconds(2);
        gm.playerReference.GetComponentInChildren<ItemSwapping>().ObtainFlareGun();
    }

    IEnumerator RandomFidget()
    {
        float timeToWait = Random.Range(minFidgetDelay, maxFidgetDelay);
        yield return new WaitForSeconds(timeToWait);
        anim.SetTrigger("Fidget");
        StartCoroutine(RandomFidget());
    }
}
