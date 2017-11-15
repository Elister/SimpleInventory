using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISessionSettings
{
	int Player { get; }
	int Enemy { get; }
	int InventoryCapacity { get; }
	int ItemPerSlot { get; }
	Inventory PlayerInventory { get; }
	Inventory EnemyInventory { get; }
}
public class App : MonoBehaviour, ISessionSettings
{
	public int PlayerId = 0;
	public int EnemyId = 1;
	public int BackpackCapacity = 20;
	public int ItemsPerStack = 99;

	private void Awake()
	{
		Inventory.ItemsBase = new Dictionary<int, Item>
		{
			{0, new Item(0, "Топор Рока")},
			{1, new Item(1, "Щит Судьбы")},
			{2, new Item(2, "Шлем Огня")}
		};

	}

	// Use this for initialization
	void Start ()
	{
		EnemyInventory = new Inventory(EnemyId, this);
		PlayerInventory = new Inventory(PlayerId, this);
		
		EnemyInventory.AddItem(1, 3);
		EnemyInventory.AddItem(0, 2);
		EnemyInventory.AddItem(2, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
		{
			//Generate item
		}

		if (Input.GetButtonDown("Fire2"))
		{
			PlayerInventory.LogContent();
			EnemyInventory.LogContent();
		}
	}

	public int Player
	{
		get { return PlayerId; }
	}

	public int Enemy
	{
		get { return EnemyId; }
	}

	public int InventoryCapacity
	{
		get { return BackpackCapacity; }
	}

	public int ItemPerSlot
	{
		get { return ItemsPerStack; }
	}

	public Inventory PlayerInventory { get; private set; }
	public Inventory EnemyInventory { get; private set; }
}
