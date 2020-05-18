using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterSpawner : MonoBehaviour
{
	[Header("웨이브 간격")]
	[SerializeField]
	private float _waveInterval = 0.0f;

	[Header("WAVE LIST")]
	[SerializeField]
	private List<SpawnInfoList> _spawnLists = new List<SpawnInfoList>();

	int _curWave;
	int _remainEnemy;

	//다음 웨이브 요청 이벤트
	public delegate void OnSpawnNextWave();
	public OnSpawnNextWave onSpawnNextWaveCallback;

	//스폰 종료 이벤트
	public delegate void OnSpawnEnd();
	public OnSpawnEnd onSpawnEndCallback;

	//몬스터 KILL 이벤트
	public delegate void OnKillEnemy();
	public OnKillEnemy onKillEnemyCallback;

	// Start is called before the first frame update
	void Start()
    {
		_curWave = 0;
		_remainEnemy = 0;
		MainGameMode.Instance.onStageStartCallback += Spawn;
		onSpawnNextWaveCallback += NextWave;
		onSpawnEndCallback += MainGameMode.Instance.OpenPortal;
		onKillEnemyCallback += DecreaseCount;
    }

	void Spawn()
	{
		var spawnList = _spawnLists[_curWave];
		var spawnMode = spawnList.spawnMode;
		float interval = spawnList.interval;

		switch (spawnMode)
		{
			case eSpawnMode.SINGLE:
				StartCoroutine(SpawnSingle(spawnList, interval));
				break;
			case eSpawnMode.PAIR:
				StartCoroutine(SpawnPair(spawnList, interval, spawnList.pairCount));
				break;
			case eSpawnMode.ALL:
				StartCoroutine(SpawnAll(spawnList));
				break;
			default:
				break;
		}
	}

	IEnumerator SpawnSingle(SpawnInfoList info, float interval)
	{
		yield return new WaitForSeconds(_waveInterval);

		var wave = info.waveList;
		_remainEnemy = wave.Count;
		for (int i = 0; i < wave.Count; i++)
		{
			MapObjectSpawner.Instance.SpawnObject(wave[i].monster, wave[i].tilePos, this);
			yield return new WaitForSeconds(interval);
		}
	}

	IEnumerator SpawnPair(SpawnInfoList info, float interval, int pairCount)
	{
		yield return new WaitForSeconds(_waveInterval);

		var wave = info.waveList;
		_remainEnemy = wave.Count;
		for (int i = 0; i < wave.Count; i++)
		{
			MapObjectSpawner.Instance.SpawnObject(wave[i].monster, wave[i].tilePos, this);

			if ((i + 1) % pairCount == 0)
				yield return new WaitForSeconds(interval);
		}
	}

	IEnumerator SpawnAll(SpawnInfoList info)
	{
		yield return new WaitForSeconds(_waveInterval);

		var wave = info.waveList;
		_remainEnemy = wave.Count;
		for (int i = 0; i < wave.Count; i++)
		{
			MapObjectSpawner.Instance.SpawnObject(wave[i].monster, wave[i].tilePos, this);
		}
		yield return null;
	}

	void NextWave()
	{
		if(0 == _remainEnemy)
		{
			Debug.Log("Next wave Start!!");
			_curWave++;
			if (_curWave < _spawnLists.Count)
				Spawn();
			else
				onSpawnEndCallback?.Invoke();
		}
	}

	void DecreaseCount()
	{
		_remainEnemy--;
		onSpawnNextWaveCallback?.Invoke();
	}
}

//<스폰할 몬스터, 타일위치>
[Serializable]
public struct sSpawnInfo
{
	[SerializeField] public GameObject monster;
	[SerializeField] public TileObject tilePos;
}

public enum eSpawnMode
{
	SINGLE,
	PAIR,
	ALL,
}

[Serializable]
public class SpawnInfoList
{
	[Header("MODE")]
	public eSpawnMode spawnMode;

	[Header("소환 간격")]
	public float interval;
	public int pairCount;

	public List<sSpawnInfo> waveList;
}