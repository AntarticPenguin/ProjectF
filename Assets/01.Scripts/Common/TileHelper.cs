using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct sTilePosition
{
	public int tileX;
	public int tileY;

	public sTilePosition(int x, int y)
	{
		tileX = x;
		tileY = y;
	}

	public override string ToString()
	{
		string str = "TileX: " + tileX + ", TileY: " + tileY;
		return str;
	}

	public override bool Equals(object obj)
	{
		if(obj is sTilePosition)
			return this.Equals((sTilePosition)obj);
		return false;
	}

	public bool Equals(sTilePosition tilePos)
	{
		return (tileX == tilePos.tileX) && (tileY == tilePos.tileY);
	}

	public override int GetHashCode()
	{
		return tileX ^ tileY;
	}

	public static bool operator ==(sTilePosition lhs, sTilePosition rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(sTilePosition lhs, sTilePosition rhs)
	{
		return !(lhs.Equals(rhs));
	}
}

public struct sTileProperties
{
	public float speed;

	public sTileProperties(float InSpeed)
	{
		speed = InSpeed;
	}
}

public static class TileHelper
{
	public static void GetNextTilePosByTileDirection(eTileDirection direction, ref sTilePosition tilePos)
	{
		switch (direction)
		{
			case eTileDirection.NORTH_WEST:
				tilePos.tileY++;
				break;
			case eTileDirection.NORTH_EAST:
				tilePos.tileX++;
				break;
			case eTileDirection.SOUTH_EAST:
				tilePos.tileY--;
				break;
			case eTileDirection.SOUTH_WEST:
				tilePos.tileX--;
				break;
			case eTileDirection.IN_TILE:
				break;
			default:
				break;
		}
	}

	public static void GetNextTilePosByDirection(eDirection direction, ref sTilePosition tilePos)
	{
		switch (direction)
		{
			case eDirection.NORTH:
				tilePos.tileX++;
				tilePos.tileY++;
				break;
			case eDirection.NORTH_EAST:
				tilePos.tileX++;
				break;
			case eDirection.EAST:
				tilePos.tileX++;
				tilePos.tileY--;
				break;
			case eDirection.SOUTH_EAST:
				tilePos.tileY--;
				break;
			case eDirection.SOUTH:
				tilePos.tileX--;
				tilePos.tileY--;
				break;
			case eDirection.SOUTH_WEST:
				tilePos.tileX--;
				break;
			case eDirection.WEST:
				tilePos.tileX--;
				tilePos.tileY++;
				break;
			case eDirection.NORTH_WEST:
				tilePos.tileY++;
				break;
			default:
				break;
		}
	}

	public static Vector2Int GetDirectionVector(TileCell from, TileCell to)
	{
		int tileX = to.GetTileX() - from.GetTileX();
		int tileY = to.GetTileY() - from.GetTileY();
		Vector2Int direction = new Vector2Int(tileX, tileY);
		return direction;
	}
}
