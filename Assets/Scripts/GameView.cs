using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
	private const int InventoryPanelsCount = 2;
	private const int AddPerClick = 1;
	private const int RemPerClick = 1;
	
	private GameObject _inventoryPanelPrefab;
	private Transform _cachedTransform;
	private List<GameObject> _panels;
	
	// Use this for initialization
	void Start ()
	{
		_inventoryPanelPrefab = Resources.Load<GameObject>("Prefabs/InventoryArea");
		_cachedTransform = transform;
		_panels = new List<GameObject>(InventoryPanelsCount);
	}

	public void AddInventoryPanel(ISessionSettings settings, Inventory model)
	{
		if (_panels.Count >= InventoryPanelsCount) return;
		
		var panel = Instantiate(_inventoryPanelPrefab, _cachedTransform);
		panel.name = "Inventory#" + model.OwnerId;
		panel.GetComponent<Image>().color =
			model.OwnerId == settings.Enemy ? new Color(255, 0, 0, 0.2f) : new Color(0, 255, 0, 0.2f);
		var contentTransform = panel.transform.GetChild(0).GetChild(0);
		_panels.Add(panel);

		for (var i = 0; i < model.Capacity; ++i)
		{
			var item = ItemView.Create(model.Content[i], model.OwnerId);
			item.ivid = model.OwnerId * 1000 + i;
			item.transform.SetParent(contentTransform);
			item.ItemClicked += (sender, args) =>
			{
				var id = item.ItemId;
				if (args.OwnerId == settings.Enemy) //TODO: Very bad approach
				{
					settings.PlayerInventory.AddItem(id, AddPerClick);
					settings.EnemyInventory.RemoveItem(id, RemPerClick);
				}
				else
				{
					settings.PlayerInventory.RemoveItem(id, AddPerClick);
					settings.EnemyInventory.AddItem(id, RemPerClick);
				}
			};
			
		}
	}
}
