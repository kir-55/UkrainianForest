using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public GameObject slotPrefab;
    public Inventory inventory;
    public GridLayoutGroup gridLayout;
    public ItemsInfo itemsInfo;
    private Color normalSlotColor;
    public InventorySlot AddItem(int itemTupe, int itemAmount = 1)
    {
        for (int i = 0; i < inventory.maxSlotsPerType; i++)
        {
            InventorySlot physicSlot = transform.GetChild(i).GetChild(1).GetComponent<InventorySlot>();
            if (physicSlot.ItemType == 0)
            {
                //Debug.Log("itemTupe = " + itemTupe);
                Sprite itemIcon = itemsInfo.itemTupesInfos[itemTupe - 1].icon;
                physicSlot = physicSlot.RewriteInventorySlot(itemTupe, itemIcon, itemAmount);
                return physicSlot;
            }
        }
        return null;
    }

    private void Start()
    {
        normalSlotColor = inventory.NormalSlotColor;
        // Instantiate one slot for each inventory slot
        for (int i = 0; i < inventory.maxSlotsPerType; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, gridLayout.transform);
            InventorySlot slot = slotObject.transform.GetChild(1).GetComponent<InventorySlot>();
            slot.NormalSlotColor = normalSlotColor;

            // Set the slot's index and inventory reference
            slot.Index = i;
            slot.Inventory = inventory;
        }
    }
}
