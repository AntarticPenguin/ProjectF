using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell
{
	Vector2 _position;
	int _tileX;
	int _tileY;

	float _width = 103.0f / 100;
	float _height = 49.6f / 100;

	public void Init(int tileX, int tileY)
	{
		_tileX = tileX;
		_tileY = tileY;
	}

	public void AddObject(MapObject mapObject, eTileLayer layer, int sortingOrder)
	{
		mapObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
		int sortingLayerID = SortingLayer.NameToID(layer.ToString());
		mapObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;

		mapObject.SetPosition(_position);
		mapObject.SetTilePosition(_tileX, _tileY);

		//float width = mapObject.GetComponent<SpriteRenderer>().bounds.size.x;
		//float height = mapObject.GetComponent<SpriteRenderer>().bounds.size.y;
		//Debug.Log("width: " + width + ", height: " + height);
	}

	public void SetPosition(Vector2 position)
	{
		_position = position;
	}

	public Vector2 GetPosition()
	{
		return _position;
	}

	public bool CheckOnTile(Vector2 charPosition)
	{
		Vector2 leftPoint = new Vector2(_position.x - _width / 2, _position.y);
		Vector2 downPoint = new Vector2(_position.x, _position.y - _height / 2);
		Vector3 upPoint = new Vector2(_position.x, _position.y + _height / 2);

		float downTan = (downPoint.y - leftPoint.y) / (downPoint.x - leftPoint.x);	//타일 아래변 두개 기울기
		float upTan = (upPoint.y - leftPoint.y) / (upPoint.x - leftPoint.x);		//타일 윗변 두개 기울기

		if (CheckUpsideSlope(downTan, downPoint, charPosition) == false)
			return false;
		if (CheckUpsideSlope(-downTan, downPoint, charPosition) == false)
			return false;
		if (CheckUpsideSlope(upTan, upPoint, charPosition) == true)
			return false;
		if (CheckUpsideSlope(-upTan, upPoint, charPosition) == true)
			return false;

		return true;
	}

	public eTileDirection CheckTileDirection(Vector2 charPosition)
	{
		Vector2 leftPoint = new Vector2(_position.x - _width / 2, _position.y);
		Vector2 downPoint = new Vector2(_position.x, _position.y - _height / 2);
		Vector3 upPoint = new Vector2(_position.x, _position.y + _height / 2);

		float downTan = (downPoint.y - leftPoint.y) / (downPoint.x - leftPoint.x);  //타일 아래변 두개 기울기
		float upTan = (upPoint.y - leftPoint.y) / (upPoint.x - leftPoint.x);        //타일 윗변 두개 기울기

		if (CheckUpsideSlope(downTan, downPoint, charPosition) == false)
			return eTileDirection.SOUTH_WEST;
		if (CheckUpsideSlope(-downTan, downPoint, charPosition) == false)
			return eTileDirection.SOUTH_EAST;
		if (CheckUpsideSlope(upTan, upPoint, charPosition) == true)
			return eTileDirection.NORTH_WEST;
		if (CheckUpsideSlope(-upTan, upPoint, charPosition) == true)
			return eTileDirection.NORTH_EAST;
		return eTileDirection.NONE;
	}

	bool CheckUpsideSlope(float slope, Vector2 onSlopePosition, Vector2 charPosition)
	{
		//직선식에 대입해서
		//0보다 작으면 위(true)
		//0보다 크면 아래(false)
		bool upSide = slope * (charPosition.x - onSlopePosition.x) - (charPosition.y - onSlopePosition.y) < 0;
		return upSide;
	}
}
