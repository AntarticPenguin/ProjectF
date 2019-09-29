using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	#region SINGLETON
	static Inventory _instance;
	public static Inventory Instance
	{
		get
		{
			if(null == _instance)
			{
				_instance = FindObjectOfType<Inventory>();
				if(null == _instance)
				{
					GameObject go = new GameObject();
					go.name = "Player Inventory";
					_instance = go.AddComponent<Inventory>();
					_instance.Init();
					DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
	}
	#endregion

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public List<Item> _items = new List<Item>();
	public Dictionary<string, int> _itemsCount = new Dictionary<string, int>();
	int _space;
	void Init()
	{
		_space = 24;
	}

	public bool AddItem(ItemObject itemObject)
	{
		if (_items.Count < _space)
		{
			var item = itemObject.GetItemInfo();
			if(_itemsCount.ContainsKey(item._itemName))
			{
				Debug.Log("Already Exist item: " + item._itemName);
				_itemsCount[item._itemName]++;
			}
			else
			{
				_items.Add(item);
				_itemsCount.Add(item._itemName, 1);
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
