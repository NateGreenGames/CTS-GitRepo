using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDestructable
{
    [Header("General Requirements:")]
    public int startingHealth = 100;
    public int currentHealth;
    public int startingOxygen = 100;
    public int currentOxygen;
    public Transform playerCam;
    public Animator m_Anim; //This is public because the animator is on the mesh, not the root of the player prefab.
    public Animator arm_Anim;
    public GameObject hessePlayerModel;
    public GameObject hesseHandsModel;

    private GameManager gm;
    private InputManager inputManager;
    private CanvasHUD playerUI;
    private SaveSettings saveSettings;

    [Space]
    [Header("Movement Properties:")]
    public bool movementEnabled = true;
    public float speed = 5.0f;
    public float sprintMod = 1.5f;
    public float gravity = -9.8f;
    public float jumpHeight = .5f;
    public float crouchTimer;
    public bool showDebugInformation = false;

    private float baseSpeed;

    [Space]
    [Header("Current Spawn Point:")]
    public Vector3 respawnLocation;
    public Quaternion respawnRotation;

    CharacterController controller;
    PlayerSwimming playerSwimming;
    public Rigidbody rb;
    public Vector3 playerVelocity;
    public bool isSwimming;
    private bool isGrounded;
    private bool lerpCrouch;
    private bool isCrouching;
    private bool isSprinting;
    
    [Space]
    [Header("Interactable Detection:")]
    [SerializeField] float distance = 3f;
    [SerializeField] LayerMask mask;

    [Space]
    [Header("Inspection Properties:")]
    public Transform inspectAnchor;

    [Space]
    [Header("Mouse Properties:")]
    private float xSensitivity = 10f;
    private float ySensitivity = 10f;
    float xRotation = 0f;

    public float sensitivity = 10f;
    private float controllerSensitivity = 4f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        currentOxygen = startingOxygen;
        baseSpeed = speed;
        gm = GameManager.gm;
        respawnLocation = transform.position;
        // Debug.Log($"first iteration {respawnLocation}");
        playerUI = GameManager.gm.canvasManager.canvasHUD;
        controller = GetComponent<CharacterController>();
        inputManager = GameManager.gm.GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        playerSwimming = GetComponent<PlayerSwimming>();
        saveSettings = GameManager.gm.GetComponent<SaveSettings>();


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isSwimming = false;
        #region Checks for checkpoints and spawns player at corresponding checkpoint
        if (GameManager.gm.playerProgressionUnlock.newGame == true)
        {
            gameObject.transform.position = GameObject.FindGameObjectWithTag("InitialSpawnPt_Player").transform.position;
            gameObject.transform.rotation = GameObject.FindGameObjectWithTag("InitialSpawnPt_Player").transform.rotation;

        } 
        else
        {
            if (saveSettings.player.respawnLocation.magnitude == 0)
            {
                gameObject.transform.position = GameObject.FindGameObjectWithTag("BoatSpawnPt").transform.position;
                gameObject.transform.rotation = GameObject.FindGameObjectWithTag("BoatSpawnPt").transform.rotation;
            }
            else
            {
                if (GameManager.gm.playerProgressionUnlock.checkPoint_Island == true)
                {
                    gameObject.transform.position = GameObject.FindGameObjectWithTag("BoatSpawnPt").transform.position;
                    gameObject.transform.rotation = GameObject.FindGameObjectWithTag("BoatSpawnPt").transform.rotation;

                }
                else
                {
                    gameObject.transform.position = GameManager.gm.playerProgressionUnlock.respawnLocation;
                    gameObject.transform.rotation = GameManager.gm.playerProgressionUnlock.respawnRotation;
                }
            }
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        OnCrouch();
        WhileLooking();
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Adjust sensitivity based on input type
        float sensitivity = Input.mousePresent ? this.sensitivity : controllerSensitivity;
        mouseX *= sensitivity;
        mouseY *= sensitivity;

        // Calculate camera rotation looking up and down
        xRotation -= mouseY * Time.deltaTime * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        
        // Applies to our camera transform
        playerCam.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate player to look left and right using mouse input
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void WhileLooking()
    {
        playerUI.hudFunctions.UpdatePrompt(string.Empty, 0);
        // Creates a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(showDebugInformation) Debug.DrawRay(ray.origin, ray.direction * distance);
            RaycastHit hitInfo; // Variable to store our collision information
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (showDebugInformation) Debug.Log(hitInfo.collider.name);
            if (hitInfo.collider.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable.isInteractable)
            {
                interactable.OnLookingAt();
                playerUI.hudFunctions.UpdatePrompt(interactable.HudMessage,1);
                 if (inputManager.onFoot.Interact.triggered)
                 {
                     interactable.OnInteract();
                 }
            }
        }
    }

    public void TestingInteractTrigger()
    {
        if (inputManager.onFoot.Interact.triggered)
        {
            Debug.Log("I got triggered!");
        }
    }
    public void OnCrouch()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (isCrouching) controller.height = Mathf.Lerp(controller.height, 1, p);
            else controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    // Receives inputs from our input manager and applies them to the character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        if (moveDirection != Vector3.zero)
        {
            if(isSprinting)
            {
                m_Anim.SetFloat("Speed", 1);
            }
            else //regular walk speed
            {
                m_Anim.SetFloat("Speed", 0.5f);
                arm_Anim.SetBool("isMoving", true);
            }
        }
        else //not moving
        {
          m_Anim.SetFloat("Speed", 0);
          arm_Anim.SetBool("isMoving", false);

        }
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
        if(showDebugInformation)Debug.Log(playerVelocity.y);
    }

    public void Jump() 
    {
        if (gm.curScene != GameManager.eScene.CTS_FrontEndScene || gm.curScene != GameManager.eScene.CTS_Letter)
        {
            if (isGrounded && !isSwimming && movementEnabled)
            {
                // GameManager.gm.audioManager.JumpSFX();
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                m_Anim.SetTrigger("Jump");
            }
        }
    }

    public void Crouch() // Currently Toggle on/off
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint() // Currently Toggle on/off
    {
        isSprinting = !isSprinting;
        if (isSprinting) speed = baseSpeed * sprintMod;
        else speed = baseSpeed;
    }

    public void takeDamage(int _damageTaken)
    {
        currentHealth -= _damageTaken;
        if (currentHealth <= 0) onDeath();
    }
    public void onDeath()
    {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        Debug.Log("Player Death Detected, initiating respawn.");
        //Run death fade out, wait for fade out to be as dark as possible. I want to run this animation on a ui component so I'm gonna wait for this bit until Nick gets the hud instantiating programatically.
        yield return new WaitForSeconds(0); 
        gameObject.transform.position = respawnLocation;
        gameObject.transform.rotation = respawnRotation;
        Debug.Log("RespawnComplete");
    }

    public void RemoveRB()
    {
        Destroy(rb);
    }

    public void AddRB()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        playerSwimming.AddRBAgain();
    }

}
