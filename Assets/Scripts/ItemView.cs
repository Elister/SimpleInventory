using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public class ItemClickedEventArgs : EventArgs
	{
		public int OwnerId { get; set; }
		public int ItemId { get; set; }
		public int SlotIndex { get; set; }
	}
	
	public event EventHandler<ItemClickedEventArgs> ItemClicked;
	
	private const string PrefabPath = "Prefabs/ItemView";
	private const string ButtonsStatesPath = "Sprites/Icons";
	
	private int _ownerId;
	private static ItemView _prefab;
	private static Sprite[] buttonStates;
	
	public int ItemId { get; private set; }
	
	public Image  ItemPic;
	public Text   ItemNameQuantity;
	public Button ItemActionButton;

	public static ItemView Create(InventorySlot model, int ownerId)
	{
		if (_prefab == null)
		{
			_prefab = Resources.Load<GameObject>(PrefabPath).GetComponent<ItemView>();
			if (_prefab == null)
			{
				Debug.LogError("Cannot load ItemView prefab!");
				return null;
			}
		}
		
		if (buttonStates == null)
		{
			buttonStates = Resources.LoadAll<Sprite>(ButtonsStatesPath);
			if (buttonStates == null)
			{
				Debug.LogError("Cannot load ItemView button states sprites!");
				return null;
			}
		}

		var createdObject = Instantiate(_prefab);
		createdObject._ownerId = ownerId;
		model.SlotStateChanged += createdObject.OnSlotStateChanged;
		createdObject.ItemActionButton.image.sprite = ownerId == 0 ? buttonStates[1] : buttonStates[0]; //TODO: Change!
		return createdObject;
	}

	
	void Awake()
	{
		_ownerId = -1;
		ItemId = -1;

		buttonStates = Resources.LoadAll<Sprite>("Sprites/Icons");

		gameObject.SetActive(false);
	}
	
	void Start ()
	{
		ItemActionButton.gameObject.SetActive(false);
		ItemActionButton.onClick.AddListener(() =>
		{
			var args = new ItemClickedEventArgs
			{
				OwnerId = _ownerId,
				ItemId = ItemId,
				SlotIndex = transform.GetSiblingIndex()
			};

			if (ItemClicked == null)
			{
				Debug.LogWarning("No listener for ItemClicked event!");
				return;
			}
			
			ItemClicked(this, args);
		});
	}

	//Handling changing in model
	private void OnSlotStateChanged(object sender, InventorySlot.SlotChangedEventArgs args)
	{
		/*Remove item fully*/
		if (args.Quantity == 0)
		{
			gameObject.SetActive(false);
			ItemId = -1;
			ItemPic.sprite = null;
			ItemNameQuantity.text = "undefined";
			ItemActionButton.gameObject.SetActive(false);
			return;
		}

		
		if (ItemGenerator.ItemsBase.ContainsKey(args.ItemId))
		{
			gameObject.SetActive(true);
			var itemDescription = ItemGenerator.ItemsBase[args.ItemId];
			ItemId = args.ItemId;
			ItemPic.sprite = itemDescription.Pic;
			ItemNameQuantity.text = string.Format("{0} ({1})", itemDescription.Name, args.Quantity);
			return;
		}
		
		Debug.LogErrorFormat("Error! ItemID {0} is not exists!", args.ItemId);

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ItemActionButton.gameObject.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ItemActionButton.gameObject.SetActive(false);
	}
}
