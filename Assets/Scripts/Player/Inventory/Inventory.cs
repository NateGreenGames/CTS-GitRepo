using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int flaresPerPickup;

    //Checks to see if the Item is in the Inventory, if not it creates an Iventory Item and adds to the itemDictionary/Inventory
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void OnEnable()
    {
        // To add more collectable items copy the syntax below after duplicating the scripts and changing the naming convention
        Flowers.OnFlowerCollected += Add;
        sFlarePickup.OnFlareCollected += Add;
    }

    private void OnDisable()
    {
        Flowers.OnFlowerCollected -= Add;
    }

    public void Add(ItemData itemData)
    {
        // Checks to see if there is that item in the Inventory, and if there is, it increments up by 1
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            /* if (item.itemData.displayName == "Flare") for (int i = 0; i < flaresPerPickup; i++) item.AddToStack();
            else*/ 
            item.AddToStack();
            GameManager.gm.playerProgressionUnlock.curFlares = GameManager.gm.playerProgressionUnlock.curFlares + GameObject.FindGameObjectWithTag("FlarePickUp").GetComponent<sFlarePickup>().flareQuantity;
            Debug.Log($"{item.itemData.displayName} total stack is now {item.stackSize}");
        }
        // If we don't have that item, creates a new item and then stores it in the inventory and dictionary
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            GameManager.gm.playerProgressionUnlock.curFlares = GameManager.gm.playerProgressionUnlock.curFlares + GameObject.FindGameObjectWithTag("FlarePickUp").GetComponent<sFlarePickup>().flareQuantity;
            Debug.Log($"Added {itemData.displayName} to the inventory for the first time");
        }
    }

        public void Remove(ItemData itemData)
    {
        // Checks to see if there is that item in the Inventory, and if there is, it increments down by 1
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            // If we have that item, it removes 1 from the stack. If stack hits 0, removes it in the inventory and dictionary
            item.RemoveFromStack();
            Debug.Log($"{item.itemData.displayName} total stack is now {item.stackSize}");
            if (item.stackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }
}