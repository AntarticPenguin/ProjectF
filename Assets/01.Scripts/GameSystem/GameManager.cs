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

		_levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

		if (_levelLoader)
			Debug.Log("Find LevelLoader");
	}

	public void BecomeViewer(MapObject mapObject)
	{
		_vcam.Follow = mapObject.transform;
	}

	public void LoadMap(string sceneName)
	{
		SavePlayerData();
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
