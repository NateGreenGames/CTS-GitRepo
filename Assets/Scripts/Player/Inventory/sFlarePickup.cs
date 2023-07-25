using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sFlarePickup : MonoBehaviour, IInteractable, ICollectable
{
    public static event HandleFlareCollected OnFlareCollected;
    public delegate void HandleFlareCollected(ItemData itemData);
    public ItemData flareData;
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }
    [Header("Allows you to choose how many flares the pickup gives the player")]
    public int flareQuantity;

    public void OnLookingAt()
    {

    }
    public void OnInteract()
    {
        OnCollect();
    }

    // Start is called before the first frame update
    void Start()
    {
        isInteractable = true;
        HudMessage = "Collect Flares";
    }

    public void OnCollect()
    {
        //On pickup it allots the amount of flares dictated in inspector to the players inventory.
        // GameManager.gm.playerReference.GetComponent<Inventory>().GetFlares(flareQuantity);
        OnFlareCollected?.Invoke(flareData);
        Destroy(this.gameObject);
    }
}
