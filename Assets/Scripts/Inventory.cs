﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Inventory
{
    public readonly int OwnerId;
    public readonly int Capacity;
    public readonly InventorySlot[] Content;
    
    public static Dictionary<int, Item> ItemsBase;
    
    private Inventory() {}
    
    public Inventory(int ownerId, ISessionSettings settings)
    {
        OwnerId = ownerId;
        Capacity = settings.InventoryCapacity;
        
        if (ItemsBase == null)
        {
            ItemsBase = new Dictionary<int, Item>();
        }
        
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
        for (var i = 0; i < Content.Length; ++i)
        {
            if (Content[i].AddItem(itemId, quantity)){
                return;
            }
        }
    
        Debug.LogWarningFormat("Inventory is full! Item {0} need cannot be added", itemId);
    }

    //Removes one item from item slot with SlotId
    public void RemoveItem(int itemId, uint quantity)
    {
        for (var i = 0; i < Content.Length; ++i)
        {
            if (Content[i].RemoveItem(itemId, quantity)){
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
        return string.Format("Inventory of owner#{0}, capacity: {1}", OwnerId, Capacity);
    }
}