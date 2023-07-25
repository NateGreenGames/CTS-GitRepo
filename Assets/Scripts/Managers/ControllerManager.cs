using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public Vector3 playerBoatSpawnPt;
    public bool isControllingBoat = false;
    private CanvasHUD playerUI;
    private SailboatBehavior boatController; // Hook up Boat to this (if there are multiple ships, will need to change this to an array)
    private Rigidbody sailboatRB;
    private CharacterController characterController; // Hook up Player character to this
    private PlayerMovement playerMovement;
    private InputManager inputManager;
    private PlayerInput playerInput;
    private GameManager gm;
    private HesseSailingAnimManager hesseSailingAnimManager;

    private BoatInteraction interactionRef;

    void Init()
    {
        gm = GameManager.gm;
        playerUI = gm.canvasManager.canvasHUD;
        boatController = gm.boatReference.GetComponent<SailboatBehavior>();
        characterController = gm.playerReference.GetComponent<CharacterController>();
        playerMovement = gm.playerReference.GetComponent<PlayerMovement>();
        sailboatRB = boatController.GetComponent<Rigidbody>();
        inputManager = gm.playerReference.GetComponent<InputManager>();
        hesseSailingAnimManager = GetComponentInChildren<HesseSailingAnimManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Icon on steering wheel pops up to allow the player to change to steering

        if (Input.GetKeyUp(KeyCode.O))
        {
            ToggleBoatPlayerCams();
        }
       
    }

    public void ToggleControlsBetweenBoatAndPlayer()
    {
        isControllingBoat = !isControllingBoat;
        // Handles if player is sitting down or standing up animation
        hesseSailingAnimManager.gameObject.SetActive(isControllingBoat);
        if (isControllingBoat == true) boatController.thirdPersonCam.SetActive(true);
        hesseSailingAnimManager.HandleHesseAnimation(isControllingBoat);
        playerUI.hudFunctions.ToggleUIModes(isControllingBoat);
        playerMovement.movementEnabled = !playerMovement.movementEnabled;
        boatController.SwitchToThird();

        // Checks if the reference is empty, and then if it is not, enable the controller
        if (characterController == null) gm.playerReference.GetComponent<CharacterController>();
        else characterController.enabled = !characterController.enabled;

        // playerMovement.playerCam.enabled = false;
        // GetComponentInChildren<sCameraSwitch>().CameraSwitch();
        switch (sailboatRB.constraints)
        {
            case RigidbodyConstraints.FreezeAll:
                sailboatRB.constraints = RigidbodyConstraints.FreezeRotation;
                break;

            case RigidbodyConstraints.FreezeRotation:
                sailboatRB.constraints = RigidbodyConstraints.FreezeAll;
                break;

            default:
                Debug.Log("Unexpected Error changing boat Rigidbody Constraints within the boat manager. Please check to make sure all are locked on play.");
                break;
        }

        boatController.enabled = !boatController.enabled;

        playerUI.hudFunctions.UpdatePrompt(string.Empty,0);

        if (playerMovement.rb != null)
        {
            playerMovement.RemoveRB();
        }
        if (playerMovement.rb == null)
        {
            GameManager.gm.playerReference.transform.position = GameObject.FindGameObjectWithTag("BoatSpawnPt").transform.position;  // GameManager.gm.playerReference.transform.position + new Vector3(0, 1, 0);
            playerMovement.AddRB();
            boatController.tillerAngle = 0;
        }

        if (isControllingBoat == false)
        {
            GameManager.gm.playerReference.GetComponent<sShadowHider>().ShadowsAndBody();
        }

        if (isControllingBoat == false)
        {
            playerMovement.movementEnabled = true;
        }
    }

    public void ToggleBoatPlayerCams()
    {
        ToggleControlsBetweenBoatAndPlayer();
        GameObject.FindGameObjectWithTag("BoatCam").SetActive(false);
        // Debug.Log("Toggle Boat Cam");
    }
    
   

    /*
    public void SwitchBoatToPlayer()
    { 
            Debug.Log("X button Pressed");
            ToggleBoatPlayerCams();
            playerInput.Sailing.Disable();
            playerInput.OnFoot.Enable();
    }

    public void SwitchPlayerToBoat()
    {
            ToggleBoatPlayerCams();
            inputManager.playerInput.Sailing.Enable();
            inputManager.playerInput.OnFoot.Disable();
    }
    */
    #region SeperatedControllerManagement
    
    #endregion
}
