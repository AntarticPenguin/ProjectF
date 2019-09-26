using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
	int _space;
	void Init()
	{
		_space = 24;
	}

	public bool AddItem(ItemObject itemObject)
	{
		if(_items.Count < _space)
		{
			_items.Add(itemObject.GetItemInfo());

			if(null != onItemChangedCallback)
				onItemChangedCallback.Invoke();

			return true;
		}

		return false;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			for (int i = 0; i < _items.Count; i++)
				Debug.Log(_items[i].name);
		}
	}
}
