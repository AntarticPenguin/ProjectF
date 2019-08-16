using UnityEngine;
using Cinemachine;

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

	CinemachineVirtualCamera _vcam;

	public void Init()
	{
		GameObject go = GameObject.FindGameObjectWithTag("FollowingCamera");
		_vcam = go.GetComponent<CinemachineVirtualCamera>();
	}

	public void BecomeViewer(MapObject mapObject)
	{
		_vcam.Follow = mapObject.transform;
	}

	TileMap _tileMap;

	public void SetMap(TileMap tileMap)
	{
		_tileMap = tileMap;
	}

	public TileMap GetMap()
	{
		return _tileMap;
	}

	public void LoadMap(sPortalInfo info)	
	{
		SavePlayerData();
		_tileMap.ClearMap();

		//string[] tokens = info.nextMap.Split('-');
		_tileMap.LoadMap(ref info);
	}

	void SavePlayerData()
	{
		//TODO: 맵 이동전 캐릭터 정보 저장
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
