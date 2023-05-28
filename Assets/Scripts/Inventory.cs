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
    private int leftHandCurrentSlotIndex = 0;
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
                if(currentSlot.typeNumber == currentItemTupe && RemoveItem(currentItemTupe, rightHandSlots))
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
        int leftHandItemTupe = -1;
        int leftHandItemAmount = 0;
        int rightHandItemTupe = -1;
        int rightHandItemAmount = 0;
        if (leftHandCurrentSlotIndex > -1 && leftHandSlots.Count > leftHandCurrentSlotIndex && leftHandSlots[leftHandCurrentSlotIndex])
        {
            leftHandItemTupe = leftHandSlots[leftHandCurrentSlotIndex].ItemType;
            leftHandItemAmount = leftHandSlots[leftHandCurrentSlotIndex].GetItemAmount();
        }

        if (rightHandCurrentSlotIndex > -1 && rightHandSlots.Count > rightHandCurrentSlotIndex && rightHandSlots[rightHandCurrentSlotIndex])
        {
            rightHandItemTupe = rightHandSlots[rightHandCurrentSlotIndex].ItemType;
            rightHandItemAmount = rightHandSlots[rightHandCurrentSlotIndex].GetItemAmount();
            if (leftHandItemAmount > 0)
            {
                rightHandSlots[rightHandCurrentSlotIndex].RewriteInventorySlot(leftHandItemTupe, ItemsInfo.itemTupesInfos[leftHandItemTupe - 1].icon, leftHandItemAmount);
                currentItem.SetCurrentItem(rightHandSlots[rightHandCurrentSlotIndex].ItemType);
            }
                
        }
        else if(leftHandItemAmount > 0)
        {
            AddItem(leftHandItemTupe, rightHandSlots, MaxSlotsInRightHand, leftHandItemAmount);
            RemoveItem(leftHandItemTupe, leftHandSlots, false);
        }
    
        if (leftHandCurrentSlotIndex > -1 && leftHandSlots.Count > leftHandCurrentSlotIndex && leftHandSlots[leftHandCurrentSlotIndex] && rightHandItemAmount > 0)
        {
            leftHandSlots[leftHandCurrentSlotIndex].RewriteInventorySlot(rightHandItemTupe, ItemsInfo.itemTupesInfos[rightHandItemTupe - 1].icon, rightHandItemAmount);
            currentItem.SetCurrentItem(leftHandSlots[leftHandCurrentSlotIndex].ItemType, false);
        }
        else if(rightHandItemAmount > 0) 
        {
            AddItem(rightHandItemTupe, leftHandSlots, MaxSlotsInLeftHand, rightHandItemAmount, false);
            RemoveItem(rightHandItemTupe, rightHandSlots);
            print("lhcsi: " + leftHandCurrentSlotIndex);
            if(leftHandSlots.Count > leftHandCurrentSlotIndex)
                currentItem.SetCurrentItem(leftHandSlots[leftHandCurrentSlotIndex].ItemType, false);
        }
 
        RewriteInventory();
        if (leftHandCurrentSlotIndex > -1 && leftHandSlots.Count > leftHandCurrentSlotIndex)
            leftHandSlots[leftHandCurrentSlotIndex].transform.parent.GetComponent<Image>().color = Color.yellow;
        if (rightHandCurrentSlotIndex > -1 && rightHandSlots.Count > rightHandCurrentSlotIndex)
            rightHandSlots[rightHandCurrentSlotIndex].transform.parent.GetComponent<Image>().color = Color.yellow;
    }
    public bool AddItem(int itemType, List<InventorySlot> slots, int maxSlotsCount, int itemAmount = 1, bool isRightHand = true)
    {
        InventorySlot slot = null;
        if (slots.Count > 0)
            slot = slots.Find(s => s.ItemType == itemType);

        if (slot != null && slot.Recount(1))
            return true;                      
        else if (slots.Count < maxSlotsCount)
        {
            print("eee: " + isRightHand);
            slots.Add(inventoryPanel.AddItem(itemType, itemAmount, isRightHand));
            if((isRightHand? rightHandCurrentSlotIndex:leftHandCurrentSlotIndex) < 0)
                ChangeSlot(true, isRightHand);

            return true;
        }

        return false;// Inventory is full or max slots per type reached
    }

    public bool RemoveItem(int itemType, List<InventorySlot> slots, bool isRightHand = true)
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
                    if ((isRightHand ? rightHandCurrentSlotIndex: leftHandCurrentSlotIndex) != slot.Index) 
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
                        else
                            return false;
                    }
                }
                else
                {
                    if(slots.Count > (isRightHand ? rightHandCurrentSlotIndex : leftHandCurrentSlotIndex))
                        slots[isRightHand ? rightHandCurrentSlotIndex : leftHandCurrentSlotIndex].transform.parent.GetComponent<Image>().color = NormalSlotColor;
                    
                    if(isRightHand)
                        rightHandCurrentSlotIndex = -1;
                    else
                        leftHandCurrentSlotIndex = -1;
                    currentItem.SetCurrentItem(-1, isRightHand);
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
                rightHandCurrentSlotIndex = newCurrentSlotIndex;
            else
                leftHandCurrentSlotIndex = newCurrentSlotIndex;

            currentItem.SetCurrentItem(staticSlots[newCurrentSlotIndex].ItemType, isRightHand);
            RewriteInventory(isRightHand);
            (isRightHand ? rightHandSlots : leftHandSlots)[newCurrentSlotIndex].transform.parent.GetComponent<Image>().color = Color.yellow;
            return true;
        }
        return false;
    }
    private void RewriteInventory(bool isRightHand = true)
    {
        List<InventorySlot> staticSlots = (isRightHand ? rightHandSlots : leftHandSlots);
        int maxI = staticSlots.Count - 1;
        List <InventorySlot> slots1 = new List<InventorySlot>();
        for (int i = 0; i <= maxI; i++)
        {
            InventorySlot slot = staticSlots[i];           
            int itemTupe = slot.ItemType;
            int itemAmount = slot.GetItemAmount();
            slot.Recount(-itemAmount);
            AddItem(itemTupe, slots1, (isRightHand ? MaxSlotsInRightHand : MaxSlotsInLeftHand), itemAmount, isRightHand);
        }
        if (slots1.Count > 0)
        {
            if (isRightHand)
            {
                rightHandSlots.Clear();
                rightHandSlots = slots1;
            }        
            else
            {
                leftHandSlots.Clear();
                leftHandSlots = slots1;
            }
        }
        print("rewrited  inventory (166) right hand slots amount: " + rightHandSlots.Count); 
        print("rewrited  inventory (166) left hand slots amount: " + leftHandSlots.Count);
    }
    public List<InventorySlot> GetSlots(bool isRightHand = true) => (isRightHand ? rightHandSlots : leftHandSlots);
}
