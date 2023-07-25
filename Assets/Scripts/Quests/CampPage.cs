using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampPage : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }


    public void OnInteract()
    {
        if (GameManager.gm.questManager.localQuestId == 6)
        {
            StartCoroutine(TextPopup());
            GameManager.gm.questManager.AdvanceStep();
            //Destroy(this.gameObject);
        }
        else
        {
            StartCoroutine(WrongStep());
        }
    }

    public void OnLookingAt()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        isInteractable = false;
        GetComponent<Outline>().enabled = false;
        HudMessage = "Collect Page";
    }

    private void OnEnable()
    {
        QuestManager.functionMessageToSend += EnablePage;
    }

    private void OnDisable()
    {
        QuestManager.functionMessageToSend -= EnablePage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WrongStep()
    {
        HudMessage = "You don't need this.";
        yield return new WaitForSeconds(2);
        HudMessage = "Collect Page";
    }

    public void EnablePage(string _string)
    {
        if (_string == "EnablePage")
        {
            isInteractable = true;
            GetComponent<Outline>().enabled = true;
        }
    }

    IEnumerator TextPopup()
    {
        GameManager.gm.canvasManager.canvasHUD.hudFunctions.SetMissionText("", "Hmm, this looks like my Dad's but it's not complete", false);
        yield return new WaitForSeconds(2);
        GameManager.gm.canvasManager.canvasHUD.hudFunctions.SetMissionText("", "", false);
    }


}
