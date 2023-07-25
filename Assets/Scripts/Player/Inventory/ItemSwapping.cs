using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public class ItemSwapping : MonoBehaviour
{
    public PlayerProgressionUnlock pToolUnlocked;

    public Transform toolContainer;
    public Transform rightHandPt;
    public soRep sRep;

    public int toolsUnlocked = 0;
    public int selectedTool = 0;
    public int previousSelectedTool;

    public Flammable flammable;
    public PlayerMovement playerMovement;

    private GameManager gm;
    [SerializeField] private GameObject astrolabe;
    [SerializeField] private GameObject lightningBottle;

    private int spawningTools;
    string pToolUnlockPath;

    private void Start()
    {
        gm = GameManager.gm;
        RightItemSwap(sRep);
        ToolSpawn(sRep);
        playerMovement = gm.playerReference.GetComponent<PlayerMovement>();
        // playerInput = new PlayerInput();
        // onFootActions = playerInput.OnFoot;
        LightningBottleActiveFalse();
    }

    private void Update()
    {
        // Saves the tool unlock states to an external .json file that persists through plays
        // Press F12 to delete the .json file to set all unlock states for tools back to false
        // F12 input located in the ToolsUnlockManager Script
        pToolUnlockPath = $"{Application.persistentDataPath}/pToolUnlock.json";
        if (File.Exists(pToolUnlockPath))
        {
            string json = File.ReadAllText(pToolUnlockPath);
            pToolUnlocked = JsonUtility.FromJson<PlayerProgressionUnlock>(json);
        }
        // SwitchingTools();
    }

    public void RightItemSwap(soRep _soRep)
    {
        int i = 0;
        // Cycles through the tools in the right hand
        foreach (Transform tool in transform)
        {
            // Checks the GameManager to see if tool is unlocked, if not increments to the next unlocked tool
            if (i == selectedTool && gm.isToolUnlocked[i]) 
            {
                if (i == 0 || i == 3)
                {
                    tool.gameObject.SetActive(false);
                }
                else
                {
                    Invoke("AstrolabeSetActiveFalse", 1.25f);
                    Invoke("LightningBottleActiveFalse", .75f);
                    tool.gameObject.SetActive(true);
                }
                _soRep.toolRtHand[i].isEquipped = true;
               // Debug.Log($"Currently equipped tool is {_soRep.toolRtHand[i].sTool}");
            }
            else
            {
                tool.gameObject.SetActive(false);
                _soRep.toolRtHand[i].isEquipped = false;
            } 
            i++;
            if (i > 4) i = 0;
        }
    }
    
    // If the future, see if the equipped tools can be combined
    public bool FlashlightEquipped()
    {
        // Check to see if Lighter is equipped and has been unlocked
        if (sRep.toolRtHand[(int)eToolRtHand.flashlight].isEquipped && GameManager.gm.playerProgressionUnlock.isFlashlightUnlocked == true)
        {
            return true;
        }
        else return false;
    }

    public bool FlareGunEquipped()
    {
        // Check to see if Flare Gun is equipped and has been unlocked
        if (sRep.toolRtHand[(int)eToolRtHand.flareGun].isEquipped && GameManager.gm.playerProgressionUnlock.isFlareGunUnlocked == true)
        {
            return true;
        }
        else return false;
    }

    public bool AstrolabeEquipped()
    {
        // Check to see if Shovel is equipped and has been unlocked
        if (sRep.toolRtHand[(int)eToolRtHand.astrolabe].isEquipped && GameManager.gm.playerProgressionUnlock.isAstrolabeUnlocked == true)
        {
            return true;
        }
        else return false;
    }

    public bool LightningBottleEquipped()
    {
        // Check to see if Shovel is equipped and has been unlocked
        if (sRep.toolRtHand[(int)eToolRtHand.lightningBottle].isEquipped && GameManager.gm.playerProgressionUnlock.isLightningBottleUnlocked == true)
        {
            return true;
        }
        else return false;
    }

    public void ToolSpawn(soRep _soRep)
    {
        
        // Spawns in all tools in the toolRtHand array
        for (int i = 0; i < _soRep.toolRtHand.Length; i++)
        {
            int x = 0, y = 0, z = 0;
            
            if (i == (int)eToolRtHand.flareGun)
            {
                // Sets the rotation of the flaregun to be rotated correctly
                x = 0;
                y = 90;
                z = 90;
            }
            if (i == (int)eToolRtHand.astrolabe)
            {
                x = 0;
                y = -90;
                z = -100;
            }
            if (i == (int)eToolRtHand.flashlight)
            {
                x = 0;
                y = 90;
                z = 90;
               // rightHandPt.transform.position = rightHandPt.transform.position - new Vector3(0, .2f, 0);
            }
            GameObject obj = Instantiate(_soRep.toolRtHand[i].pTool, rightHandPt.transform.position, transform.rotation * Quaternion.Euler(x, y, z));
            obj.transform.parent = toolContainer;
        }
        RightItemSwap(sRep);
    }
   
    public void NextItemSwap()
    {
        if (selectedTool >= transform.childCount - 1) selectedTool = 0;
        else selectedTool++;
    }

    /*
    public void SwitchingTools()
    {
        // Switches between the items in the player's right hand
        previousSelectedTool = selectedTool;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) NextItemSwap();
        if (Input.GetAxis("Mouse ScrollWheel") < 0f )
        {
            if (selectedTool <= 0) selectedTool = transform.childCount - 1;
            else selectedTool--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedTool = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) selectedTool = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) selectedTool = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4) selectedTool = 3;
        if (previousSelectedTool != selectedTool) RightItemSwap(sRep);
    }
    */
    public void SwitchTool()
    {
        previousSelectedTool = selectedTool;
        NextItemSwap();
        if (previousSelectedTool != selectedTool) RightItemSwap(sRep);
        HandleAnimation(selectedTool);
    }

    public void ObtainFlareGun()
    {
        // Gives player flare gun
        gm.canvasManager.canvasHUD.tutorialHUD.DiscoverItemUI(2);
        gm.canvasManager.canvasHUD.DisplayDpadPrompt(2);
        GameManager.gm.isToolUnlocked[2] = true;
        GameManager.gm.playerProgressionUnlock.isFlareGunUnlocked = true;
        selectedTool = (int)eToolRtHand.flareGun;
        RightItemSwap(sRep);
        HandleAnimation(selectedTool);
    }

    public void ObtainFlashlight()
    {
        // Gives player flare gun
        gm.canvasManager.canvasHUD.DisplayDpadPrompt(1);
        GameManager.gm.isToolUnlocked[1] = true;
        GameManager.gm.playerProgressionUnlock.isFlashlightUnlocked = true;
        selectedTool = (int)eToolRtHand.flashlight;
        RightItemSwap(sRep);
        HandleAnimation(selectedTool);
    }

    public void ObtainLightningBottle()
    {
        // Gives player lightning bottle
        gm.canvasManager.canvasHUD.DisplayDpadPrompt(3);
        GameManager.gm.isToolUnlocked[3] = true;
        GameManager.gm.playerProgressionUnlock.isLightningBottleUnlocked = true;
        selectedTool = (int)eToolRtHand.lightningBottle;
        RightItemSwap(sRep);
        HandleAnimation(selectedTool);
    }

    public void HandleAnimation(int _curTool)
    {
        // Debug.Log(playerMovement.arm_Anim.gameObject.name);
        playerMovement.arm_Anim.SetTrigger("onUnequip");
        playerMovement.arm_Anim.SetInteger("curTool", _curTool);
    }

    public void AstrolabeSetActiveTrue()
    {
        astrolabe.SetActive(true);
    }

    public void AstrolabeSetActiveFalse()
    {
        astrolabe.SetActive(false);
    }

    public void LightningBottleActiveTrue()
    {
        lightningBottle.SetActive(true);
    }

    public void LightningBottleActiveFalse()
    {
        lightningBottle.SetActive(false);
    }

    public void SwitchAstrolabe()
    {
        // Switches to Astrolabe on left d pad. 
        if (GameManager.gm.playerProgressionUnlock.isAstrolabeUnlocked == true)
        {
            // if (spawningTools == 0) obj.SetActive(false);
            selectedTool = (int)eToolRtHand.astrolabe;
            RightItemSwap(sRep);
            HandleAnimation(selectedTool);
            Invoke("AstrolabeSetActiveTrue", .75f);
            gm.questManager.GetComponent<QuestManager>().UpdateCoords();
            Invoke("LightningBottleActiveFalse", .75f);
            GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(selectedTool);
        }
    }

    public void SwitchFlashlight()
    {
        if (GameManager.gm.playerProgressionUnlock.isFlashlightUnlocked == true)
        {
            selectedTool = (int)eToolRtHand.flashlight;
            RightItemSwap(sRep);
            HandleAnimation(selectedTool);
            Invoke("LightningBottleActiveFalse", .75f);
            GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(selectedTool);
        }
    }

    public void SwitchFlaregun()
    {
        if (GameManager.gm.playerProgressionUnlock.isFlareGunUnlocked == true)
        {
            selectedTool = (int)eToolRtHand.flareGun;
            RightItemSwap(sRep);
            HandleAnimation(selectedTool);
            Invoke("LightningBottleActiveFalse", .75f);
            GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(selectedTool);
        }
    }

    public void SwitchEmptyHand()
    {
        selectedTool = (int)eToolRtHand.emptyHand;
        RightItemSwap(sRep);
        HandleAnimation(selectedTool);
        Invoke("LightningBottleActiveFalse", .75f);
        GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(selectedTool);
    }

    public void SwitchLightningBottle()
    {
        if (GameManager.gm.playerProgressionUnlock.isLightningBottleUnlocked == true)
        {
            Invoke("LightningBottleActiveTrue", .75f);
            selectedTool = (int)eToolRtHand.lightningBottle;
            RightItemSwap(sRep);
            HandleAnimation(selectedTool);
            Invoke("AstrolabeSetActiveFalse", 1f);
            GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(selectedTool);
            if (gm.playerProgressionUnlock.isLightningBottleCharged) LightningBottleCharged();
            if (!gm.playerProgressionUnlock.isLightningBottleCharged) LightningBottleUncharged();
        }
    }

    public void LightningBottleCharged()
    {
         lightningBottle.GetComponent<LightningBottleBehavior>().lightningParticle.SetActive(true);
    }

    public void LightningBottleUncharged()
    {
         lightningBottle.GetComponent<LightningBottleBehavior>().lightningParticle.SetActive(false);
    }
}

