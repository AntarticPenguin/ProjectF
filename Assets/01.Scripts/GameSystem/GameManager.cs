using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	CinemachineVirtualCamera _vcam;
	LevelLoader _levelLoader;

	public void Init()
	{
		GameObject go = GameObject.FindGameObjectWithTag("FollowingCamera");
		_vcam = go.GetComponent<CinemachineVirtualCamera>();

		var levelLoaderObj = GameObject.Find("LevelLoader");

		if (levelLoaderObj)
		{
			_levelLoader = levelLoaderObj.GetComponent<LevelLoader>();
			Debug.Log("Find LevelLoader");
		}
	}

	public void BecomeViewer(MapObject mapObject)
	{
		_vcam.Follow = mapObject.transform;
	}

	void SavePlayerData()
	{
		//TODO: 맵 이동전 캐릭터 정보 저장
	}

	public void LoadMap(string sceneName)
	{
		SavePlayerData();
		_levelLoader.LoadLevel(sceneName);
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
