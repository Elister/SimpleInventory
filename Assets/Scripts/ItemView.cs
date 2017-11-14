using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public class ItemClickedEventArgs : EventArgs
	{
		public int OwnerId { get; set; }
		public int SlotIndex { get; set; }
	}
	
	public event EventHandler<ItemClickedEventArgs> ItemClicked;
	
	private int ownerId;
	
	private Image  _itemPic;
	private Text   _itemNameQuantity;
	private Button _itemActionButton;

	void Awake()
	{
		ownerId = -1;
	}
	
	void Start ()
	{
		_itemPic = GetComponentInChildren<Image>();
		_itemNameQuantity = GetComponentInChildren<Text>();
		_itemActionButton = GetComponentInChildren<Button>();
		_itemActionButton.gameObject.SetActive(false);
		_itemActionButton.onClick.AddListener(() =>
		{
			var args = new ItemClickedEventArgs
			{
				OwnerId = ownerId,
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
			Debug.LogFormat("Event ItemClocked will be called: {0}", args);
#endif
			ItemClicked(this, args);
		});
		
		//gameObject.SetActive(false);
	}

	//Handling changing in model
	private void OnSlotStateChanged(object sender, InventorySlot.SlotChangedEventArgs args)
	{
		if (args.Quantity == 0)
		{
			_itemPic.sprite = null;
			_itemNameQuantity.text = "undefined";
			_itemActionButton.gameObject.SetActive(false);
			return;
		}

		if (Inventory.ItemsBase.ContainsKey(args.ItemId))
		{
			var itemDescription = Inventory.ItemsBase[args.ItemId];
			_itemPic.sprite = itemDescription.Pic;
			_itemNameQuantity.text = String.Format("{0} ({1})", itemDescription.Name, args.Quantity);
			/*TODO: Button possible state by the owner id*/
			_itemActionButton.image.color = ownerId == 0 ? Color.green : Color.red;
			ownerId = args.OwnerId;
			return;
		}
		
		Debug.LogErrorFormat("Error! ItemID {0} is not exists!", args.ItemId);

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_itemActionButton.gameObject.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_itemActionButton.gameObject.SetActive(false);
	}
}
