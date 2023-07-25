using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlareGun : MonoBehaviour, ICollectable, IInteractable
{
    public static event HandleFlareGunCollected OnFlareGunCollected;
    public delegate void HandleFlareGunCollected(ItemData itemData);
    public ItemData flareGunData;

    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }

    private void Start()
    {
        HudMessage = "Interact to pick up";
        isInteractable = true;
    }

    public void OnInteract()
    {
        OnCollect();
    }
    public void OnLookingAt()
    {
        // Reserved for objects that will be put into the inventory 
        // Will refactor this in the future when inventory is hooked up with visuals
    }
    public void OnCollect()
    {
        OnFlareGunCollected?.Invoke(flareGunData);
        GameManager.gm.playerProgressionUnlock.flareGunLoaded = true;
        Destroy(gameObject);
    }
}