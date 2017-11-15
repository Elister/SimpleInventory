using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using Random = System.Random;

public class ItemGenerator
{
    struct Combination
    {
        public ItemType type;
        public int item;
        public int feature;
        public int sprite;
    }

    enum ItemType
    {
        Axe = 0,
        Boots = 1,
        Armor = 2,
        Gaunlet = 3,
        Hammer = 4,
        Helmet = 5,
        Shield = 6
    }

    public static Dictionary<int, Item> ItemsBase;

    public static readonly List<string> CharNames = new List<string>
    {
        "Уныния",
        "Боли",
        "Рока",
        "Силы",
        "Джа",
        "Судьбы",
        "Мести"
    };

    private readonly Dictionary<ItemType, List<string>> ItemNames = new Dictionary<ItemType, List<string>>
    {
        {ItemType.Axe, new List<string> {"Топор", "Тесак", "Секира"}},
        {ItemType.Boots, new List<string> {"Сапоги", "Ботинки"}},
        {ItemType.Armor, new List<string> {"Броня", "Нагрудник", "Кираса"}},
        {ItemType.Gaunlet, new List<string> {"Перчатки"}},
        {ItemType.Hammer, new List<string> {"Молот", "Кувалда", "Ручник"}},
        {ItemType.Helmet, new List<string> {"Шлем", "Шапка", "Каска"}},
        {ItemType.Shield, new List<string> {"Щит"}}
    };

    private readonly Dictionary<int, List<Sprite>> ItemSprites;

    private List<Combination> Combinations;

    private Random Randomizer;

    public ItemGenerator()
    {
        if (ItemsBase == null)
        {
            ItemsBase = new Dictionary<int, Item>();
        }

        Randomizer = new Randomizer();
        ItemSprites = new Dictionary<int, List<Sprite>>();
        var typesCount = Enum.GetNames(typeof(ItemType)).Length;

        for (int i = 0; i < typesCount; ++i)
        {
            ItemSprites.Add(i, new List<Sprite>());
        }
        
        /*Load sprites*/
        Sprite[] items = Resources.LoadAll<Sprite>("Sprites/Items");
        foreach (var sprite in items)
        {
            var spriteName = sprite.name;
            if (spriteName.StartsWith("Axe"))
            {
                ItemSprites[(int) ItemType.Axe].Add(sprite);
            }
            else if (spriteName.StartsWith("Boots"))
            {
                ItemSprites[(int) ItemType.Boots].Add(sprite);
            }
            else if (spriteName.StartsWith("Chest"))
            {
                ItemSprites[(int) ItemType.Armor].Add(sprite);
            }
            else if (spriteName.StartsWith("Gaunlet"))
            {
                ItemSprites[(int) ItemType.Gaunlet].Add(sprite);
            }
            else if (spriteName.StartsWith("Hammer"))
            {
                ItemSprites[(int) ItemType.Hammer].Add(sprite);
            }
            else if (spriteName.StartsWith("Helmet"))
            {
                ItemSprites[(int) ItemType.Helmet].Add(sprite);
            }
            else if (spriteName.StartsWith("Shield"))
            {
                ItemSprites[(int) ItemType.Shield].Add(sprite);
            }
        }
        
        /*generate combinations*/
        Combinations = new List<Combination>();
        for (var i = 0; i < typesCount; ++i) //Item name
        {
            for (var j = 0; j < ItemNames[(ItemType) i].Count; ++j) //Variant item name
            {
                for (var k = 0; k < ItemSprites[i].Count; ++k)
                {
                    for (var l = 0; l < CharNames.Count; ++l)
                    {
                        Combination value;
                        value.item = j;
                        value.feature = l;
                        value.type = (ItemType) i;
                        value.sprite = k;
                        Combinations.Add(value);
                    }
                }
            }
        }
    }

    public Item GetRandomNewItem()
    {
        var index = Randomizer.Next(0, Combinations.Count);
        var choosen = Combinations[index];

        var value = new Item(ItemsBase.Count,
            ItemNames[choosen.type][choosen.item] + " " + CharNames[choosen.feature],
            ItemSprites[(int)choosen.type][choosen.sprite]);

        ItemsBase.Add(value.Id, value);
        return value;
    }

    public Item GetRandomOldItem()
    {
        return ItemsBase[Randomizer.Next(0, ItemsBase.Count)];
    }
}