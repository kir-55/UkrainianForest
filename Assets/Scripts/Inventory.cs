using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSlotsPerType = 10;

   public List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] private InventoryPanel inventoryPanel;
    public bool AddItem(int itemType)
    {
        // Check if we already have the item type in the inventory
        InventorySlot slot = slots.Find(s => s.ItemType == itemType);

        if (slot != null)// Check if we can add the item to an existing slot
        {
            if(slot.Recount(1))
                Debug.Log("changing ");
            return true;
        }                         
        else if (slots.Count < maxSlotsPerType)
        {
            Debug.Log("adding");
            slots.Add(inventoryPanel.AddItem(itemType));
            return true;
        }

        return false;// Inventory is full or max slots per type reached
    }

    public void RemoveItem(int itemType)
    {
        InventorySlot slot = slots.Find(s => s.ItemType == itemType);
        if (slot != null)
        {
            slot.Recount(-1);
            if (slot.GetItemAmount() == 0)
                slots.Remove(slot);
        }
    }

    public int GetItemCount(int itemType)
    {
        InventorySlot slot = slots.Find(s => s.ItemType == itemType);
        return slot != null ? slot.GetItemAmount() : 0;
    }

    public List<InventorySlot> GetSlots() => slots;
}
