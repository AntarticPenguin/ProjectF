using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	Item _item = null;
	public Image _icon;
	public Text _countText;

	public void AddItem(Item newItem, int slotIndex)
	{
		_item = newItem;
		Inventory.Instance._itemsInfo[_item._itemName].slotIndex = slotIndex;

		_icon.name = _item.name;
		_icon.sprite = _item._icon;
		_icon.enabled = true;

		_countText.text = Inventory.Instance._itemsInfo[_item._itemName].count.ToString();
		_countText.enabled = true;
	}

	public void ClearSlot()
	{
		_item = null;

		_icon.sprite = null;
		_icon.enabled = false;

		_countText.enabled = false;
	}

	public void UpdateUI()
	{
		int count = Inventory.Instance._itemsInfo[_item._itemName].count;
		if (count > 0)
			_countText.text = count.ToString();
		else
		{
			Inventory.Instance._items.Remove(_item);
			ClearSlot();
		}
	}

	public Item GetItem() { return _item; }
}