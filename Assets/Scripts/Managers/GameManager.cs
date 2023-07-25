using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Enviro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //Hehe testing for anton.
    public enum eScene { CTS_FrontEndScene,CTS_Letter, CTS_CabinScene, CTS_WorldScene, nickPIslandGreyboxTest, NateG_Scene, Note_Test, Anton_TestScene, Anton_WaterTestScene, };

    // the number true/false is the number of tools that can be unlocked
    public bool[] isToolUnlocked = new bool[] { true, false, false, false, false };
    public ItemData[] itemDatas;

    public static GameManager gm;
    public AudioManager audioManager;
    public CanvasManager canvasManager;
    public LoadingManager fadeManager;
    public EnviroManager enviroSystem;
    public CrestManager crestReference;
    public GameObject playerReference, boatReference;
    public WeatherController weatherController;
    public Widget_NewGame widgetNewGame;
    public SaveSettings saveSettings;
    public EventSystem eventSys;
    public InputManager inputManager;
    public PlayerInput playerInput;
    public QuestManager questManager;

    public bool skipLetter;

    [SerializeField] private bool displayCanvasHud;
    [SerializeField] public PlayerProgressionUnlock playerProgressionUnlock;
    private string pToolUnlockPath;
    public eScene curScene;


    // Initial spawn point location... Should be moved into savesettings 
    private Vector3 InitialSpawnPt_Pos;
    private Quaternion InitialSpawnPt_Rot;
    private Vector3 BoatSpawnPt_Pos;
    private Quaternion BoatSpawnPt_Rot;

    private void Awake()
    {
        Init();
        saveSettings = gameObject.GetComponent<SaveSettings>();
        // Currently all this script does is allow devs to see what tool is unlocked
        pToolUnlockPath = $"{Application.persistentDataPath}/pToolUnlock.json";
        if (File.Exists(pToolUnlockPath))
        {
            string json = File.ReadAllText(pToolUnlockPath);
            playerProgressionUnlock = JsonUtility.FromJson<PlayerProgressionUnlock>(json);
        }

    }
    private void Start()
    {
        // Grab the current Enviro instance.
        enviroSystem = EnviroManager.instance;
        eventSys = GetComponentInChildren<EventSystem>();
    }

    private void Update()
    {
        CheckUnlocks();
    }

    private void Init()
    {
        if(gm == null)
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }
        if (!inputManager) inputManager = gameObject.AddComponent<InputManager>();
        if (!audioManager) audioManager = gameObject.AddComponent<AudioManager>();
        if (!canvasManager) canvasManager = gameObject.AddComponent<CanvasManager>();
        if (!fadeManager) fadeManager = gameObject.GetComponentInChildren<LoadingManager>();
        if (!questManager) questManager = gameObject.GetComponentInChildren<QuestManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Debug.Log("Scene Disabled");
        Debug.Log($"{SceneManager.sceneCount}");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode) //Determines what scripts are loaded when the next scene loads. Double check eScene names with actual Scenes in Build in Build Settings.
    {
        curScene = (eScene)_scene.buildIndex;
        Debug.Log((eScene)_scene.buildIndex);
        switch (curScene)
        {
            case eScene.CTS_FrontEndScene:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
            default: //if current loaded scene ISN'T FE, load the HUD.
                if (displayCanvasHud)
                {
                    canvasManager.ShowCanvasHUD();
                    //Instantiate(Resources.Load("Widgets/Widget_FadeIn") as GameObject);
                }
                break;
        }

        //Try to override global references.
        
        playerReference = GameObject.FindGameObjectWithTag("Player");
        boatReference = GameObject.FindGameObjectWithTag("Ship");
        crestReference = GameObject.Find("Crest Ocean Systems").GetComponent<CrestManager>();
        enviroSystem = GameObject.Find("Enviro 3").GetComponent<EnviroManager>();

       

        if (SceneManager.GetActiveScene().name != "CTS_FrontEndScene" && SceneManager.GetActiveScene().name != "CTS_CabinScene" && SceneManager.GetActiveScene().name != "CTS_Letter")
        {
            weatherController = GameObject.FindGameObjectWithTag("Weather").GetComponent<WeatherController>();
        }
        if(SceneManager.GetActiveScene().name == "CTS_Letter")
        {
            playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
        }
        if (curScene != eScene.CTS_FrontEndScene)
        {
            inputManager.Init();

            inputManager.itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();
            inputManager.controllerManager = boatReference.GetComponent<ControllerManager>();
            inputManager.sailboatBehavior = boatReference.GetComponent<SailboatBehavior>();
            inputManager.movement = playerReference.GetComponent<PlayerMovement>();
            inputManager.playerSwimming = playerReference.GetComponent<PlayerSwimming>();
            inputManager.flashlightBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlashlightBehavior>(true);
            inputManager.flaregunBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlaregunBehavior>(true);
            if (!gm.playerProgressionUnlock.isAstrolabeUnlocked) gm.playerProgressionUnlock.isAstrolabeUnlocked = true;
        }
        NewGameSpawnLocation();

        if (SceneManager.GetActiveScene().name != "CTS_FrontEndScene" && SceneManager.GetActiveScene().name != "CTS_Letter")
        {
            canvasManager.canvasHUD.UpdateDpadUI(0);
            if (playerProgressionUnlock.isFlashlightUnlocked) canvasManager.canvasHUD.DisplayDpadPrompt(1);
            if (playerProgressionUnlock.isFlareGunUnlocked) canvasManager.canvasHUD.DisplayDpadPrompt(2);
            if (playerProgressionUnlock.isLightningBottleUnlocked) canvasManager.canvasHUD.DisplayDpadPrompt(3);
        }
    }

    public void CheckUnlocks()
    {
        // Check tool is unlocked when game starts
        if (playerProgressionUnlock.isAstrolabeUnlocked)
        {
           isToolUnlocked[0] = true;
           // Debug.Log($"{playerProgressionUnlock.isFlareGunUnlocked}");
        } 
        if (playerProgressionUnlock.isFlashlightUnlocked)
        {
            isToolUnlocked[1] = true;
            // Debug.Log($"{playerProgressionUnlock.isFlashlightUnlocked}");
        } else isToolUnlocked[1] = false;
        if (playerProgressionUnlock.isFlareGunUnlocked)
        {
            isToolUnlocked[2] = true;
           // Debug.Log($"{playerProgressionUnlock.isAstrolabeUnlocked}");
        } else isToolUnlocked[2] = false;
        if (playerProgressionUnlock.isLightningBottleUnlocked)
        {
            isToolUnlocked[3] = true;
            // Debug.Log($"{playerProgressionUnlock.isAstrolabeUnlocked}");
        }
        else isToolUnlocked[3] = false;
    }

    //needs to move in save settings after milestone 4
    public void NewGameSpawnLocation()
    {
        // If a new scene is added follow this layout, add a new bool in playerprogressionunlock to account for the checkpoint and then set that checkpoint true and all
        // others false TODO: (YES THIS NEEDS TO BE CHANGED AND OPTIMIZED IT IS)
        if (SceneManager.GetActiveScene().name == "CTS_Letter")
        {
            if (playerProgressionUnlock.checkpoint_Letter == false)
            {
                playerProgressionUnlock.checkpoint_Letter = true;
                playerProgressionUnlock.checkPoint_MainCabin = false;
                playerProgressionUnlock.checkPoint_Island = false;
            }
            GetSpawnLocations();
        }
        if (SceneManager.GetActiveScene().name == "CTS_CabinScene")
        {
            if (playerProgressionUnlock.checkPoint_MainCabin == false)
            {
                playerProgressionUnlock.checkpoint_Letter = false;
                playerProgressionUnlock.checkPoint_MainCabin = true;
                playerProgressionUnlock.checkPoint_Island = false;
            }
            
            GetSpawnLocations();
        }
        if (SceneManager.GetActiveScene().name == "CTS_WorldScene")
        {
            if (playerProgressionUnlock.checkPoint_Island == false)
            {
                playerProgressionUnlock.checkpoint_Letter = false;
                playerProgressionUnlock.checkPoint_MainCabin = false;
                playerProgressionUnlock.checkPoint_Island = true;
               
            }
            GetSpawnLocations();
        }
        string json = JsonUtility.ToJson(playerProgressionUnlock);
        File.WriteAllText(pToolUnlockPath, json);
    }

    public void GetSpawnLocations()
    {
        // Grabs the location of the objects when first entering the scene
        InitialSpawnPt_Pos = GameObject.FindGameObjectWithTag("InitialSpawnPt_Player").transform.position;
        InitialSpawnPt_Rot = GameObject.FindGameObjectWithTag("InitialSpawnPt_Player").transform.rotation;
        BoatSpawnPt_Pos = GameObject.FindGameObjectWithTag("BoatSpawnPt").transform.position;
        BoatSpawnPt_Rot = GameObject.FindGameObjectWithTag("BoatSpawnPt").transform.rotation;

        // Sets the spawn location of spawn in the cabin scene to be default and checks if its a new game or not
        if (playerProgressionUnlock.newGame == true)
        {
            Vector3 spawnLocation = InitialSpawnPt_Pos;
            Quaternion spawnRotation = InitialSpawnPt_Rot;
            if (curScene == eScene.CTS_Letter)
            {
                // Sets the spawn to be the initialSpawnPt_Player
                playerProgressionUnlock.respawnLocation = InitialSpawnPt_Pos;
                playerProgressionUnlock.respawnRotation = InitialSpawnPt_Rot;
            } 
            else
            {
                playerProgressionUnlock.respawnLocation = spawnLocation;
                playerProgressionUnlock.respawnRotation = spawnRotation;

            }
            // Saves the spawn locations in the json file
            string json = JsonUtility.ToJson(playerProgressionUnlock);
            File.WriteAllText(pToolUnlockPath, json);
        }
        else
        {
            Vector3 spawnLocation = playerProgressionUnlock.respawnLocation;
            Quaternion spawnRotation = playerProgressionUnlock.respawnRotation;
            playerProgressionUnlock.respawnLocation = spawnLocation;
            playerProgressionUnlock.respawnRotation = spawnRotation;
            // Saves the spawn locations in the json file
            string json = JsonUtility.ToJson(playerProgressionUnlock);
            File.WriteAllText(pToolUnlockPath, json);
        }
        // Sends the locations to the playermovement script for the player to spawn when scene first loads
        playerReference.transform.position = playerProgressionUnlock.respawnLocation;
        playerReference.transform.rotation = playerProgressionUnlock.respawnRotation;
        // Debug.Log(GameManager.gm.playerReference.transform.position);
        // Debug.Log(GameManager.gm.playerReference.transform.rotation);
    }
}
