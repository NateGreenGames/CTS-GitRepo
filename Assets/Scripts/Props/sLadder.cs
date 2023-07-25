using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sLadder : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    public GameObject onLandSpot;

    public void OnInteract()
    {
        GameManager.gm.playerReference.transform.position = onLandSpot.transform.position;
        
    }

    public void OnLookingAt()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        isInteractable = true;
        HudMessage = "Climb Ladder";
    }
}
