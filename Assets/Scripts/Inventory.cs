using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static ItemsInfo;

public class Inventory : MonoBehaviour
{
    public int MaxSlotsInRightHand = 10;
    public int MaxSlotsInLeftHand = 1;
    private int rightHandCurrentSlotIndex = -1;
    private int leftHandCurrentSlotIndex = -1;
    [SerializeField] private ItemsInfo ItemsInfo;
    private List<InventorySlot> rightHandSlots = new List<InventorySlot>();
    private List<InventorySlot> leftHandSlots = new List<InventorySlot>();
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private CurrentItem currentItem;
    public Color NormalSlotColor;
    [SerializeField] private Transform dropPoint;

    private void Start()
    {
        ChangeSlot(true);
    }
    private void OnSwitchSlot(InputValue value)
    {
        ChangeSlot(value.Get<float>() > 0? false:true);
    }
    private bool OnTakeOut(InputValue value) 
    {
        if(rightHandSlots != null && rightHandCurrentSlotIndex < rightHandSlots.Count && rightHandCurrentSlotIndex >= 0)
        {
            int currentItemTupe = rightHandSlots[rightHandCurrentSlotIndex].ItemType;
            //Vector3 ThrowPos = new Vector3(transform.position.x + Random.Range(-0.4f, 0.4f), transform.position.y + Random.Range(1f, 1.5f), 0);
            foreach (ItemTupeInfos currentSlot in ItemsInfo.itemTupesInfos)
            {
                if(currentSlot.typeNumber == currentItemTupe && RemoveItem(currentItemTupe, rightHandSlots, MaxSlotsInRightHand))
                {
                    Instantiate(ItemsInfo.itemTupesInfos[currentItemTupe - 1].prefab, dropPoint.position, transform.rotation);
                    return true;
                }
            }            
        }
        return false;
    }
    private void OnGetToOtherHand()
    {
        //to do
    }
    public bool AddItem(int itemType, List<InventorySlot> slots, int maxSlotsCount, int itemAmount = 1)
    {
        InventorySlot slot = null;
        if (slots.Count > 0)
            slot = slots.Find(s => s.ItemType == itemType);

        if (slot != null && slot.Recount(1))
            return true;                      
        else if (slots.Count < maxSlotsCount)
        {
            slots.Add(inventoryPanel.AddItem(itemType, itemAmount));
            if (rightHandCurrentSlotIndex >= 0)
            {
                ChangeSlot(true);
                print("changed slot (67)");
            } 
            return true;
        }

        return false;// Inventory is full or max slots per type reached
    }

    public bool RemoveItem(int itemType, List<InventorySlot> slots, int maxSlotsCount, bool isRightHand = true)
    {
        InventorySlot slot = null;
        if (slots.Count > 0)
            slot = slots.Find(s => s.ItemType == itemType);

        if (slot != null)
        {
            if(slot.GetItemAmount() > 1)
            {
                slot.Recount(-1);
                return true;
            }
            else 
            { 
                if(slots.Count > 1) 
                {
                    if ((isRightHand ? rightHandCurrentSlotIndex: -1) != slot.Index) 
                    {
                        slot.Recount(-1);
                        slots.Remove(slot);
                        return true;
                    }
                    else 
                    {
                        slots[isRightHand ? rightHandCurrentSlotIndex : leftHandCurrentSlotIndex].transform.parent.GetComponent<Image>().color = NormalSlotColor;
                        slot.Recount(-1);
                        slots.Remove(slot);
                        if (ChangeSlot(false))
                            return true;
                        return false;
                    }
                }
                else
                {
                    slots[rightHandCurrentSlotIndex].transform.parent.GetComponent<Image>().color = NormalSlotColor;
                    rightHandCurrentSlotIndex = -1;
                    currentItem.SetCurrentItem(-1);
                    slot.Recount(-1);
                    slots.Remove(slot);
                    return true;
                } 
            }
        }
        return false;

    }

    public int GetItemAmount(int itemType, bool isRightHand = true)
    {
        List<InventorySlot> slots = (isRightHand ? rightHandSlots : leftHandSlots);
        InventorySlot slot = slots.Find(s => s.ItemType == itemType);
        return slot != null ? slot.GetItemAmount() : 0;
    }
    private bool ChangeSlot(bool direction, bool isRightHand = true) 
    {
        int currentSlotIndex = (isRightHand ? rightHandCurrentSlotIndex : leftHandCurrentSlotIndex);
        List<InventorySlot> staticSlots = (isRightHand ? rightHandSlots : leftHandSlots);
        if (staticSlots != null && staticSlots.Count > 0 && !(direction && currentSlotIndex + 1 >= staticSlots.Count))
        {
            int newCurrentSlotIndex = direction ? currentSlotIndex + 1 : currentSlotIndex - 1;
            if (newCurrentSlotIndex < 0)
                newCurrentSlotIndex = 0;
            if (isRightHand)
                currentItem.SetCurrentItem(staticSlots[newCurrentSlotIndex].ItemType);
            RewriteInventory();
            (isRightHand ? rightHandSlots : leftHandSlots)[newCurrentSlotIndex].transform.parent.GetComponent<Image>().color = Color.yellow;
            return true;
        }
        return false;
    }
    private void RewriteInventory(bool isRightHand = true)
    {
        List<InventorySlot> slots = (isRightHand ? rightHandSlots : leftHandSlots);
        int maxI = slots.Count - 1;
        List <InventorySlot> slots1 = new List<InventorySlot>();
        for (int i = 0; i <= maxI; i++)
        {
            InventorySlot slot = slots[i];           
            int itemTupe = slot.ItemType;
            int itemAmount = slot.GetItemAmount();
            slot.Recount(-itemAmount);
            AddItem(itemTupe, slots1, (isRightHand ? MaxSlotsInRightHand : MaxSlotsInLeftHand), itemAmount);
        }
        if (slots1.Count > 0)
        {
            slots.Clear();
            slots = slots1;
            if (isRightHand)
            {
                rightHandSlots.Clear();
                rightHandSlots = slots;
            }        
            else
            {
                leftHandSlots.Clear();
                leftHandSlots = slots;
            }
        }
        print("rewrited  inventory (166)");
    }
    public List<InventorySlot> GetSlots(bool isRightHand = true) => (isRightHand ? rightHandSlots : leftHandSlots);
}
