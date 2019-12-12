using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectSpawner : SingletonMonobehavior<MapObjectSpawner>
{
	public override void Init()
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
		position.z = -1.0f;
		characterObject.transform.position = position;
		characterObject.transform.localScale = new Vector2(2.0f, 2.0f);

		Character character = null;
		switch (scriptName)
		{
			case "Player":
				character = characterObject.AddComponent<Player>();
				characterObject.AddComponent<PlayerController>();
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
