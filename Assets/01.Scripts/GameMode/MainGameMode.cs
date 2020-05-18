using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMode : SingletonMonobehavior<MainGameMode>
{
	[Header("Player Spawn Position")]
	public int _playerSpawnX;
	public int _playerSpawnY;

	[Header("Must Put Tilemap Size")]
	public int _width;
	public int _height;

	GameObject _portalObject;

	public delegate void OnStageStart();
	public OnStageStart onStageStartCallback;

    // Update is called once per frame
    void Update()
    {
		MessageSystem.Instance.ProcessMessage();
    }

	public override void InitAwake()
	{
		Debug.Log("Gamemode awake");
		GameManager.Instance.Init();

		TileSystem.Instance._width = _width;
		TileSystem.Instance._height = _height;
		TileSystem.Instance.Init();

		Character player = MapObjectSpawner.Instance.CreateCharacter(_playerSpawnX, _playerSpawnY, "Player", "Player_Preset01");
		player.Init();
		GameManager.Instance.SetPlayer(player);
		GameManager.Instance.BecomeViewer(player);

		_portalObject = GameObject.FindGameObjectWithTag(eMapObjectType.PORTAL.ToString());
		_portalObject.SetActive(false);
	}

	public override void InitStart()
	{
		onStageStartCallback?.Invoke();
	}

	public void OpenPortal()
	{
		_portalObject.SetActive(true);
	}
}
