using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IDropHandler
{
	public Image _icon;
	public void OnDrop(PointerEventData eventData)
	{
		var item = ItemDragHandler._itemBeingDragged;
		if (item != null)
		{
			_icon.sprite = item.icon;
			_icon.enabled = true;
		}
	}
}
