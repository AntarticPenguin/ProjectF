﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectSpawner : SingletonMonobehavior<MapObjectSpawner>
{
	Vector3 _groundTilemapTransform;

	public override void InitStart()
	{
		gameObject.name = "MapObjectSpanwer";
		DontDestroyOnLoad(gameObject);
	}

	GameObject GetPrefabByType(eMapObjectType type)
	{
		switch (type)
		{
			case eMapObjectType.ITEM:
				return Resources.Load<GameObject>("Prefabs/Item/ItemPrefab");
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

	public Character CreateCharacter(int tileX, int tileY, string scriptName, string resourceName)
	{
		TileSystem tileSystem = TileSystem.Instance;

		string filePath = "Prefabs/Character/" + resourceName;

		GameObject charPrefabs = Resources.Load<GameObject>(filePath);
		GameObject characterObject = Instantiate(charPrefabs);
		characterObject.InitTransformAsChild(tileSystem.GetTilemap(eTilemapType.GROUND).transform);

		Vector3 position = characterObject.transform.position;
		characterObject.transform.position = position;
		characterObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

		Character character = null;
		switch (scriptName)
		{
			case "Player":
				character = characterObject.AddComponent<Player>();
				character._playerController = characterObject.AddComponent<PlayerController>();
				character.name = "Player";
				break;
			case "Enemy":
				character = characterObject.AddComponent<Enemy>();
				character.name = "Enemy";
				break;
			default:
				break;
		}
		var spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>(true);
		foreach(var spriteRenderer in spriteRenderers)
		{
			spriteRenderer.sortingOrder = tileSystem.GetTileCell(tileX, tileY).GetGroundLayerOrder();
		}
		tileSystem.GetTileCell(tileX, tileY).SetObject(character, eTileLayer.GROUND);

		Debug.Log("SPAWN::Create " + character.name);
		return character;
	}

	public void SpawnObject(GameObject monsterPrefab, TileObject tileObject, MonsterSpawner spawner)
	{
		var tileSystem = TileSystem.Instance;

		GameObject monsterObj = Instantiate(monsterPrefab);
		monsterObj.InitTransformAsChild(tileSystem.GetTilemap(eTilemapType.GROUND).transform);

		Vector3 pos = monsterObj.transform.position;
		monsterObj.transform.position = pos;
		monsterObj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

		Enemy monster = monsterObj.AddComponent<Enemy>();
		monster.name = "Enemy";
		monster.Init();
		monster.SetSpawner(spawner);

		var spriteRenderers = monster.GetComponentsInChildren<SpriteRenderer>(true);
		foreach (var spriteRenderer in spriteRenderers)
		{
			spriteRenderer.sortingOrder = tileSystem.GetTileCell(tileObject.GetTilePosition()).GetGroundLayerOrder();
		}
		tileSystem.GetTileCell(tileObject.GetTilePosition()).SetObject(monster, eTileLayer.GROUND);
	}
}
