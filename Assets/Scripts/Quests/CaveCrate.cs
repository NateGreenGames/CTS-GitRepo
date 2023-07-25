using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCrate : MonoBehaviour,IInteractable
{

    [SerializeField] GameObject lightningBottle;
    [SerializeField] MeshRenderer[] hologramBottle;
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }
    private Outline outlineReference;

    private void OnEnable()
    {
        QuestManager.functionMessageToSend += CheckForQuest;
    }
    private void OnDisable()
    {
        QuestManager.functionMessageToSend -= CheckForQuest;
    }

    public void OnInteract()
    {
        //If the player has a charged jar:
        ToggleActive();
        lightningBottle.SetActive(true);

        for (int i = 0; i < hologramBottle.Length; i++)
        {
            hologramBottle[i].enabled = false;
        }
        GameManager.gm.playerReference.GetComponentInChildren<ItemSwapping>().SwitchEmptyHand();
    }

    public void OnLookingAt()
    {
        //If the jar is interactable, and the jar is not charged, set the hud message to be "You need a charged jar" otherwise, set it to "Place jar"
    }

    // Start is called before the first frame update
    void Start()
    {
        outlineReference = gameObject.GetComponent<Outline>();
        HudMessage = "Place Jar";
        isInteractable = true;
    }

    private void ToggleActive()
    {
        outlineReference.enabled = !outlineReference.enabled;
        isInteractable = !isInteractable;
    }

    private void CheckForQuest(string _message)
    {
        if(_message == "Cave Active")
        {
            ToggleActive();
        }
    }
}
