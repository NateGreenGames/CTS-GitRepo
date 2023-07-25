using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectJellyfish : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    private GameManager gm;

    public void OnInteract()
    {
        if (gm.questManager.localQuestId == 10 && gm.playerReference.GetComponentInChildren<ItemSwapping>().LightningBottleEquipped() == true)
        {
            gm.playerProgressionUnlock.isLightningBottleCharged = true;
            gm.playerReference.GetComponentInChildren<ItemSwapping>().LightningBottleCharged();
            gm.playerReference.GetComponentInChildren<ItemSwapping>().SwitchLightningBottle();
            gm.questManager.AdvanceStep();
        }
        else
        {
            StartCoroutine(NoBottle());
        }
    }

    public void OnLookingAt()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        HudMessage = "Collect Jellyfish";
        isInteractable = true;
    }
    IEnumerator NoBottle()
    {
        HudMessage = "You need a jar";
        yield return new WaitForSeconds(2);
        HudMessage = "Collect Jellyfish";
    }
}
