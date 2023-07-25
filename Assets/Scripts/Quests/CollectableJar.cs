using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableJar : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    public void OnInteract()
    {
        if (GameManager.gm.questManager.localQuestId == 8)
        {
            GameManager.gm.playerReference.GetComponentInChildren<ItemSwapping>().ObtainLightningBottle();
            GameManager.gm.questManager.AdvanceStep();
            //GameManager.gm.canvasManager.canvasHUD.tutorialHUD.DiscoverItemUI(4);
            Destroy(this.gameObject);
        }

    }

    public void OnLookingAt()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        HudMessage = "Collect Jar";
        isInteractable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WrongStep()
    {
        HudMessage = "You have no need for this";
        yield return new WaitForSeconds(2);
        HudMessage = "Collect Jar";
    }
}
