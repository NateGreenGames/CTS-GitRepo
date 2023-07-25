using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SaveSettings : MonoBehaviour
{
    public static SaveSettings saveSettings;
    public PlayerProgressionUnlock player;

    private PlayerMovement playerMovement;
    private sBoatHealthTest boatHealthTest;
    private Inventory inventory;
    private FlaregunBehavior flaregunBehavior;
    private ItemSwapping itemSwapping;

    public string pToolUnlockPath;

    private void Awake()
    {
        pToolUnlockPath = $"{Application.persistentDataPath}/pToolUnlock.json";
        if (File.Exists(pToolUnlockPath))
        {
            string json = File.ReadAllText(pToolUnlockPath);
            player = JsonUtility.FromJson<PlayerProgressionUnlock>(json);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        player = GameManager.gm.playerProgressionUnlock;
        // Sets variables to null while in the frontend since they dont exist yet
        if (SceneManager.GetActiveScene().name == "CTS_FrontEndScene")
        {
            playerMovement = null;
            boatHealthTest = null;
            inventory = null;
            flaregunBehavior = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Delete(player);
            Debug.Log("Json File Deleted!");
        }
        
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Saving();
            Debug.Log("Game Saved!");
        }
        pToolUnlockPath = $"{Application.persistentDataPath}/pToolUnlock.json";
        if (File.Exists(pToolUnlockPath))
        {
            string json = File.ReadAllText(pToolUnlockPath);
            player = JsonUtility.FromJson<PlayerProgressionUnlock>(json);
        }

        LoadComponents();
    }
    public void Delete(PlayerProgressionUnlock player)
    {
        File.Delete(Application.persistentDataPath + "/pToolUnlock.json");
    }

    public void Saving()
    {
        player.newGame = false;
        player.haveSavedFile = true;
        // Grab Latest Checkpoint location
        player.respawnLocation = playerMovement.respawnLocation;
        player.respawnRotation = playerMovement.respawnRotation;
        // Debug.Log($"Player's current locations is {GameManager.gm.playerProgressionUnlock.respawnLocation}");

        // Current HP
        player.curPlayerHealth = playerMovement.currentHealth;
        // Debug.Log($"Saving Player HP: {GameManager.gm.playerProgressionUnlock.curPlayerHealth}");

        // Current Boat HP
        player.curBoatHealth = boatHealthTest.boatHealth;
        // Debug.Log($"Saving Player HP: {GameManager.gm.playerProgressionUnlock.curBoatHealth}");

        // Current Flares left
        player.curFlares = flaregunBehavior.flaresLeft;


        string json = JsonUtility.ToJson(player);
        File.WriteAllText(pToolUnlockPath, json);
    }

    public void NewGame()
    {
        Debug.Log("New Game started!");
        // These are the default settings for the save system.
        player.newGame = true;
        player.haveSavedFile = false;
        player.curBoatHealth = 100;
        player.curPlayerHealth = 100;
        player.curFlares = 0;
        player.isAstrolabeUnlocked = false;
        player.isFlareGunUnlocked = false;
        player.isFlashlightUnlocked = false;
        player.gryffnFirstVisit = true;
        player.calliopeFirstVisit = true;
        player.isLightningBottleUnlocked = false;
        player.isLightningBottleCharged = false;

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(pToolUnlockPath, json);
    }

    public void ContinueGame()
    {
        GameManager.gm.playerReference.transform.position = player.respawnLocation;
        GameManager.gm.playerReference.transform.rotation = player.respawnRotation;
    }

    public void AutoSave()
    { 
        Saving();
    }

    public void LoadComponents()
    {

        if (SceneManager.GetActiveScene().name != "CTS_FrontEndScene")
        {
            playerMovement = GameManager.gm.playerReference.GetComponent<PlayerMovement>();
            boatHealthTest = GameManager.gm.boatReference.GetComponent<sBoatHealthTest>();
            inventory = GameManager.gm.playerReference.GetComponent<Inventory>();
            flaregunBehavior = GameObject.Find("ToolContainer").GetComponentInChildren<FlaregunBehavior>(true);
            itemSwapping = GameObject.Find("ToolContainer").GetComponentInChildren<ItemSwapping>(true);
        }
    }
}
