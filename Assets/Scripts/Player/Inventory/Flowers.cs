using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Flowers : MonoBehaviour, ICollectable, IInteractable
{
    public static event HandleFlowerCollected OnFlowerCollected;
    public delegate void HandleFlowerCollected(ItemData itemData);
    public ItemData flowerData;

    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }

    private void Start()
    {
        HudMessage = $"Interact to pick up {flowerData}";
        isInteractable = true;
    }

    public void OnLookingAt()
    {
        // Reserved for objects that will be put into the inventory 
        // Will refactor this in the future when inventory is hooked up with visuals
    }

    public void OnInteract()
    {
        OnCollect();
    }
    public void OnCollect()
    {
        OnFlowerCollected?.Invoke(flowerData);
        Destroy(gameObject);
    }
}
