using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemQuickSlot : MonoBehaviour, IDropHandler
{
	public Image _icon;
	public Text _countText;
	Item _slottedItem;

	public void OnDrop(PointerEventData eventData)
	{
		var item = ItemDragHandler._itemBeingDragged;
		if (item != null)
		{
			CheckDuplicatedItem(item);

			_slottedItem = item;
			_icon.sprite = item._icon;
			_icon.enabled = true;

			_countText.text = Inventory.Instance._itemsInfo[_slottedItem._itemName].count.ToString();
			_countText.enabled = true;

			Inventory.Instance.onItemChangedCallback += UpdateUI;
		}
	}

	void Clear()
	{
		_slottedItem = null;
		_icon.sprite = null;
		_icon.enabled = false;
		_countText.enabled = false;
	}

	void CheckDuplicatedItem(Item item)
	{
		var quickSlots = UIMonitor.Instance._itemQuickSlots;
		for (int i = 0; i < quickSlots.Length; i++)
		{
			var slottedItem = quickSlots[i]._slottedItem;
			if (slottedItem != null &&
				slottedItem._itemName.Equals(item._itemName))
			{
				quickSlots[i].Clear();
				break;
			}
		}
	}

	void UpdateUI()
	{
		_countText.text = Inventory.Instance._itemsInfo[_slottedItem._itemName].count.ToString();
		_icon.color = Color.white;
	}

	public void Use()
	{
		if(_slottedItem != null)
		{
			if (Inventory.Instance._itemsInfo.ContainsKey(_slottedItem._itemName) == false)
			{
				Debug.Log("Not enough Item in Inventory");
				return;
			}
				

			if(Inventory.Instance._itemsInfo[_slottedItem._itemName].count > 0)
			{
				_slottedItem.Use(GameManager.Instance.GetPlayer());
				Inventory.Instance._itemsInfo[_slottedItem._itemName].count--;

				int inventoryIndex = Inventory.Instance._itemsInfo[_slottedItem._itemName].slotIndex;
				UIMonitor.Instance._inventorySlots[inventoryIndex].UpdateUI();
				UpdateUI();
			}
				

			if(Inventory.Instance._itemsInfo[_slottedItem._itemName].count == 0)
			{
				_icon.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
				Inventory.Instance._itemsInfo.Remove(_slottedItem._itemName);
			}
		}
	}
}
