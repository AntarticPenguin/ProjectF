using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	Item _item = null;
	public Image _icon;

	public void AddItem(Item newItem)
	{
		_item = newItem;

		_icon.name = _item.name;
		_icon.sprite = _item.icon;
		_icon.enabled = true;
	}

	public void ClearSlot()
	{
		_item = null;

		_icon.sprite = null;
		_icon.enabled = false;
	}

	public Item GetItem() { return _item; }
}