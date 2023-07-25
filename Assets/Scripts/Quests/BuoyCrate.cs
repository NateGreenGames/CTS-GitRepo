using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuoyCrate : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }


    public void OnInteract()
    {
        if (GameManager.gm.questManager.localQuestId == 1)
        {
            GameManager.gm.questManager.AdvanceStep();
            Destroy(this.gameObject);
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
        HudMessage = "Collect Crate";
    }

    private void OnEnable()
    {
        QuestManager.functionMessageToSend += EnableCrates;
    }

    private void OnDisable()
    {
        QuestManager.functionMessageToSend -= EnableCrates;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WrongStep()
    {
        HudMessage = "You don't need this.";
        yield return new WaitForSeconds(2);
        HudMessage = "Collect Crate";
    }

    public void EnableCrates(string _string)
    {
        if (_string == "EnableCrates")
        {
            isInteractable = true;
            GetComponent<Outline>().enabled = true;
        }
    }

    
}
