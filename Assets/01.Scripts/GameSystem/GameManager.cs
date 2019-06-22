using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
	#region SINGLETON
	static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			if(null == _instance)
			{
				_instance = new GameManager();
				_instance.Init();
			}
			return _instance;
		}
	}
	#endregion

	public void Init()
	{
		
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

	Character _player;
	public void SetPlayer(Character character)
	{
		_player = character;
	}

	public Character GetPlayer()
	{
		return _player;
	}
}
