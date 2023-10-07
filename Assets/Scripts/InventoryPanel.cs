using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public GameObject SlotPrefab, RightHand, LeftHand;
    public Inventory inventory;
    public ItemsInfo itemsInfo;
    private Color normalSlotColor;
    public InventorySlot AddItem(int itemTupe, int itemAmount = 1, bool isRightHand = true)
    {
        for (int i = 0; i < (isRightHand? inventory.MaxSlotsInRightHand : inventory.MaxSlotsInLeftHand); i++)
        {
            InventorySlot physicSlot = (isRightHand? RightHand: LeftHand).transform.GetChild(i).GetChild(1).GetComponent<InventorySlot>();
            if (physicSlot.ItemType == 0)
            {
                Sprite itemIcon = itemsInfo.itemInfos[itemTupe - 1].icon;
                physicSlot = physicSlot.RewriteInventorySlot(itemTupe, itemIcon, itemAmount);
                return physicSlot;
            }
        }
        return null;
    }
    private void GenerateSlots(int count, GameObject hand)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject slotObject = Instantiate(SlotPrefab, hand.GetComponent<GridLayoutGroup>().transform);
            InventorySlot slot = slotObject.transform.GetChild(1).GetComponent<InventorySlot>();
            slot.NormalSlotColor = normalSlotColor;
            slot.Index = i;
            slot.Inventory = inventory;
        }
    }
    private void Start()
    {
        normalSlotColor = inventory.NormalSlotColor;
        GenerateSlots(inventory.MaxSlotsInRightHand, RightHand);
        GenerateSlots(inventory.MaxSlotsInLeftHand, LeftHand);
    }
}
