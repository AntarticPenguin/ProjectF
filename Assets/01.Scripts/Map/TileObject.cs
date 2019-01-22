using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	int _tileX;
	int _tileY;

	public void SetTilePosition(int x, int y)
	{
		_tileX = x;
		_tileY = y;
	}
}
