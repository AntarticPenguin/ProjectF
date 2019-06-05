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
		UIMonitor.Instance.Init();
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
