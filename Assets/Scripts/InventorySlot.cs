using System;
using UnityEngine;

public class InventorySlot
{
    public class SlotChangedEventArgs : EventArgs
    {
        public int OwnerId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
    private const int SlotCapacity = 99;

    private int _id;
    private int _quantity;

    public int ItemId { get; private set; }

    //On get: returns item quantity; 
    //On set: checks if quantity value correct then change it to value (if not - set zero id)
    public int Quantity
    {
        get { return _quantity; }

        private set
        {
            if (value > -1 && value < SlotCapacity + 1)
            {
                _quantity = value;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarningFormat("Incorrect quantity value! ItemId is {0}, value is {1}", ItemId, value);
#endif
                _quantity = 0;
            }
        }
    }


    public InventorySlot()
    {
        ItemId = 0;
        Quantity = 0;
    }

    public bool IsFull()
    {
        return Quantity == SlotCapacity; //Can add stack restriction
    }

    public bool IsEmpty()
    {
        return Quantity == 0;
    }

    public void AddItem(int id)
    {
        if (IsEmpty())
        {
            ItemId = id;
            Quantity = 1;
        }
        else if (ItemId == id) //Maybe overchecking
        {
            Quantity++;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarningFormat("Incorrect call of AddItem. Try to add {0} but in slot is {1}", id, ItemId);
#endif
            return;
        }

#if UNITY_EDITOR
        Debug.LogFormat("Item {0} added. Count of items {0} is {1}", ItemId, Quantity);
#endif
    }

    public void RemoveItem()
    {
        if (!IsEmpty())
        {
            Quantity--;

#if UNITY_EDITOR
            Debug.LogFormat("Item {0} removed. Count of items {0} is {1}", ItemId, Quantity);
#endif

            if (Quantity == 0)
            {
                ItemId = 0;
#if UNITY_EDITOR
                Debug.LogFormat("Item erased.");
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarningFormat("Incorrect call of RemoveItem (empty slot)");
#endif
        }

#if UNITY_EDITOR
        Debug.LogFormat("Item {0} added. Count of items {0} is {1}", ItemId, Quantity);
#endif
    }
}