﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
	private void Awake()
	{
		_objectType = eMapObjectType.NONE;
		_currentLayer = eTileLayer.NONE;
	}

	protected Vector2 _position;
	protected int _tileX;
	protected int _tileY;
	sTilePosition _tilePosition;
	protected eMapObjectType _objectType;

	public void SetPosition(Vector2 position)
	{
		_position = position;
		transform.position = _position;
	}

	public void SetTilePosition(int tileX, int tileY)
	{
		_tileX = tileX;
		_tileY = tileY;
		if(null == _tilePosition)
		{
			_tilePosition = new sTilePosition(_tileX, _tileY);
		}
		else
		{
			_tilePosition.tileX = _tileX;
			_tilePosition.tileY = _tileY;
		}
	}

	public int GetTileX() { return _tileX; }
	public int GetTileY() { return _tileY; }
	public sTilePosition GetTilePosition()
	{
		return _tilePosition;
	}

	public TileCell GetCurrentTileCell()
	{
		return TileSystem.Instance.GetTileCell(_tileX, _tileY);
	}

	public eMapObjectType GetMapObjectType() { return _objectType; }
	public void SetMapObjectType(eMapObjectType type)
	{
		_objectType = type;
		tag = type.ToString();
		foreach (Transform child in transform)
		{
			child.tag = tag;
		}
	}

	protected eTileLayer _currentLayer;
	public eTileLayer GetCurrentLayer() { return _currentLayer; }
	public void SetCurrentLayer(eTileLayer layer)
	{
		_currentLayer = layer;
	}

	#region Message

	public virtual void ReceiveMessage(MessageParam msgParam)
	{
		//TODO: 메세지 처리
	}

	#endregion
}
