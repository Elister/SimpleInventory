using System;
using UnityEngine;

public class Item
{
    private int _id;

    public int Id
    {
        get { return _id; }

        private set
        {
            if (value > -1 && value < 256) //Better with MAX_ID constant
            {
                _id = value;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarningFormat("Incorrect item id value! ItemId is {0}, value is {1}", _id, value);
#endif
                _id = 0;
            }
        }
    }

    public string Name { get; set; }

    public Sprite Pic { get; set; }
    
    public Item()
    {
        _id = -1;
        Name = "Undefined item";
        Pic = null;
    }

    public Item(int id, string name, Sprite pic)
    {
        Id = id;
        Name = name;
        Pic = pic;
    }
}