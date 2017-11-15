using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISessionSettings
{
    int PlayerId { get; }
    int EnemyId { get; }
    int InventoryCapacity { get; }
    int ItemPerSlot { get; }
    Inventory PlayerInventory { get; }
    Inventory EnemyInventory { get; }
}

public class App : MonoBehaviour, ISessionSettings
{
    private ItemGenerator _gen;

    void Awake()
    {
        _gen = new ItemGenerator();
        PlayerId = 0;
        EnemyId = 1;
        InventoryCapacity = 25;
        ItemPerSlot = 1000;
    }

    // Use this for initialization
    void Start()
    {
        EnemyInventory = new Inventory(EnemyId, this);
        PlayerInventory = new Inventory(PlayerId, this);

        EnemyInventory.AddItem(_gen.GetRandomNewItem().Id, 5);
        EnemyInventory.AddItem(_gen.GetRandomNewItem().Id, 3);
        EnemyInventory.AddItem(_gen.GetRandomNewItem().Id, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Boom! New random stuff!");
            EnemyInventory.AddItem(_gen.GetRandomNewItem().Id, 3);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Boom! Boring old stuff!");
            EnemyInventory.AddItem(_gen.GetRandomOldItem().Id, 3);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            PlayerInventory.LogContent();
            EnemyInventory.LogContent();
        }
    }

    public int PlayerId { get; private set; }

    public int EnemyId { get; private set; }

    public int InventoryCapacity { get; private set; }

    public int ItemPerSlot { get; private set; }

    public Inventory PlayerInventory { get; private set; }
    public Inventory EnemyInventory { get; private set; }
}