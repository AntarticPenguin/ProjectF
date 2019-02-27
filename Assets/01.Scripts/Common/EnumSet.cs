using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
	NONE,
	GROUND,
	ON_GROUND,
	MAXCOUNT,
}

public enum eTileDirection
{
	NORTH_WEST,
	NORTH_EAST,
	SOUTH_EAST,
	SOUTH_WEST,
	NONE,
}

public enum eMapObjectType
{
	NONE,
	TILEOBJECT,
	CHARACTER,
	PLAYER,
	ENEMY,
}

public enum eStateType
{
	NONE,
	IDLE,
	MOVE,
}

public struct sTilePosition
{
	public int _tileX;
	public int _tileY;

	public override string ToString()
	{
		string str = "TileX: " + _tileX + ", TileY: " + _tileY;
		return str;
	}
}

public struct sTileProperties
{
	public float _speed;

	public sTileProperties(float speed)
	{
		_speed = speed;
	}
}

public class EnumSet
{
 
}
