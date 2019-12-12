using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
	NONE,
	GROUND,
	ITEM,
	ON_GROUND,
	TRIGGER,
	BLOCK,
	MAXCOUNT,
}

public enum eTileDirection
{
	NORTH_WEST,
	NORTH_EAST,
	SOUTH_EAST,
	SOUTH_WEST,
	IN_TILE,
}

public enum eDirection
{
	NORTH,
	NORTH_EAST,
	EAST,
	SOUTH_EAST,
	SOUTH,
	SOUTH_WEST,
	WEST,
	NORTH_WEST,
}

public enum eMapObjectType
{
	NONE,
	TILEOBJECT,
	CHARACTER,
	PLAYER,
	ENEMY,
	ITEM,
	PORTAL,
}

public enum eStateType
{
	NONE,
	IDLE,
	MOVE,
	ATTACK,
	DAMAGE,
	CHASE,
	PATHFIND,
}

public enum eBoundary
{
	UP = 0,
	DOWN,
	LEFT,
	RIGHT,
	MAX,
}

enum eTileAsset
{
	Brick,
	Dirt,
	Grass,
	Lava,
	Sand,
	Snow,
	Stairs,
	Stone,
	Water,
	Wood,
	MAX,
}

public enum eTilemapType
{
	TRIGGER,
	GROUND,
	BLOCK,
}


public class EnumSet
{
 
}
