using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sAnchor : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    public bool anchorDown;

    private Rigidbody boatRigidbody;

    public void OnInteract()
    {
        if (anchorDown == true)
        {
            boatRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX; 
            boatRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            HudMessage = "Press E to Drop Anchor";
            anchorDown = false;
            isInteractable = true;
            return;
        }
        if (anchorDown == false)
        {
            boatRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            HudMessage = "Press E to Raise Anchor";
            anchorDown = true;
            isInteractable = true;
            return;
        }

    }

    public void OnLookingAt()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        boatRigidbody = GetComponentInParent<Rigidbody>();
        boatRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        anchorDown = true;
        isInteractable = true;
        HudMessage = "Press E to Raise Anchor";
    }
}
