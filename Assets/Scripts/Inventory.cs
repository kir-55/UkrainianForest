using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSlotsPerType = 10;

    private List<InventorySlot> slots = new List<InventorySlot>();

    public bool AddItem(int itemType)
    {
        // Check if we already have the item type in the inventory
        InventorySlot slot = slots.Find(s => s.itemType == itemType);

        if (slot != null && slot.count < maxSlotsPerType)// Check if we can add the item to an existing slot
        {
            slot.count++;
            return true;
        }                         
        else if (slots.Count < maxSlotsPerType)
        {
            slots.Add(new InventorySlot(itemType, 1));
            return true;
        }

        return false;// Inventory is full or max slots per type reached
    }

    public void RemoveItem(int itemType)
    {
        InventorySlot slot = slots.Find(s => s.itemType == itemType);
        if (slot != null)
        {
            slot.count--;
            if (slot.count == 0)
                slots.Remove(slot);
        }
    }

    public int GetItemCount(int itemType)
    {
        InventorySlot slot = slots.Find(s => s.itemType == itemType);
        return slot != null ? slot.count : 0;
    }

    public List<InventorySlot> GetSlots() => slots;
}
