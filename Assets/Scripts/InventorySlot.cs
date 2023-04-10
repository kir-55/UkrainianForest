using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventorySlot : MonoBehaviour
{
    public int ItemType;
    private int ItemAmount = 0;
    public Inventory Inventory;
    public int Index;
    [SerializeField] private TMP_Text ItemAmountText;
    [SerializeField] private Image ItemIcon;
    public Color NormalSlotColor;
    public InventorySlot RewriteInventorySlot(int itemType, Sprite itemIcon, int itemAmount = 1)
    {
        if(itemType != 0) 
        {
            this.ItemType = itemType;
            this.ItemAmount = itemAmount;
            this.ItemAmountText.text = ItemAmount.ToString();
            this.ItemIcon.sprite = itemIcon;
            return this;
        }
        //Debug.Log("itemType == 0");
        return null;
       
    }
    public bool Recount(int value) 
    {
        if((ItemAmount + value) >= 0)
        {
            ItemAmount += value;
            ItemAmountText.text = ItemAmount.ToString();
            if (ItemAmount == 0)
            {
                ItemType = 0;
                ItemIcon.sprite = null;
                transform.parent.GetComponent<Image>().color = NormalSlotColor;
            }
            return true;
        }
        return false; 
    }
    public int GetItemAmount() => ItemAmount;
}
