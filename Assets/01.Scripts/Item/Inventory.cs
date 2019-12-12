using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonMonobehavior<Inventory>
{
	public List<Item> _items = new List<Item>();
	public Dictionary<string, InventoryInfo> _itemsInfo = new Dictionary<string, InventoryInfo>();
	int _space;

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public override void Init()
	{
		gameObject.name = "Player Inventory";
		DontDestroyOnLoad(gameObject);

		_space = 24;
	}

	public bool AddItem(ItemObject itemObject)
	{
		if (_items.Count < _space)
		{
			var item = itemObject.GetItem();
			if(_itemsInfo.ContainsKey(item._itemName))
			{
				Debug.Log("Already Exist item: " + item._itemName);
				_itemsInfo[item._itemName].count++;
			}
			else
			{
				_items.Add(item);
				InventoryInfo newInfo = new InventoryInfo();
				newInfo.count = 1;
				newInfo.slotIndex = 0;
				_itemsInfo.Add(item._itemName, newInfo);
			}
			

			if (null != onItemChangedCallback)
				onItemChangedCallback.Invoke();

			return true;
		}

		return false;
	}

	private void Update()
	{
		
	}
}

//Info about item in inventory
public class InventoryInfo
{
	public int count;
	public int slotIndex;
}