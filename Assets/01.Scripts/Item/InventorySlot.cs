using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	Item _item = null;
	public Image _icon;
	public Text _countText;

	public void AddItem(Item newItem)
	{
		_item = newItem;

		_icon.name = _item.name;
		_icon.sprite = _item._icon;
		_icon.enabled = true;

		_countText.text = Inventory.Instance._itemsCount[_item._itemName].ToString();
		_countText.enabled = true;
	}

	public void ClearSlot()
	{
		_item = null;

		_icon.sprite = null;
		_icon.enabled = false;
	}

	public Item GetItem() { return _item; }
}