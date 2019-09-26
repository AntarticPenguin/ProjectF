using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static Item _itemBeingDragged;
	Transform _startParent;
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		_itemBeingDragged = gameObject.GetComponentInParent<InventorySlot>().GetItem();
		_startParent = transform.parent;
		transform.SetParent(GameObject.FindGameObjectWithTag("UI Canvas").transform);
		GetComponent<Image>().raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;

		//GraphicRaycaster gr = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<GraphicRaycaster>();
		//List<RaycastResult> raycastResults = new List<RaycastResult>();
		//gr.Raycast(eventData, raycastResults);
		//if(raycastResults.Count != 0)
		//{
		//	var go = raycastResults[0].gameObject;
		//	Debug.Log(go.name);
		//}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.SetParent(_startParent);
		transform.localPosition = Vector3.zero;
		_itemBeingDragged = null;

		GetComponent<Image>().raycastTarget = true;
	}
}
