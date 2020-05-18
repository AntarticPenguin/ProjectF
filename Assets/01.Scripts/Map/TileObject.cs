using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{
	[SerializeField]
	public List<string> _tileCellInfo = new List<string>();
	public List<string> _tileCellRangeInfo = new List<string>();

	private void Awake()
	{
		SetMapObjectType(eMapObjectType.TILEOBJECT);
	}

	private void Update()
	{
		//TEST
		if(Input.GetKeyDown(KeyCode.E))
		{
			check();
		}
	}

	sTileProperties _tileProperties;
	public sTileProperties GetProperties() { return _tileProperties; }
	public void SetTileProperties(float speed)
	{
		_tileProperties.speed = speed;
	}

	public void check()
	{
		_tileCellInfo.Clear();
		_tileCellRangeInfo.Clear();
		var list = TileSystem.Instance.GetTileCell(_tileX, _tileY)._mapObjectListByLayer[(int)eTileLayer.GROUND];
		var rangeList = TileSystem.Instance.GetTileCell(_tileX, _tileY)._mapObjectListByLayer[(int)eTileLayer.RANGE];
		for(int i = 0; i < list.Count; i++)
		{
			_tileCellInfo.Add(list[i].name);
		}
		for (int i = 0; i < rangeList.Count; i++)
		{
			_tileCellRangeInfo.Add(rangeList[i].name);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//TODO: 캐릭터와 일정 높이(offset)를 벗어나면 밟고 있지 않다는걸로 표기
		var mapObject = collision.gameObject.GetComponentInParent<Character>();
		if(mapObject)
		{
			var type = mapObject.GetMapObjectType();
			switch (type)
			{
				case eMapObjectType.PLAYER:
				case eMapObjectType.ENEMY:
					TileSystem.Instance.GetTileCell(_tileX, _tileY).AddObject(mapObject, eTileLayer.RANGE, false);
					break;
				default:
					break;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var mapObject = collision.gameObject.GetComponentInParent<Character>();
		if(mapObject)
		{
			var type = mapObject.GetMapObjectType();
			switch (type)
			{
				case eMapObjectType.PLAYER:
				case eMapObjectType.ENEMY:
					TileSystem.Instance.GetTileCell(_tileX, _tileY).RemoveObject(mapObject, eTileLayer.RANGE);
					break;
				default:
					break;
			}
		}
		
	}
}
