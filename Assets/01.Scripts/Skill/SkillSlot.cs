using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IDropHandler
{
	public Image _icon;
	public Text _countText;
	Item _slottedItem;
	public void OnDrop(PointerEventData eventData)
	{
		var item = ItemDragHandler._itemBeingDragged;
		if (item != null)
		{
			_slottedItem = item;
			_icon.sprite = item._icon;
			_icon.enabled = true;

			_countText.text = Inventory.Instance._itemsCount[_slottedItem._itemName].ToString();
			_countText.enabled = true;
			//_icon.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
		}
	}

	public void Use()
	{
		if(_slottedItem != null)
		{
			_slottedItem.Use();
		}
		else
		{
			Debug.Log("SkillSlot::Use(): empty slot");
		}
	}
}
