using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionBehavior : MonoBehaviour, IInteractable
{
    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }

    public float interactionScale = 1;
    [SerializeField] private bool applyOffset = false;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float inspectRotationSpeed = 1;
    [SerializeField] private bool destroyAfterInspect = false;
    private bool isInspecting;
    private Vector3 startingPosition, startingRotation, startingScale;
    private PlayerMovement playerMovementRef;
    private Collider m_Coll;
    

    private void Start()
    {
        m_Coll = gameObject.GetComponent<Collider>();
        playerMovementRef = GameManager.gm.playerReference.GetComponent<PlayerMovement>();
        isInteractable = true;
        HudMessage = "Inspect";
        if(!destroyAfterInspect)
        {
            startingPosition = transform.position;
            startingRotation = transform.rotation.eulerAngles;
            startingScale = transform.localScale;
        }
    }

    public void OnLookingAt()
    {
        if (isInspecting) RotateWhileInspecting();
        if (GameManager.gm.playerReference.GetComponent<InputManager>().onFoot.Interact.triggered) OnInteractEnd();
    }
    public void OnInteract()
    {
        isInteractable = false;
        if (applyOffset)
        {
            transform.position = playerMovementRef.inspectAnchor.position + offset;
        }else
        {
            transform.position = playerMovementRef.inspectAnchor.position;
        }
        transform.localScale = new Vector3(interactionScale, interactionScale, interactionScale);
        isInspecting = true;
        playerMovementRef.movementEnabled = false;
    }
    public void OnInteractEnd()
    {
        isInspecting = false;
        isInteractable = true;
        playerMovementRef.movementEnabled = true;
        if (!destroyAfterInspect)
        {
            transform.position = startingPosition;
            transform.rotation = Quaternion.Euler(startingRotation);
            transform.localScale = startingScale;
        }
        else if (destroyAfterInspect)
        {
            Destroy(gameObject);
        }
    }
    public void RotateWhileInspecting()
    {
        Vector3 directionToRotate = new Vector3(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"), 0);
        transform.Rotate(directionToRotate * inspectRotationSpeed * Time.deltaTime, Space.World);
    }

    /*
         public void OnInteract()
    {
        isInteractable = false;
        if (pivotFix)
        {
            transform.position = playerMovementRef.inspectAnchor.position - new Vector3(0, m_Coll.bounds.center.y - playerMovementRef.inspectAnchor.position.y, 0);
        }else
        {
            transform.position = playerMovementRef.inspectAnchor.position;
        }
        transform.localScale = new Vector3(interactionScale, interactionScale, interactionScale);
        isInspecting = true;
        playerMovementRef.movementEnabled = false;
    }


    */
}
