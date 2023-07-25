using System.IO;
using UnityEngine;

public enum eToolRtHand { astrolabe, flashlight, flareGun, lightningBottle, emptyHand, terminator }
public enum eToolLtHand { emptyHand, shovel,  lighter, terminator }

public class PlayerTools : MonoBehaviour, IInteractable
{
    // TODO: Convert this script to use Enum
    // Make Scriptable objects

    public PlayerProgressionUnlock pToolUnlocked;
    public GameObject rtHand;

    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }

    [SerializeField]
    private ItemSwapping itemSwapping;
    private string pToolUnlockPath;

    private void Start()
    {
        rtHand = GameObject.Find("ToolContainer");
        itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();
        // Saves the tool unlock states to an external .json file that persists through plays
       
        HudMessage = "Interact to pick up";
        isInteractable = true;
    }

    private void Update()
    {
        // Currently all this script does is allow devs to see what tool is unlocked
        pToolUnlockPath = $"{Application.persistentDataPath}/pToolUnlock.json";
        if (File.Exists(pToolUnlockPath))
        {
            string json = File.ReadAllText(pToolUnlockPath);
            pToolUnlocked = JsonUtility.FromJson<PlayerProgressionUnlock>(json);
        }
    }
    public void OnLookingAt()
    {

    }
    public void OnInteract()
    {
      //  OnCollect();
    }

}

