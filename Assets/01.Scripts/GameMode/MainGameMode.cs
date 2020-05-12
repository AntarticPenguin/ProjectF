using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMode : SingletonMonobehavior<MainGameMode>
{
	[Header("Player Spawn Position")]
	public int _playerSpawnX;
	public int _playerSpawnY;

	[Header("Enemy Spawn Position")]
	public int _enemySpawnX;
	public int _enemySpawnY;

	[Header("Must Put Tile Size")]
	public int _width;
	public int _height;

	[Header("Stage Start: Wait Time")]
	public float _waitTime;

	public delegate void OnStageStart();
	public OnStageStart onStageStartCallback;

    // Update is called once per frame
    void Update()
    {
		MessageSystem.Instance.ProcessMessage();
    }

	public override void InitAwake()
	{
		GameManager.Instance.Init();

		TileSystem.Instance._width = _width;
		TileSystem.Instance._height = _height;
		TileSystem.Instance.Init();

		Character player = MapObjectSpawner.Instance.CreateCharacter(_playerSpawnX, _playerSpawnY, "Player", "Player_Preset01");
		player.Init();
		GameManager.Instance.SetPlayer(player);
		GameManager.Instance.BecomeViewer(player);

		StartCoroutine(StageStart(_waitTime));
	}

	IEnumerator StageStart(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		onStageStartCallback?.Invoke();
	}
}
