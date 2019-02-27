using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{
	// Start is called before the first frame update
	void Start()
	{
		_objectType = eMapObjectType.TILEOBJECT;
	}

	// Update is called once per frame
	void Update()
	{

	}

	sTileProperties _tileProperties;
	public sTileProperties GetProperties() { return _tileProperties; }
	public void SetTileProperties(float speed)
	{
		_tileProperties._speed = speed;
	}
}
