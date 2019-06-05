using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MapObject
{
	//맵상에 표시 혹은 올려지는 오브젝트를 위한 클래스
	Item _item;

	private void Awake()
	{
		_objectType = eMapObjectType.ITEM;
	}

	public void Init(Item item)
	{
		_item = item;
		gameObject.name = _item.name;
	}

	public Item GetItemInfo() { return _item; }
}
