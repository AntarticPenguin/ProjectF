using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	#region SINGLETON
	static ItemSpawner _instance;
	public static ItemSpawner Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance = FindObjectOfType<ItemSpawner>();
				if (null == _instance)
				{
					GameObject go = new GameObject();
					go.name = "ItemSpawner";
					_instance = go.AddComponent<ItemSpawner>();
					_instance.Init();
					DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
	}
	#endregion

	void Init()
	{

	}

	public void CreateItem(int tileX, int tileY, string itemName)
	{
		TileMap map = GameManager.Instance.GetMap();
		GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/ItemPrefab");
		GameObject go = Instantiate(itemPrefab);
		go.InitTransformAsChild(map.transform);
		ItemObject itemObject = go.AddComponent<ItemObject>();
		Item item = Resources.Load<Item>("Items/" + itemName);
		itemObject.Init(item);

		TileCell tileCell = map.GetTileCell(tileX, tileY);
		if(null != tileCell)
			tileCell.SetObject(itemObject, eTileLayer.ON_GROUND, 0);
	}
}
