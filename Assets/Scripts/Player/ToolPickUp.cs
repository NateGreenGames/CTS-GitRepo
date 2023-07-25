using UnityEngine;
using System.IO;

public class ToolPickUp : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    // public static event ToolsCollected onToolsCollected; // Not sure if this is needed anymore for tools (shouldn't be)
    public delegate void ToolsCollected(ItemData itemData);
    public ItemData itemData;

    public ItemSwapping itemSwapping;
    public PlayerProgressionUnlock pToolUnlocked;

    private string pToolUnlockPath;
    private int widgetToSpawn;

    private void Start()
    {
        itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();
        DestroyTool();
    }
    private void Update()
    {
        OnLookingAt();
        pToolUnlockPath = $"{Application.persistentDataPath}/pToolUnlock.json";
        if (File.Exists(pToolUnlockPath))
        {
            string json = File.ReadAllText(pToolUnlockPath);
            pToolUnlocked = JsonUtility.FromJson<PlayerProgressionUnlock>(json);
        }
    }

    public void OnInteract()
    {
        SaveJson();

        Destroy(gameObject);
    }

    public void OnLookingAt()
    {
        HudMessage = $"Pick up {itemData.displayName}";
        isInteractable = true;
    }

    void SaveJson()
    {
        // The item data determines which tool is unlocked after it is picked up
        // Each place in the array = a tool
        // Follow if template to add more tools in the future

        // element 0 = astrolabe, element 1 = flashlight, element 2 = flaregun, element 3 = lightningbottle
        if (itemData.displayName == "Flashlight")
        {
            widgetToSpawn = 1;
            GameManager.gm.isToolUnlocked[1] = true;
            GameManager.gm.playerProgressionUnlock.isFlashlightUnlocked = true;
            itemSwapping.selectedTool = (int)eToolRtHand.flashlight;
            itemSwapping.RightItemSwap(itemSwapping.sRep);
            itemSwapping.HandleAnimation(itemSwapping.selectedTool);
            GameManager.gm.canvasManager.canvasHUD.tutorialHUD.DiscoverItemUI(itemSwapping.selectedTool);
            GameManager.gm.canvasManager.canvasHUD.DisplayDpadPrompt(itemSwapping.selectedTool);
            GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(itemSwapping.selectedTool);

        }
        if (itemData.displayName == "Flare Gun")
        {
            widgetToSpawn = 2;
            GameManager.gm.isToolUnlocked[2] = true;
            GameManager.gm.playerProgressionUnlock.isFlareGunUnlocked = true;
            itemSwapping.selectedTool = (int)eToolRtHand.flareGun;
            itemSwapping.RightItemSwap(itemSwapping.sRep);
            GameManager.gm.canvasManager.canvasHUD.tutorialHUD.DiscoverItemUI(itemSwapping.selectedTool);
            GameManager.gm.canvasManager.canvasHUD.DisplayDpadPrompt(itemSwapping.selectedTool);
            GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(itemSwapping.selectedTool);
        }
        if (itemData.displayName == "Astrolabe")
        {
            widgetToSpawn = 3;
            GameManager.gm.isToolUnlocked[3] = true;
            GameManager.gm.playerProgressionUnlock.isAstrolabeUnlocked = true;
            itemSwapping.selectedTool = (int)eToolRtHand.astrolabe;
            itemSwapping.RightItemSwap(itemSwapping.sRep);
            GameManager.gm.canvasManager.canvasHUD.tutorialHUD.DiscoverItemUI(itemSwapping.selectedTool);
            GameManager.gm.canvasManager.canvasHUD.DisplayDpadPrompt(itemSwapping.selectedTool);
            GameManager.gm.canvasManager.canvasHUD.UpdateDpadUI(itemSwapping.selectedTool);
        }
        string json = JsonUtility.ToJson(pToolUnlocked);
        File.WriteAllText(pToolUnlockPath, json);
    }

    public void ObtainFlareGun()
    {
        // Gives player flare gun
        GameManager.gm.isToolUnlocked[1] = true;
        GameManager.gm.playerProgressionUnlock.isFlareGunUnlocked = true;
        itemSwapping.selectedTool = (int)eToolRtHand.flareGun;
        itemSwapping.RightItemSwap(itemSwapping.sRep);
    }

    public void DestroyTool()
    {
        if (GameManager.gm.playerProgressionUnlock.isFlashlightUnlocked == true)
        {
            if (GameObject.Find("Flashlight (Pickup)"))
            {
                Destroy(GameObject.Find("Flashlight (Pickup)"));
            }
        }
        if (GameManager.gm.playerProgressionUnlock.isFlareGunUnlocked == true)
        {
            if (GameObject.Find("FlareGun (Pickup)"))
            {
                Destroy(GameObject.Find("FlareGun (Pickup)"));
            }
        }
        if (GameManager.gm.playerProgressionUnlock.isAstrolabeUnlocked == true)
        {
            if (GameObject.Find("Astrolabe (Pickup)"))
            {
                Destroy(GameObject.Find("Astrolabe (Pickup)"));
            }
        }
    }
}
