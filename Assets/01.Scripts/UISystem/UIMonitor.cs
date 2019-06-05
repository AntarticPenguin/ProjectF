using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMonitor : MonoBehaviour
{
	#region SINGLETON
	static UIMonitor _instance;
	public static UIMonitor Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance = FindObjectOfType<UIMonitor>();
				if (null == _instance)
				{
					GameObject go = new GameObject();
					go.name = "UIMonitor";
					_instance = go.AddComponent<UIMonitor>();
					DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
	}
	#endregion

	// Start is called before the first frame update
	void Start()
	{
		Init();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			OpenInventory();
		}
	}

	public void Init()
	{
		_inventory = Inventory.Instance;
		_inventory.onItemChangedCallback += UpdateUI;
		_slots = _slotParent.GetComponentsInChildren<InventorySlot>();
		_bInventoryOpen = false;
	}

	#region INVENTORY

	public GameObject _inventoryWindow;
	public Transform _slotParent;
	Inventory _inventory;
	InventorySlot[] _slots;

	bool _bInventoryOpen;

	public void OpenInventory()
	{
		if (_bInventoryOpen)
		{
			_inventoryWindow.SetActive(false);
			_bInventoryOpen = false;
		}
		else if (!_bInventoryOpen)
		{
			_inventoryWindow.SetActive(true);
			_bInventoryOpen = true;
		}
	}

	void UpdateUI()
	{
		Debug.Log("Update UI");
		for(int i = 0; i < _slots.Length; i++)
		{
			if(i < _inventory._items.Count)
			{
				_slots[i].AddItem(_inventory._items[i]);
			}
			else
			{
				_slots[i].ClearSlot();
			}
		}
	}

	#endregion
}
