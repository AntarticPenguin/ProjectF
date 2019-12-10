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

	//타일좌표 뺀 값을 방향벡터로 변환
	public static Vector2Int GetDirectionVector(TileCell from, TileCell to)
	{
		int x = to.GetTileX() - from.GetTileX();
		int y = to.GetTileY() - from.GetTileY();

		if (x > 1) x = 1;
		if (x < -1) x = -1;
		if (y > 1) y = 1;
		if (y < -1) y = -1;

		Vector2Int direction = Vector2Int.zero;
		if (x == 1 && y == 1)		//NORTH(0,1)
		{
			direction.x = 0;
			direction.y = 1;
		}
		else if(x == -1 && y == -1) //SOUTH(0,-1)
		{
			direction.x = 0;
			direction.y = -1;
		}
		else if(x == 1 && y == -1)  //EAST(1,0)
		{
			direction.x = 1;
			direction.y = 0;
		}
		else if (x == -1 && y == 1)  //WEST(-1,0)
		{
			direction.x = -1;
			direction.y = 0;
		}
		else if (x == 1 && y == 0)  //NORTH_EAST(1,1)
		{
			direction.x = 1;
			direction.y = 1;
		}
		else if (x == 0 && y == 1)  //NORTH_WEST(-1,1)
		{
			direction.x = -1;
			direction.y = 1;
		}
		else if (x == 0 && y == -1)  //SOUTH_EAST(1,-1)
		{
			direction.x = 1;
			direction.y = -1;
		}
		else if (x == -1 && y == 0)  //SOUTH_WEST(-1,-1)
		{
			direction.x = -1;
			direction.y = -1;
		}

		return direction;
	}

	public static Vector2 GetSlopeDirection(Vector2 direction)
	{
		TileSystem tileSystem = TileSystem.Instance;
		Vector2 newDirection = direction;
		if(direction.y == -1 && (direction.x == 1 || direction.x == -1))
		{
			//SOUTH_EAST, SOUTH_WEST
			newDirection.y = -tileSystem.GetSlope();
		}
		else if(direction.y == 1 && (direction.x == 1 || direction.x == -1))
		{
			//NORTH_EAST, NORTH_WEST
			newDirection.y = tileSystem.GetSlope();
		}
		return newDirection;
	}

	public static eDirection ConvertToeDirection(Vector2Int direction)
	{
		if (direction.x == 0 && direction.y == 1)
			return eDirection.NORTH;
		if (direction.x == 0 && direction.y == -1)
			return eDirection.SOUTH;
		if (direction.x == -1 && direction.y == 0)
			return eDirection.WEST;
		if (direction.x == 1 && direction.y == 0)
			return eDirection.EAST;
		if (direction.x == 1 && direction.y == 1)
			return eDirection.NORTH_EAST;
		if (direction.x == -1 && direction.y == 1)
			return eDirection.NORTH_WEST;
		if (direction.x == 1 && direction.y == -1)
			return eDirection.SOUTH_EAST;
		if (direction.x == -1 && direction.y == -1)
			return eDirection.SOUTH_WEST;

		return eDirection.NORTH;
	}
}
