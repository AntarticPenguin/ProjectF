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
		else if(Input.GetKeyDown(KeyCode.C))
		{
			OpenStatus();
		}
		else if(Input.anyKeyDown)
		{
			int num;
			if(int.TryParse(Input.inputString, out num))
			{
				if (1 <= num && num <= 6)
				{
					UseItem(num - 1);
				}
			}
		}
	}

	public void Init()
	{
		InitInventoryWindow();
		InitStatusWindow();
		InitSkillSlot();
	}

	void InitInventoryWindow()
	{
		_inventory = Inventory.Instance;
		_inventory.onItemChangedCallback += UpdateInventoryUI;
		_inventorySlots = _inventorySlotParent.GetComponentsInChildren<InventorySlot>();
		_bInventoryOpen = false;
	}

	void InitStatusWindow()
	{
		StatusUI[] list = _statusParentPanel.GetComponentsInChildren<StatusUI>();
		for (int i = 0; i < list.Length; i++)
		{
			if(null != list[i].gameObject.GetComponent<Slider>())
			{
				list[i].SetUIType(eStatusUIType.SLIDER);
			}
			else
			{
				list[i].SetUIType(eStatusUIType.TEXT);
			}
			_statusElements.Add(list[i].name, list[i].gameObject);
		}
		_bStatusOpen = false;
		GameManager.Instance.GetPlayer().onHpChangedCallback += UpdateStatusUI;
	}

	#region INVENTORY

	public GameObject _inventoryWindow;
	public Transform _inventorySlotParent;
	Inventory _inventory;
	public InventorySlot[] _inventorySlots;

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

	void UpdateInventoryUI()
	{
		for (int index = 0; index < _inventorySlots.Length; index++)
		{
			if (index < _inventory._items.Count)
			{
				_inventorySlots[index].AddItem(_inventory._items[index], index);
			}
			else
			{
				_inventorySlots[index].ClearSlot();
			}
		}
	}

	#endregion

	[Space(20)]
	#region STATUS

	public GameObject _statusWindow;
	public Transform _statusParentPanel;
	public Dictionary<string, GameObject> _statusElements = new Dictionary<string, GameObject>();

	bool _bStatusOpen;

	public void OpenStatus()
	{
		if (_bStatusOpen)
		{
			_statusWindow.SetActive(false);
			_bStatusOpen = false;
		}
		else if (!_bStatusOpen)
		{ 
			_statusWindow.SetActive(true);
			UpdateStatusUI();
			_bStatusOpen = true;
		}
	}
	
	void UpdateStatusUI()
	{
		Character player = GameManager.Instance.GetPlayer();
		foreach (var key in _statusElements.Keys)
		{
			_statusElements[key].GetComponent<StatusUI>().UpdateInfo(key, player.GetStatus());
		}
	}

	#endregion

	[Space(20)]
	#region Quick Item Slot
	public Transform _itemSlotParent;
	public ItemQuickSlot[] _itemQuickSlots;

	void InitSkillSlot()
	{
		_itemQuickSlots = _itemSlotParent.GetComponentsInChildren<ItemQuickSlot>();
	}

	void UseItem(int slotNumber)
	{
		_itemQuickSlots[slotNumber].Use();
	}

	#endregion
}
