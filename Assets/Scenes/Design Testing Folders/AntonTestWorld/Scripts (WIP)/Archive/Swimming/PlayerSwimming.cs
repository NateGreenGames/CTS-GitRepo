using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerSwimming : MonoBehaviour
{
    [Tooltip("Movement will follow the camera in this slot while swimming")]
    public Transform mainCamera;

    [Header("KeyBinds")]
    public KeyCode swimUp = KeyCode.Z;
    public KeyCode swimDown = KeyCode.C;
    public KeyCode swimSprint = KeyCode.V;

    [Header("Swim Speed")]
    [Tooltip("This is the speed the player will be swimming forward at")]
    public float moveSpeed;
    public float moveSpeedUp;
    public float moveSpeedDown;
    public float sprintMultiplier = 1.5f;
    public float waterDrag;
    public bool isSwimming;
    public bool isDrowning;

    private float baseSwimSpeed = 3;
    private bool isSwimSprint;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    [Header("Sounds")]
    [Tooltip("Sounds that will play when entering water")]
    public AudioClip EnterWater;

    [Tooltip("Sounds that will play when exiting water")]
    public AudioClip ExitWater;

    [Tooltip("Sounds that will play when swimming underwater")]
    public AudioClip SwimmingUnderWater;

    [Header("Effects")]
    public Color WaterFogColor;
    public float FogDensity = 0.25f;
    // public PostProcessProfile UnderwaterPostProcessing;

    // For Post-processing while under water
    private bool defFogEnabled;
    private Color defFogColor;
    private FogMode defFogMode;
    private float defFogDensity;

    // For breathing timer ticks
    private float timeValue = 3;
    private float tick = 1;

    public CharacterController characterController; // Does this need to be public? if so move it for refactoring
    public Rigidbody rigidBody;
    private PlayerMovement playerMovement;
    private CapsuleCollider capsuleCollider;
    private AudioSource swimmingAudio;
    private ItemSwapping itemSwapping;
    [SerializeField] private Transform Hand_WalkingPos;
    [SerializeField] private Transform Hand_SwimmingPos;

    void Start()
    {
        // Grabs the controller from the player
        playerMovement = GetComponent<PlayerMovement>();
        swimmingAudio = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        // Disables the rigid body while the character controller is active in the beginning
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        rigidBody.useGravity = false;
        capsuleCollider.enabled = false;
        isSwimSprint = false;
        itemSwapping = GameManager.gm.playerReference.GetComponentInChildren<ItemSwapping>();

        /*
        // Sets the Fog 
        defFogEnabled = RenderSettings.fog;
        defFogColor = RenderSettings.fogColor;
        defFogMode = RenderSettings.fogMode;
        defFogDensity = RenderSettings.fogDensity;
        */

         moveSpeed = 3;
         moveSpeedUp = 7;
         moveSpeedDown = 7;
         sprintMultiplier = 1.5f;
}

    private void Update()
    {
        PlayerInputs();
        if (isSwimming == true && !GameManager.gm.boatReference.GetComponent<ControllerManager>().isControllingBoat)
        {
            MovePlayer();
            rigidBody.drag = waterDrag;
            SpeedControl();

            if (horizontalInput > 0 || verticalInput > 0) playerMovement.arm_Anim.SetBool("isSwimming", true);
            else playerMovement.arm_Anim.SetBool("isSwimming", false);

            if (Input.GetKey(swimUp))
            {
                SwimUp();
            }
            if (Input.GetKey(swimDown))
            {
                SwimDown();
            }
        }
        if (isSwimming == true && isDrowning == true) 
        {
            Breathing();
        }
        if (isSwimming == false && isDrowning == false)
        {
            OxygenRegen();
        }
        if (Input.GetKeyDown(swimSprint) && isSwimming == true) 
        {
            SwimSprint();
        }
        if (!swimmingAudio.isPlaying && isSwimming)
        {
            swimmingAudio.clip = SwimmingUnderWater;
            swimmingAudio.loop = true;
            swimmingAudio.Play();
        }
    }

    public void IsBelowWater()
    {
        isSwimming = true;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
         Debug.Log($"Player is below water {isSwimming}");
        characterController.enabled = false;
        capsuleCollider.enabled = true;
        isDrowning = true;
        EnterWaterAudio();
        playerMovement.arm_Anim.SetBool("inWater", true);
        playerMovement.arm_Anim.SetTrigger("onUnequip");
        playerMovement.hesseHandsModel.transform.position = Hand_SwimmingPos.transform.position;
        playerMovement.hesseHandsModel.transform.rotation = Hand_SwimmingPos.transform.rotation;
        itemSwapping.AstrolabeSetActiveFalse();
        if (itemSwapping.selectedTool == (int)eToolRtHand.emptyHand) itemSwapping.SwitchEmptyHand();
        if (itemSwapping.selectedTool == (int)eToolRtHand.lightningBottle)
        {
            itemSwapping.LightningBottleEquipped();
            itemSwapping.LightningBottleActiveTrue();
        }
        else itemSwapping.LightningBottleActiveFalse();




        // BelowWaterPostProc();
    }

    public void IsAboveWater()
    {
        isSwimming = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
         Debug.Log($"Player is below water {isSwimming}");
        characterController.enabled = true;
        playerMovement.arm_Anim.SetBool("inWater", false);
        playerMovement.hesseHandsModel.transform.position = Hand_WalkingPos.transform.position;
        playerMovement.hesseHandsModel.transform.rotation = Hand_WalkingPos.transform.rotation;
        /*
        if (GameManager.gm.curScene != GameManager.eScene.CTS_Letter)
        {
            if (itemSwapping.selectedTool == (int)eToolRtHand.astrolabe) itemSwapping.AstrolabeSetActiveTrue();
            if (itemSwapping.selectedTool == (int)eToolRtHand.lightningBottle) itemSwapping.LightningBottleActiveTrue();
        }
        */
        itemSwapping.AstrolabeSetActiveTrue();
        playerMovement.playerVelocity.y = Mathf.Sqrt(playerMovement.jumpHeight * -0.15f * playerMovement.gravity);
        capsuleCollider.enabled = false;
        isDrowning = false;
        timeValue = 3;
        // AboveWaterPostProc();
        
        if (swimmingAudio.isPlaying)
        {
            ExitWaterAudio();
        }
    }

    public void PlayerInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculates movement direction
        moveDirection = mainCamera.forward * verticalInput + mainCamera.right * horizontalInput;
        rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        // Limits Velocity if needed 
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

    private void SwimUp()
    {
        moveDirection = mainCamera.forward * verticalInput + mainCamera.right * horizontalInput;
        rigidBody.AddForce(transform.up * moveSpeedUp, ForceMode.Force);
    }
    
    private void SwimDown()
    {
        moveDirection = mainCamera.forward * verticalInput + mainCamera.right * horizontalInput;
        rigidBody.AddForce(-transform.up * moveSpeedDown, ForceMode.Force);
    }
    
    public void Drowning()
    {
        // Runs when oxygen is at 0
        playerMovement.currentHealth -= 10;
        if (gameObject.tag == "Player" && playerMovement.currentHealth <= 0)
        {
            gameObject.GetComponent<PlayerMovement>().onDeath();
            playerMovement.currentOxygen = playerMovement.startingOxygen;
            playerMovement.currentHealth = playerMovement.startingHealth;
        }
    }

    public void Breathing()
    {
        // Oxygen bar goes down ever tick
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            if (playerMovement.currentOxygen > 0)
            {
                playerMovement.currentOxygen -= 10;
            }
            if (playerMovement.currentOxygen == 0)
            {
                Drowning();
            }
            timeValue += 3;
        }
    }

    public void OxygenRegen()
    {
        // Oxygen bar regenerates every second while above water
        if (tick > 0)
        {
            tick -= Time.deltaTime;
        }
        else
        {
            if (playerMovement.currentOxygen != 100)
            {
                playerMovement.currentOxygen += 10;
            }
            tick += 1;
        }
    }

    public void SwimSprint()
    {
        // Toggles faster swimming 
        isSwimSprint = !isSwimSprint;
        if (isSwimSprint) moveSpeed = baseSwimSpeed * sprintMultiplier;
        else moveSpeed = baseSwimSpeed;
    }

    public void AboveWaterPostProc() // Activates post-processing effects upon going underwater
    {
        RenderSettings.fog = defFogEnabled;
        RenderSettings.fogColor = defFogColor;
        RenderSettings.fogMode = defFogMode;
        RenderSettings.fogDensity = defFogDensity;
    }

    public void BelowWaterPostProc() // Resets post-processing effects upon leaving the water
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = WaterFogColor;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = FogDensity;
    }

    public void EnterWaterAudio()
    {
        if (EnterWater != null && isSwimming)
        {
            swimmingAudio.clip = EnterWater;
            //swimmingAudio.Play();
        }
    }

    public void ExitWaterAudio()
    {
        if (swimmingAudio.isPlaying)
        {
            //swimmingAudio.Stop();
        }
        if (ExitWater != null && swimmingAudio)
        {
            swimmingAudio.clip = ExitWater;
            swimmingAudio.loop = false;
            //swimmingAudio.Play();
        }
    }

    public void AddRBAgain()
    {
        if (rigidBody == null)
        {
            rigidBody = gameObject.GetComponent<Rigidbody>();
        }
        
    }
}
