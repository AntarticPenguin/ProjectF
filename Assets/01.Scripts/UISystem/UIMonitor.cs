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
					Debug.Log("UIMONITOR::" + num + " is Pressed");
					UseSkill(num - 1);
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
	}

	#region INVENTORY

	public GameObject _inventoryWindow;
	public Transform _inventorySlotParent;
	Inventory _inventory;
	InventorySlot[] _inventorySlots;

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
		for (int i = 0; i < _inventorySlots.Length; i++)
		{
			if (i < _inventory._items.Count)
			{
				_inventorySlots[i].AddItem(_inventory._items[i]);
			}
			else
			{
				_inventorySlots[i].ClearSlot();
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
		else if (!_bInventoryOpen)
		{ 
			_statusWindow.SetActive(true);
			Character player = GameManager.Instance.GetPlayer();
			foreach(var key in _statusElements.Keys)
			{
				_statusElements[key].GetComponent<StatusUI>().UpdateInfo(key, player.GetStatus());
			}
			_bStatusOpen = true;
		}
	}
	
	void UpdateStatusUI()
	{

	}

	#endregion

	[Space(20)]
	#region Skill Slot
	public Transform _skillParent;
	SkillSlot[] _skillSlot;

	void InitSkillSlot()
	{
		_skillSlot = _skillParent.GetComponentsInChildren<SkillSlot>();
	}

	void UseSkill(int slotNumber)
	{
		_skillSlot[slotNumber].Use();
	}

	#endregion
}
