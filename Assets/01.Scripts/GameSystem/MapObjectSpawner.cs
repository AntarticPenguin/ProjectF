﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectSpawner : MonoBehaviour
{
	#region SINGLETON
	static MapObjectSpawner _instance;
	public static MapObjectSpawner Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance = FindObjectOfType<MapObjectSpawner>();
				if (null == _instance)
				{
					GameObject go = new GameObject();
					go.name = "MapObjectSpawner";
					_instance = go.AddComponent<MapObjectSpawner>();
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

	GameObject GetPrefabByType(eMapObjectType type)
	{
		switch (type)
		{
			case eMapObjectType.ITEM:
				return Resources.Load<GameObject>("Prefabs/Item/ItemPrefab");
			case eMapObjectType.PORTAL:
				return Resources.Load<GameObject>("Prefabs/PortalTest");
			default:
				break;
		}
		return null;
	}

	public void CreateMapObject(int tileX, int tileY, eMapObjectType type, string name)
	{
		TileSystem tileSystem = TileSystem.Instance;
		GameObject prefab = GetPrefabByType(type);
		GameObject go = Instantiate(prefab);
		go.InitTransformAsChild(tileSystem.GetTilemap(eTilemapType.GROUND).transform);
		eTileLayer layer = eTileLayer.NONE;
		MapObject mapObject = null;

		switch (type)
		{
			case eMapObjectType.ITEM:
				ItemObject itemObject = go.AddComponent<ItemObject>();
				Item item = Resources.Load<Item>("Items/" + name);
				itemObject.Init(item);

				mapObject = itemObject;
				layer = eTileLayer.ITEM;
				break;
		}

		TileCell tileCell = tileSystem.GetTileCell(tileX, tileY);
		if (null != tileCell)
			tileCell.SetObject(mapObject, layer);
	}

	public void CreatePortal(sPortalInfo info)
	{
		TileSystem tileSystem = TileSystem.Instance;
		GameObject prefab = GetPrefabByType(eMapObjectType.PORTAL);
		GameObject go = Instantiate(prefab);
		go.InitTransformAsChild(tileSystem.GetTilemap(eTilemapType.GROUND).transform);
		eTileLayer layer = eTileLayer.ON_GROUND;

		PortalObject portalObject = go.AddComponent<PortalObject>();
		portalObject.name = info.portalName;
		portalObject.SetPortalInfo(info);

		TileCell tileCell = tileSystem.GetTileCell(info.tileX, info.tileY);
		if (null != tileCell)
			tileCell.SetObject(portalObject, layer);
	}

	public Character CreateCharacter(int tileX, int tileY, string scriptName, string resourceName)
	{
		TileSystem tileSystem = TileSystem.Instance;

		string filePath = "Prefabs/Character/" + resourceName;

		GameObject charPrefabs = Resources.Load<GameObject>(filePath);
		GameObject characterObject = Instantiate(charPrefabs);
		characterObject.InitTransformAsChild(tileSystem.GetTilemap(eTilemapType.GROUND).transform);

		Vector3 position = characterObject.transform.position;
		position.z = -1.0f;
		characterObject.transform.position = position;
		characterObject.transform.localScale = new Vector2(2.0f, 2.0f);

		Character character = null;
		switch (scriptName)
		{
			case "Player":
				character = characterObject.AddComponent<Player>();
				character.name = "Player";
				break;
			case "Enemy":
				character = characterObject.AddComponent<Enemy>();
				character.name = "Enemy";
				break;
			case "Pathfinder":
				character = characterObject.AddComponent<Pathfinder>();
				character.name = "Pathfinder";
				break;
			default:
				break;
		}
		character.GetComponent<SpriteRenderer>().sortingOrder = tileSystem.GetTileCell(tileX, tileY).GetGroundLayerOrder();
		tileSystem.GetTileCell(tileX, tileY).SetObject(character, eTileLayer.GROUND);

		return character;
	}
}
