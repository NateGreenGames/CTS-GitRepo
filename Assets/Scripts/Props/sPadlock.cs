using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPadlock : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    public GameObject lockedObject;
    public int[] lockCombination = new int[3];


    void Start()
    {
        isInteractable = true;
        HudMessage = "Interact";
        
    }

    public void OnInteract()
    {
        /*
        //Spawn Padlock UI
        sWidgetPadlock widgetRef = Instantiate(Resources.Load("Widgets/INGAME_Widgets/Widget_Padlock") as GameObject, GameManager.gm.canvasManager.canvasHUD.transform).GetComponent<sWidgetPadlock>();
        //Pass Needed Information
        widgetRef.Init(lockCombination, this);
       
        //Show/unconstrain mouse cursor and disable player movement.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
         */
        
    }
    public void OnLookingAt()
    {

    }

    public void Solved()
    {
        // lockedObject.GetComponent<IInteractable>().isInteractable = true;
        isInteractable = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void Unlock()
    {
        // lockedObject.GetComponent<IInteractable>().isInteractable = true;
        isInteractable = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Padlock is unlocked");
    }
}
