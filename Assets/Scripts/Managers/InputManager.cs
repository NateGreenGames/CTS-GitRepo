using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.SailingActions sailing;
    public ControllerManager controllerManager;
    public SailboatBehavior sailboatBehavior;
    public PlayerMovement movement;
    public PlayerSwimming playerSwimming;
    public ItemSwapping itemSwapping;
    public FlashlightBehavior flashlightBehavior;
    public FlaregunBehavior flaregunBehavior;

    public GameManager gm;
    private bool letterSkipped;
    private bool onUnequip;

    // Start is called before the first frame update

    private void Awake()
    {
        if (playerInput == null) playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        sailing = playerInput.Sailing;
    }
    private void OnEnable()
    {
        onFoot.Enable();
        sailing.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
        sailing.Disable();
    }
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gm = GameManager.gm;
        controllerManager = gm.boatReference.GetComponent<ControllerManager>();
        sailboatBehavior = gm.boatReference.GetComponent<SailboatBehavior>();

        if (gm.curScene == GameManager.eScene.CTS_FrontEndScene)
        {
            itemSwapping = null;
            flaregunBehavior = null;
            flashlightBehavior = null;
            playerSwimming = null;
            movement = null;
        }
        else
        {
            flashlightBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlashlightBehavior>(true);
            flaregunBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlaregunBehavior>(true);
            playerSwimming = gm.playerReference.GetComponent<PlayerSwimming>();
            movement = gm.playerReference.GetComponent<PlayerMovement>();

            OnFootActionsToggle();

            // onFoot Action Maps Inputs
            #region OnFoot Action Callbacks

            onFoot.Jump.performed += ctx => movement.Jump();
            onFoot.SwitchActionMaps.performed += ctx => SwitchActionMaps();
            onFoot.Interact.performed += ctx => InitiateLetter();
            onFoot.Astrolabe.performed += ctx => UpDPad();
            onFoot.Flashlight.performed += ctx => LeftDPad();
            onFoot.Flaregun.performed += ctx => DownDPad();
            onFoot.LightningBottle.performed += ctx => RightDPad();
            onFoot.OperateTool.performed += ctx => OperateTools();
            onFoot.Reload.performed += ctx => Reload();
            #endregion
            // Sailing Action Maps Inputs
            #region Sailing Action Callbacks
            sailing.ToPlayerControls.performed += ctx => ToPlayerControls();
            sailing.BoomRotateLeft.performed += ctx => sailboatBehavior.RotateSailLeft();
            sailing.BoomRotateLeft.canceled += ctx => sailboatBehavior.ResetSailRotation();
            sailing.BoomRotateRight.performed += ctx => sailboatBehavior.RotateSailRight();
            sailing.BoomRotateRight.canceled += ctx => sailboatBehavior.ResetSailRotation();
            sailing.IncreaseSail.performed += ctx => sailboatBehavior.IncreaseSail();
            sailing.IncreaseSail.canceled += ctx => sailboatBehavior.ResetSail();
            sailing.DecreaseSail.performed += ctx => sailboatBehavior.DecreaseSail();
            sailing.DecreaseSail.canceled += ctx => sailboatBehavior.ResetSail();
            sailing.FireAreaFlare.performed += ctx => sailboatBehavior.FireFlare();
            sailing.ThirdToFirst.performed += ctx => sailboatBehavior.SwitchCams();

            #endregion
            // onFoot.Crouch.performed += ctx => movement.Crouch();
            // onFoot.Sprint.performed += ctx => movement.Sprint();
        }
    }

    private void Update()
    {
       //  onUnequip = movement.arm_Anim.GetCurrentAnimatorStateInfo(0).IsName("onUnequip");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Tell the playermovement to move using the value from our movement action
        if (gm.curScene != GameManager.eScene.CTS_FrontEndScene)
        {
            if (playerSwimming.characterController.enabled == true)
            // Debug.Log($"{movement.movementEnabled}");
            if (movement.movementEnabled) movement.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        if (gm.curScene != GameManager.eScene.CTS_FrontEndScene)
        {
            if (movement.movementEnabled) movement.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }
    }
    public void OnFootActionsToggle()
    {
        onFoot.Enable();
        sailing.Disable();

        // Debug.Log($"Onfoot control is : {onFoot.enabled}");
        // Debug.Log($"Sailing control is : {sailing.enabled}");
    }

    public void SailingActionsToggle()
    {
        onFoot.Disable();
        sailing.Enable();
        // Debug.Log($"Onfoot control is : {onFoot.enabled}");
        // Debug.Log($"Sailing control is : {sailing.enabled}");
    }

    public void SwitchActionMaps()
    {
        Debug.Log($" {Keyboard.current} Pressed");
        if (onFoot.enabled)
        {
            Debug.Log($"onFoot is {onFoot.enabled}");
            SailingActionsToggle();
        }

    }
    public void ToPlayerControls()
    {
        if (sailing.enabled && gm.boatReference.GetComponent<SailboatBehavior>().enabled == true)
        {
            // Debug.Log("To player controls from boat");
            OnFootActionsToggle();
            controllerManager.ToggleBoatPlayerCams();
            GameManager.gm.playerReference.GetComponent<sShadowHider>().ShadowsOnly();
            StartCoroutine(sailboatBehavior.FurlSails());
            if (itemSwapping.selectedTool == (int)eToolRtHand.emptyHand) itemSwapping.HandleAnimation((int)eToolRtHand.emptyHand);
            if (itemSwapping.selectedTool == (int)eToolRtHand.astrolabe)
            {
                itemSwapping.HandleAnimation((int)eToolRtHand.astrolabe);
                itemSwapping.AstrolabeSetActiveTrue();

            }
        }
    }

    public void OperateTools()
    {
        if (gm.curScene != GameManager.eScene.CTS_FrontEndScene || gm.curScene != GameManager.eScene.CTS_Letter)
        {
            if (flashlightBehavior == null || flaregunBehavior == null || itemSwapping == null)
            {
                flashlightBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlashlightBehavior>(true);
                flaregunBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlaregunBehavior>(true);
                itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();
            }
            // element 0 = astrolabe, element 1 = flashlight, element 2 = flaregun, element 3 = lightningbottle
            if (gm.playerProgressionUnlock.isFlashlightUnlocked && itemSwapping.selectedTool == 1 && !playerSwimming.isSwimming)
            {
                if (movement.arm_Anim.GetBool("isFlashlightActive") == false) //if our flashlight is off & we hit RT... AKA turn on the flashlight
                {
                    movement.arm_Anim.SetTrigger("onFlashlightToggle"); //play da animation for OnFlashlightToggle
                    flashlightBehavior.ToggleFlashlight(); //control lightsource!

                }
                else //AKA if our flashlight is ON & we hit RT... turn the flashlight OFF
                {
                    movement.arm_Anim.SetTrigger("onFlashlightToggle"); //play the animation for OnFlashlightToggle
                    movement.arm_Anim.SetBool("isFlashlightActive", false); //now the flashlight is NOT active
                    flashlightBehavior.ToggleFlashlight(); //actually control lightsource
                }
            }
            // If the flaregun is out, it will fire a flare and trigger the animation and not swimming
            if (gm.playerProgressionUnlock.isFlareGunUnlocked && itemSwapping.selectedTool == 2 && !playerSwimming.isSwimming)
            {
                // If the flaregun is loaded, fire the gun and play animation
                if (gm.playerProgressionUnlock.flareGunLoaded)
                {
                    gm.audioManager.FlareSFX(0);
                    movement.arm_Anim.SetTrigger("onFire");
                    flaregunBehavior.FlareGunOperation();
                }
            }
        }
    }

    public void Reload()
    {
        // Checks if the script behaviors are null
        if (flaregunBehavior == null || itemSwapping == null)
        {
            flaregunBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlaregunBehavior>(true);
            itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();
        }
        // Check if the flaregun is unlocked and is not loaded, then plays the reload animations and timed to give you the ability to shoot at the end of the animation
        if (itemSwapping.selectedTool == 2 && GameManager.gm.playerProgressionUnlock.isFlareGunUnlocked && !GameManager.gm.playerProgressionUnlock.flareGunLoaded && !flaregunBehavior.isReloading && gm.playerProgressionUnlock.curFlares > 0)
        {
            movement.arm_Anim.SetTrigger("onReload");
            flaregunBehavior.Reload();
        }
    }

    public void Testing()
    {
        // Placeholder until we need camera switching while sailing
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("F3 Pressed");
            flashlightBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlashlightBehavior>(true);
            flaregunBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlaregunBehavior>(true);
            itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();

        }
    }

    public void InitiateLetter()
    {
        if (gm.curScene == GameManager.eScene.CTS_Letter && !letterSkipped)
        {
            DadLetter dadLetter = GameObject.Find("CTS_L_Page002").GetComponent<DadLetter>();
            dadLetter.OnInteract();
            letterSkipped = true;
        }
    }
    /*
    private void SwitchTool()
    {
        // Debug.Log("switching tools");
        if (itemSwapping == null)
        {
                itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();
        }
        else
        if (!onUnequip)
        {
            itemSwapping.SwitchTool();
        }
    }
    */

    private void UpDPad()
    {
        // If current tool is not the flashlight, switch to the flashlight
        if (itemSwapping.selectedTool != 1) itemSwapping.SwitchFlashlight();
        // If player pressed the current equipped tool, it will unequip it
        else if (itemSwapping.selectedTool == 1) itemSwapping.SwitchEmptyHand();
    }

    private void LeftDPad()
    {
        // If current tool is not the astrolabe, switch to the astrolabe
        if (itemSwapping.selectedTool != 0 && !playerSwimming.isSwimming) itemSwapping.SwitchAstrolabe();
        // If player pressed the current equipped tool, it will unequip it
        else if (itemSwapping.selectedTool == 0) itemSwapping.SwitchEmptyHand();
    }

    private void DownDPad()
    {
        // If current tool is not the flashlight, switch to the flashlight
        if (itemSwapping.selectedTool != 3) itemSwapping.SwitchLightningBottle();
        // If player pressed the current equipped tool, it will unequip it
        else if (itemSwapping.selectedTool == 3) itemSwapping.SwitchEmptyHand();
    }

    private void RightDPad()
    {
        // If current tool is not the flashlight, switch to the flashlight
        if (itemSwapping.selectedTool != 2) itemSwapping.SwitchFlaregun();
        // If player pressed the current equipped tool, it will unequip it
        else if (itemSwapping.selectedTool == 2) itemSwapping.SwitchEmptyHand();
    }
}
