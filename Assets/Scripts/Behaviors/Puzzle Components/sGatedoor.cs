using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sGatedoor : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    public void OnInteract()
    {
        Destroy(this.gameObject);
    }
    public void OnLookingAt() 
    {
    
    }
    void Start()
    {
        isInteractable = true;
        HudMessage = "Press Interact to Open Gate";
    }
}
