using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use IInteractable, add a ', IInteractble' after Monobehavior
// Right click on IInteractble and add the interface to populate the string, bool, and function
// For HUDmessage, create a start function and then HUDmessage = " "; where the blank is what is shown when you look at the item
public interface IInteractable
{
    bool isInteractable { get; set; }
    string HudMessage { get; set; }
    void OnInteract();
    void OnLookingAt();
}
