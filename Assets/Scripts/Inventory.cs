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
    public int maxSlotsPerType = 10;
    private int currentSlotIndex = -1;
    [SerializeField] private ItemsInfo ItemsInfo;
    public List<InventorySlot> slots = new List<InventorySlot>();
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
        if(slots != null && currentSlotIndex < slots.Count && currentSlotIndex >= 0)
        {
            int currentItemTupe = slots[currentSlotIndex].ItemType;
            //Vector3 ThrowPos = new Vector3(transform.position.x + Random.Range(-0.4f, 0.4f), transform.position.y + Random.Range(1f, 1.5f), 0);
            foreach (ItemTupeInfos currentSlot in ItemsInfo.itemTupesInfos)
            {
                if(currentSlot.typeNumber == currentItemTupe && RemoveItem(currentItemTupe))
                {
                    Instantiate(ItemsInfo.itemTupesInfos[currentItemTupe - 1].prefab, dropPoint.position, transform.rotation);
                    return true;
                }
            }            
        }
        return false;
    }
    public bool AddItem(int itemType, List<InventorySlot> slots1, int itemAmount = 1)
    {
        InventorySlot slot = null;
        // Check if we already have the item type in the inventory
        if (slots1.Count > 0)
            slot = slots1.Find(s => s.ItemType == itemType);

        if (slot != null)// Check if we can add the item to an existing slot
        {
            if (slot.Recount(1)) 
            {
                //Debug.Log("changing ");
                return true;
            }
            Debug.Log("F1");
            return false;
        }                         
        else if (slots.Count < maxSlotsPerType)
        {
            //Debug.Log("adding");
            slots1.Add(inventoryPanel.AddItem(itemType, itemAmount));
            if(currentSlotIndex < 0)
                ChangeSlot(true);
            return true;
        }

        return false;// Inventory is full or max slots per type reached
    }

    public bool RemoveItem(int itemType)
    {
        if (slots != null && currentSlotIndex < slots.Count)
        {
            InventorySlot slot = slots.Find(s => s.ItemType == itemType);
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
                        if(currentSlotIndex != slot.Index) 
                        {
                            slot.Recount(-1);
                            slots.Remove(slot);
                            return true;
                        }
                        else 
                        {
                            slots[currentSlotIndex].transform.parent.GetComponent<Image>().color = NormalSlotColor;
                            slot.Recount(-1);
                            slots.Remove(slot);
                            //Debug.Log("removing");
                            if (ChangeSlot(false))
                                return true;
                            return false;
                            //return false;
                        }
                        //else
                        //{
                        //    Debug.Log("F2???");
                        //    return false;
                        //}
                    }
                    else
                    {
                        slots[currentSlotIndex].transform.parent.GetComponent<Image>().color = NormalSlotColor;
                        currentSlotIndex = -1;
                        currentItem.SetCurrentItem(-1);
                        slot.Recount(-1);
                        slots.Remove(slot);
                        //Debug.Log("T1???");
                        return true;
                    } 
                }
            }
            //Debug.Log("F3???");
            return false;
        }
        //Debug.Log("F4???");
        return false;

    }

    public int GetItemCount(int itemType)
    {
        InventorySlot slot = slots.Find(s => s.ItemType == itemType);
        return slot != null ? slot.GetItemAmount() : 0;
    }
    private bool ChangeSlot(bool direction) 
    {
        
        if (slots != null && slots.Count > 0 && !(direction && currentSlotIndex + 1 >= slots.Count))//((direction && currentSlotIndex < slots.Count - 1) || (!direction && currentSlotIndex > 0))
        {

            //Debug.Log("slots.Count " + slots.Count);


            currentSlotIndex = direction ? currentSlotIndex + 1 : currentSlotIndex - 1;
            if (currentSlotIndex >= 0)
            {
                print("color of slot: " + currentSlotIndex + "has been chenged to yellow");
                currentItem.SetCurrentItem(slots[currentSlotIndex].ItemType);
                Debug.Log("current slot index: " + currentSlotIndex);
                
            }
            else
            {
                currentSlotIndex = 0;
                print("color of slot: " + currentSlotIndex + "has been chenged to yellow");
                currentItem.SetCurrentItem(slots[currentSlotIndex].ItemType);
                Debug.Log("slot has been automaticly changed to 0");
            }
            RewriteInventory();
            slots[currentSlotIndex].transform.parent.GetComponent<Image>().color = Color.yellow;
            return true;
        }
        return false;
    }
    private void RewriteInventory()
    {
        int maxI = slots.Count - 1;
        List <InventorySlot> slots1 = new List<InventorySlot>();
        for (int i = 0; i <= maxI; i++)
        {
            InventorySlot slot = slots[i];           
            int itemTupe = slot.ItemType;
            int itemAmount = slot.GetItemAmount();
            //slots1.Remove(slot);
            slot.Recount(-itemAmount);
            //Debug.Log("item tupe:" + itemTupe);
            AddItem(itemTupe, slots1, itemAmount);
            //Debug.Log("changed position of itemTupe: " + itemTupe + " from slot index: " + slot.Index + " slots1 count: " + slots1.Count + " slots count" + slots1.Count + " [162]");
       
            //Debug.Log("slot index: " + slot.Index + " i: " + i + " slots1 count: " + slots1.Count);
            
        }
        if (slots1.Count > 0)
        {
            slots.Clear();
            slots = slots1;
        }
            
    }
    public List<InventorySlot> GetSlots() => slots;
}
