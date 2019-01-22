using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
	//Singleton
	static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			if(null == _instance)
			{
				_instance = new GameManager();
			}
			return _instance;
		}
	}

	TileMap _curMap;

	public void SetMap(TileMap tileMap)
	{
		_curMap = tileMap;
	}

	public TileMap GetMap()
	{
		return _curMap;
	}
}
