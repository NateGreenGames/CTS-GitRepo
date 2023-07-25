using System.IO;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CanvasFE : MonoBehaviour
{
    GameManager gm;

    public Widget_NewGame newGameWidget;
    public FeFunctions feFunctions;
    public GameObject spwnSettings;
    private bool menuSpawned = false;
    public bool hasSaveFile = false;
    public AudioSource musicSource;
    public TextMeshProUGUI tStartGame;
    public Button continueBtn;
    public Button[] mainButtons;
    public GameObject grpMenu;
    public VerticalLayoutGroup grpMenuLayout;
    public GameObject[] menusToSpawn;
    public GameObject newGameBtn;
    public GameObject settingsMenu;
    public GameObject videoSettingsBtn;
    public SaveSettings saveSettings;


    private void Start()
    {
        gm = GameManager.gm;
        feFunctions = GetComponent<FeFunctions>();
        Cursor.visible = false;

        if (gm.playerProgressionUnlock.haveSavedFile)
        {
            continueBtn.interactable = true;
        }
        else
        {
            continueBtn.interactable = false;
        }
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        newGameWidget = GetComponentInChildren<Widget_NewGame>();
        grpMenu.SetActive(false);

        Debug.Log(Cursor.lockState);
        saveSettings = GameManager.gm.GetComponent<SaveSettings>();

    }

    private void Update()
    {
        if (Input.anyKeyDown && !menuSpawned)
        {
            ActivateMainMenu();
        }
    }

    public void ActivateMainMenu()
    {
        menuSpawned = true; //remove update condition        
        tStartGame.alpha = 0; //hide start game text
        feFunctions.WiggleButtonPanel();
        grpMenu.SetActive(true);
        gm.eventSys.SetSelectedGameObject(newGameBtn);
    }

    public void LoadContinueGame(int scene)
    {
        if(scene == 1)
        {
            StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_Letter", 1.5f, 3f));
        }
        else if (scene == 2)
        {
            StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_CabinScene", 1.5f, 1.5f));
        }else if (scene == 3)
        {
            StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_WorldScene", 1.5f, 1.5f));
        }
    }

    public int savedSceneChecker()
    {
        // Needs to have more conditions based on how many scenes we have
        if (GameManager.gm.playerProgressionUnlock.newGame == true)
        {
            return 1;
        }
        else if (GameManager.gm.playerProgressionUnlock.checkpoint_Letter == true)
        {
            return 2;
        }
        else return 3;
    }

    public void OnYesPressed()
    {
        //Something something reset save file
        LoadNewGame();
        //Load into Cabin Scene
        StartCoroutine(GameManager.gm.fadeManager.ChangeScenes("CTS_Letter", 1.5f, 3f));
    }

    private void LoadNewGame()
    {
        saveSettings.Delete(saveSettings.player);
        saveSettings.NewGame();
    }
}
