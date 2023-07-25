using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;


public class Tutorials : MonoBehaviour
{
    CanvasHUD canvasHUD;
    FeFunctions feFunctions;
    GameManager gm;
    PlayerMovement movement;
    ControllerManager controllerMan;
    ItemSwapping itemSwapping;
    private InputManager inputManager;
    //Item Discovery UI
    [SerializeField] GameObject[] tutorialWindows;
    bool windowIsActive;
    int activeWindow;
    //UI that is Hidden between Player and Boat controls
    [SerializeField] GameObject flareAmmoUI;
    [SerializeField] TextMeshProUGUI tFlareCount;
    [SerializeField] TextMeshProUGUI tFlareLoadCount;
    [SerializeField] GameObject shipFlareUI;
    public GameObject shipFlarePrompt;
    [SerializeField] GameObject questTrackerUI;
    [SerializeField] GameObject healthBarsUI;
    [SerializeField] GameObject crosshairUI;
    //General stuff
    public CanvasGroup groupCanvas;
    float alphaOne = 1;
    float alphaTwo = 0;

    private void Start()
    {
        gm = GameManager.gm;
        movement = gm.playerReference.GetComponent<PlayerMovement>();
        controllerMan = gm.boatReference.GetComponent<ControllerManager>();
        itemSwapping = gm.playerReference.GetComponentInChildren<ItemSwapping>();
        inputManager = gm.gameObject.GetComponent<InputManager>();
        for (int i = 0; i < tutorialWindows.Length; i++)
        {
            tutorialWindows[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (windowIsActive && Gamepad.current.bButton.wasPressedThisFrame)
        {
            ResetTutorials();
        }

        if (itemSwapping.selectedTool == 2) //if flare gun is held, show ammo UI
        {
            flareAmmoUI.SetActive(true);
        }
        else
        {
            flareAmmoUI.SetActive(false);
        }

        if (controllerMan.isControllingBoat)
        {
            ActivateBoatUI();
        }
        else if (!controllerMan.isControllingBoat)
        {
            ActivatePlayerUI();
        }

        UpdateFlares();
    }

    public void DiscoverItemUI(int _itemFound) //activate window from tutorialWindows[]
    {
        movement.movementEnabled = false;
        activeWindow = _itemFound;
        tutorialWindows[_itemFound].SetActive(true);
        windowIsActive = true;
    }

    public void ResetTutorials() //Hide any active item widgets
    {
        for (int i = 0; i < tutorialWindows.Length; i++)
        {
            tutorialWindows[i].SetActive(false);
        }

        if (!movement.movementEnabled) movement.movementEnabled = true;
    }

    public void ActivateBoatUI() //if player is in a boat, these elements are active
    {
        crosshairUI.SetActive(false);
        flareAmmoUI.SetActive(false);
        healthBarsUI.SetActive(false);

        shipFlareUI.SetActive(true);
    }

    public void ActivatePlayerUI()//if player is in not in a boat, these elements are active
    {
        crosshairUI.SetActive(true);
        healthBarsUI.SetActive(true);

        shipFlareUI.SetActive(false);
    }

    public void UpdateFlares() //update how many flares player can shoot
    {
        if (gm.playerProgressionUnlock.flareGunLoaded)
        {
            tFlareLoadCount.text = "1";
        }
        else
        {
            tFlareLoadCount.text = "0";
        }
        tFlareCount.text = "" + gm.playerProgressionUnlock.curFlares;
    }

    public void ShowMeters() //display health and o2 bars
    {
        groupCanvas.alpha = Mathf.Lerp(alphaTwo,alphaOne, 8f);
    }

    public void HideMeters() //hide health and o2 bars
    {
        groupCanvas.alpha = Mathf.Lerp(alphaOne, alphaTwo, 8f);
    }
}
