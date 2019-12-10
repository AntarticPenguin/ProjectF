using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
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

	public void LoadMap(sPortalInfo info)	
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
