using UnityEngine;

public class BoatInteraction : MonoBehaviour, IInteractable
{
    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }

    private ControllerManager controllerManager;
    private GameManager gm;
    private SailboatBehavior boatControllerRef;
    private InputManager inputManager;


    private void Start()
    {
        gm = GameManager.gm;
        HudMessage = "Interact with Boat";
        isInteractable = true;
        if (gm.curScene == GameManager.eScene.CTS_FrontEndScene) inputManager = null; 
        else inputManager = GameManager.gm.gameObject.GetComponent<InputManager>();
        boatControllerRef = gm.boatReference.GetComponent<SailboatBehavior>();
        controllerManager = gm.boatReference.GetComponent<ControllerManager>();
    }

    private void Update()
    {
        if(boatControllerRef.enabled == true)
        {
            isInteractable = false;
        }
        else
        {
            isInteractable = true;
        }
    }
    public void OnLookingAt()
    {

    }
    public void OnInteract()
    {
        controllerManager.ToggleControlsBetweenBoatAndPlayer();
        inputManager.SailingActionsToggle();
    }
}
