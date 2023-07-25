using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CanvasHUD : MonoBehaviour
{
    //References
    GameManager gm;
    PlayerMovement movement;
    public Tutorials tutorialHUD;
    public PlayerMovement playerRef;
    public PlayerSwimming playerSwimming;
    public WidgetPause pauseWidget;
    public HUDFunctions hudFunctions;
    //HUD Components
    public GameObject inBoat;
    public GameObject inPlayer;
    public TextMeshProUGUI promptText;
    public Image promptButton;
    public Sprite[] promptButtonRefs;
    //Flare Counter
    private int flareCount;
    private int shipFlareTime;
    public TextMeshProUGUI tFlareCount;
    public TextMeshProUGUI tShipFlare;
    public GameObject shipFlarePrompt;
    //Meter UI
    public Slider playerHP;
    public Slider playerAir;
    public GameObject airMeter;
    public GameObject hpMeter;
    public bool showMeters;
    public Image iPanelBG;
    public float startingAlpha;
    //Mission UI <-- Unused??
    public GameObject panelHint;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI taskText;
    public bool useBackground;
    //Pause UI
    private int curSceneIdx;
    public GameObject pauseMenu;
    public bool pauseOpen;
    //Letter Scene Prompt
    public RectTransform prompt;
    Vector3 letterPrompt = new Vector3(0, -150f, 0);
    //Quest HUD
    public GameObject questHUD;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questText;
    //DPAD UI
    public GameObject dPadIcon;
    public CanvasGroup dPadAlpha;
    public GameObject[] iconsToShow;
    [SerializeField] Image[] dpadIconSprites;
    [SerializeField] Color iconColorSelected;

    private void Awake()
    {

        dpadIconSprites = new Image[4];

        for (int i = 0; i < iconsToShow.Length; i++)
        {
            dpadIconSprites[i] = iconsToShow[i].GetComponentInChildren<Image>();
        }

    }

    private void Start()
    {
        gm = GameManager.gm;
        curSceneIdx = (int)GameManager.gm.curScene;
        iPanelBG = panelHint.GetComponent<Image>();
        movement = gm.playerReference.GetComponent<PlayerMovement>();
        tutorialHUD = GetComponentInChildren<Tutorials>();
        pauseWidget = GetComponentInChildren<WidgetPause>();
        playerRef = GameManager.gm.playerReference.GetComponent<PlayerMovement>();
        playerSwimming = GameManager.gm.playerReference.GetComponent<PlayerSwimming>();
        hudFunctions = gameObject.GetComponent<HUDFunctions>();
        startingAlpha = iPanelBG.color.a;
        panelHint.SetActive(false);
        if ((int)gm.curScene == 1)
        {
            prompt.anchoredPosition = letterPrompt;
            dPadIcon.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateHealth();
        //line about updating # of flares in UI
        if (Gamepad.all.Count == 0)
        {
            Debug.Log("Please plug in controller!");
        } else 
        if ((Input.GetKeyDown(KeyCode.Q) || Gamepad.current.startButton.wasPressedThisFrame) && !pauseOpen && curSceneIdx != 1)
        {
            hudFunctions.OpenPauseMenu();
        }
        else if ((Input.GetKeyDown(KeyCode.Q) || Gamepad.current.startButton.wasPressedThisFrame) && pauseOpen)
        {
            hudFunctions.ClosePauseMenu();
        }
    }

    public void DisplayDpadPrompt(int _iconToShow)
    {
        iconsToShow[_iconToShow].SetActive(true);
    }

    public void UpdateDpadUI(int _toolIndex)
    {
        if (_toolIndex < 4)
        {
            dpadIconSprites[_toolIndex].color = iconColorSelected;
            for (int i = 0; i < dpadIconSprites.Length; i++)
            {
                if (i != _toolIndex)
                {
                    dpadIconSprites[i].color = Color.white;
                }
            }
        }
        else if (_toolIndex == 4)
        {
            for (int i = 0; i < dpadIconSprites.Length; i++)
            {
                dpadIconSprites[i].color = Color.white;
            }
        }
        else
        {
            return;
        }
    }

    public void UpdateHealth() //obviously this doesnt fix the thing nate told me to fix
    {
        if (movement.currentOxygen != movement.startingOxygen)
        {
            tutorialHUD.ShowMeters();
        }
        else if (movement.currentHealth == movement.startingHealth)
        {
            tutorialHUD.HideMeters();
        }
        playerHP.value = playerRef.currentHealth;
        playerAir.value = playerRef.currentOxygen;
    }

    public void UpdateQuestHUD(string _questTitle,string _questText,int _curStep,int _maxStep)
    {
        questTitle.text = _questTitle;
        questText.text = _questText + "(" + _curStep + "/" + _maxStep + ")"; 
    }
}
