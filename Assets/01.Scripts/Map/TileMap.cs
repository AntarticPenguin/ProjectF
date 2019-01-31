﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		InitTiles();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public int _width = 5;
	public int _height = 5;
	float _xInterval = 0.545f;
	float _yInterval = 0.28f;
	float _xStartPos = 0.0f;

	TileCell[,] _tileCellList;

	public GameObject _tileObjectPrefab;

	void InitTiles()
	{
		_tileCellList = new TileCell[_width, _height];

		for(int y = 0; y < _height; ++y)
		{
			for(int x = 0; x < _width; ++x)
			{
				_tileCellList[y, x] = new TileCell();
			}
		}

		CreateTiles(eTileLayer.GROUND);
		//CreateTiles(eTileLayer.MIDDLE_GROUND);
	}

	void CreateTiles(eTileLayer layer)
	{
		int sortingOrder = _width * _height;

		for (int y = 0; y < _height; ++y)
		{
			_xStartPos = _xInterval * -y;
			for (int x = 0; x < _width; ++x)
			{
				GameObject tileGameObject = Instantiate(_tileObjectPrefab);
				tileGameObject.transform.SetParent(transform);
				tileGameObject.transform.localPosition = Vector3.zero;
				tileGameObject.transform.localScale = Vector3.one;

				TileObject tileObject = tileGameObject.GetComponent<TileObject>();
				tileObject.SetTilePosition(x, y);

				// x행, y열
				// y = 0.514x + (열 + 층수)
				float xPos = _xInterval * x + _xStartPos;
				float yPos = 0.514f * (xPos + _xInterval * (y + (int)layer)) + (_yInterval * (y + (int)layer));
				GetTileCell(x, y).Init(x, y);
				GetTileCell(x, y).SetPosition(new Vector2(xPos, yPos));
				GetTileCell(x, y).AddObject(tileObject, layer, sortingOrder);
				--sortingOrder;
			}
		}
	}

	public TileCell GetTileCell(int x, int y)
	{
		return _tileCellList[y, x];
	}
}
