using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
    public const int Capacity = 50;

    private readonly InventorySlot[] Content;

    public Inventory()
    {
        Content = new InventorySlot[Capacity];
    }

    //Adding item with ItemId to slot in inventory
    //Priority for slots: 1. Slot with this item; 2. Empty slot.
    public void AddItem(int ItemId)
    {
        for (int i = 0; i < Content.Length; ++i)
        {
            if (Content[i].ItemId == ItemId && !Content[i].IsFull())
            {
                //TODO: Think about situation, when first will be empty (array on-delete-offset)
                Content[i].AddItem(ItemId);
                return;
            }
        }

#if UNITY_EDITOR
        Debug.LogWarningFormat("Inventory is full! Item {0} need cannot be added", ItemId);
#endif
    }

    //Removes one item from item slot with SlotId
    public void RemoveItem(int SlotId)
    {
        if (SlotId > -1 && SlotId < Capacity)
        {
            //TODO: Array-on-delete-offset
            Content[SlotId].RemoveItem();
        }

#if UNITY_EDITOR
        Debug.LogWarningFormat("Slot ID is incorrect: {0}", SlotId);
#endif

        return;
    }

    //Shows inventory content in format "Item ID in quantity QUANTITY"
    public void LogContent()
    {
        string log = "";
        foreach (var Slot in Content)
        {
            if (!Slot.IsEmpty())
            {
                //Will be duplicate stacks
                log += String.Format("Item {0} in quantity {1}\n", Slot.ItemId, Slot.Quantity);
            }
        }
        Debug.Log(log);
    }
}