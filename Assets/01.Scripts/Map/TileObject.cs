﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{
	[SerializeField]
	public List<string> _tileCellInfo = new List<string>();
	public List<string> _tileCellRangeInfo = new List<string>();

	private void Awake()
	{
		_objectType = eMapObjectType.TILEOBJECT;
	}

	sTileProperties _tileProperties;
	public sTileProperties GetProperties() { return _tileProperties; }
	public void SetTileProperties(float speed)
	{
		_tileProperties.speed = speed;
	}

	void check()
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
		//캐릭터 중심은  character.cs에서 처리하고 범위부분만 트리거로 처리해야할듯
		var type = collision.tag;
		if (type.Equals(eMapObjectType.PLAYER.ToString()))
		{
			var mapObject = collision.gameObject.GetComponentInParent<Player>();
			//캐릭터와 걸쳐있는 타일
			TileSystem.Instance.GetTileCell(_tileX, _tileY).AddObject(mapObject, eTileLayer.RANGE, false);
		}
		check();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var type = collision.tag;
		if (type.Equals(eMapObjectType.PLAYER.ToString()))
		{
			//걸쳐있는 타일 정보 제거
			var mapObject = collision.gameObject.GetComponentInParent<Player>();
			TileSystem.Instance.GetTileCell(_tileX, _tileY).RemoveObject(mapObject, eTileLayer.RANGE);
		}
		check();
	}
}
