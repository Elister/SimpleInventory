using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public int ivid;
	private static ItemView prefab;
	private static string prefabPath = "Prefabs/ItemView";
	
	public static ItemView Create(InventorySlot model, int ownerId)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<GameObject>(prefabPath).GetComponent<ItemView>();
			if (prefab == null)
			{
				Debug.LogError("Cannot load ItemView prefab!");
				return null;
			}
		}

		var createdObject = Instantiate(prefab);
		createdObject._ownerId = ownerId;
		model.SlotStateChanged += createdObject.OnSlotStateChanged;

		return createdObject;
	}
	
	public class ItemClickedEventArgs : EventArgs
	{
		public int OwnerId { get; set; }
		public int ItemId { get; set; }
		public int SlotIndex { get; set; }
	}
	
	public event EventHandler<ItemClickedEventArgs> ItemClicked;
	
	private int _ownerId;
	
	public int ItemId { get; private set; }
	
	public Image  ItemPic;
	public Text   ItemNameQuantity;
	public Button ItemActionButton;

	void Awake()
	{
		_ownerId = -1;
		ItemId = -1;
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
				#if UNITY_EDITOR
				Debug.LogWarning("No listener for ItemClicked event!");
				#endif
				return;
			}
			
#if UNITY_EDITOR
			Debug.LogFormat("Event ItemClocked will be called, IVId {0}", ivid);
#endif
			ItemClicked(this, args);
		});
		
		//gameObject.SetActive(false);
	}

	//Handling changing in model
	private void OnSlotStateChanged(object sender, InventorySlot.SlotChangedEventArgs args)
	{
		/*Remove item fully*/
		if (args.Quantity == 0)
		{
			ItemId = -1;
			//gameObject.SetActive(false);
			ItemPic.sprite = null;
			ItemNameQuantity.text = "undefined";
			ItemActionButton.gameObject.SetActive(false);
			return;
		}

		
		if (Inventory.ItemsBase.ContainsKey(args.ItemId))
		{
			//gameObject.SetActive(true);
			var itemDescription = Inventory.ItemsBase[args.ItemId];
			ItemId = args.ItemId;
			ItemPic.sprite = itemDescription.Pic;
			ItemNameQuantity.text = String.Format("{0} ({1})", itemDescription.Name, args.Quantity);
			ItemActionButton.image.color = _ownerId == 0 ? Color.green : Color.red;
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
