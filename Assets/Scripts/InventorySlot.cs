using System;
using UnityEngine;

public class InventorySlot
{
    public event EventHandler<SlotChangedEventArgs> SlotStateChanged;

    public class SlotChangedEventArgs : EventArgs
    {
        public int OwnerId { get; set; }
        public int ItemId { get; set; }
        public uint Quantity { get; set; }
    }

    private const int UndefinedId = -1;
    private readonly int _slotCapacity;
    private readonly int _owner;

    public int ItemId { get; private set; }
    public uint Quantity { get; private set; }

    private InventorySlot()
    {
    }

    public InventorySlot(int ownerId, int capacity)
    {
        _owner = ownerId;
        _slotCapacity = capacity;
        ItemId = UndefinedId;
        Quantity = 0;
    }

    public bool IsFull()
    {
        return Quantity == _slotCapacity; //Can add stack restriction
    }

    public bool IsEmpty()
    {
        return Quantity == 0;
    }

    public bool AddItem(int itemId, uint quantity)
    {
        /*Update model, if neccessary*/
        if (itemId == -1 || (ItemId != -1 && ItemId != itemId) || Quantity + quantity >= _slotCapacity)
        {
            return false;
        }

        ItemId = itemId;
        Quantity += quantity;

        Debug.LogFormat("#{2}: Item {0} added. Count of items {0} is {1}", ItemId, Quantity, _owner);

        /* Update view */
        SlotStateChanged(this, new SlotChangedEventArgs
        {
            ItemId = ItemId,
            OwnerId = _owner,
            Quantity = Quantity
        });

        return true;
    }

    public bool RemoveItem(int item, uint RemQuantity)
    {
        if (item == -1 || ItemId != item)
        {
            return false;
        }

        /*Update model, if neccessary*/
        Quantity -= RemQuantity;

        Debug.LogFormat("#{2}: Item {0} removed. Count of items {0} is {1}", ItemId, Quantity, _owner);

        SlotStateChanged(this, new SlotChangedEventArgs
        {
            ItemId = ItemId,
            OwnerId = 0,
            Quantity = Quantity
        });

        if (Quantity == 0)
        {
            ItemId = UndefinedId;
        }

        return true;
    }

    public override string ToString()
    {
        return string.Format("Item {0}, quantity {1} of {2}", ItemId, Quantity, _slotCapacity);
    }
}