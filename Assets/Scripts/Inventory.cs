using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Inventory
{
    public readonly int OwnerId;
    public readonly int Capacity;
    public readonly InventorySlot[] Content;

    private Inventory()
    {
    }

    public Inventory(int ownerId, ISessionSettings settings)
    {
        OwnerId = ownerId;
        Capacity = settings.InventoryCapacity;

        Content = new InventorySlot[Capacity];

        for (var i = 0; i < Capacity; ++i)
        {
            Content[i] = new InventorySlot(ownerId, settings.ItemPerSlot);
        }

        Refrences.instance.GameView.AddInventoryPanel(settings, this);
    }

    //Adding item with ItemId to slot in inventory
    //Priority for slots: 1. Slot with this item; 2. Empty slot.
    public void AddItem(int itemId, uint quantity)
    {
        var firstEmpty = -1;

        for (var i = 0; i < Content.Length; ++i)
        {
            /*Remember first empty place*/
            if (firstEmpty < 0 && Content[i].IsEmpty())
            {
                firstEmpty = i;
            }

            if (Content[i].ItemId == itemId && Content[i].IsFull())
            {
                Debug.LogFormat("Cannot carry so much item {0}", itemId);
                return;
            }

            /*Try to find simmiliar slot*/
            if (!Content[i].IsEmpty() && Content[i].AddItem(itemId, quantity))
            {
                return;
            }
        }

        if (firstEmpty < 0)
        {
            Debug.LogWarningFormat("Inventory is full! Item {0} need cannot be added", itemId);
            return;
        }

        /*Add item to first free slot*/
        Content[firstEmpty].AddItem(itemId, quantity);
    }

    //Removes one item from item slot with SlotId
    public void RemoveItem(int itemId, uint quantity)
    {
        for (var i = 0; i < Content.Length; ++i)
        {
            if (Content[i].RemoveItem(itemId, quantity))
            {
                return;
            }
        }

        Debug.LogWarningFormat("Cannot find item {0} for remove!", itemId);
    }

    //Shows inventory content in format "Item ID in quantity QUANTITY"
    public void LogContent()
    {
        var log = this + "\n" +
                  Content.Where(slot => !slot.IsEmpty())
                      .Aggregate("", (current, slot) => current + slot);

        Debug.Log(log);
    }

    public override string ToString()
    {
        return string.Format("Inventory of owner#{0}, capacity: {1}\n", OwnerId, Capacity);
    }
}