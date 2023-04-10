
for (int i = 0; i < slots.Count; i++)
{
    InventorySlot slot = slots[i];
    if (slot.Index > i)
    {
        int itemTupe = slot.ItemType;
        int itemAmount = slot.GetItemAmount();
        slot.Recount(-itemAmount);
        slots.Remove(slot);
        AddItem(itemTupe, itemAmount);
        Debug.Log("changed position of itemTupe: " + itemTupe + " from slot index: " + slot.Index + " slots count: " + slots.Count + " [162]");
    }
    Debug.Log("slot index: " + slot.Index + " i: " + i);
}
if (currentSlotIndex >= 0)
{
    slots[currentSlotIndex].transform.parent.GetComponent<Image>().color = Color.yellow;
    currentItem.SetCurrentItem(slots[currentSlotIndex].ItemType);
    Debug.Log("current slot index: " + currentSlotIndex);
}
else
{
    currentSlotIndex = 0;
    slots[currentSlotIndex].transform.parent.GetComponent<Image>().color = Color.yellow;
    currentItem.SetCurrentItem(slots[currentSlotIndex].ItemType);
    Debug.Log("slot has been automaticly changed to 0");
}
return true;
