using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterSpawner : MonoBehaviour
{
	[Header("WAVE LIST")]
	[SerializeField]
	public List<SpawnInfoList> _spawnLists = new List<SpawnInfoList>();

	int _curWave;

	//다음 웨이브 요청 이벤트
	public delegate void OnSpawnNextWave();
	public OnSpawnNextWave onSpawnNextWaveCallback;

	//스폰 종료 이벤트
	public delegate void OnSpawnEnd();
	public OnSpawnEnd onSpawnEndCallback;

	// Start is called before the first frame update
	void Start()
    {
		_curWave = 0;
		MainGameMode.Instance.onStageStartCallback += Spawn;
		onSpawnNextWaveCallback += NextWave;
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

	void NextWave()
	{
		_curWave++;
		if (_curWave < _spawnLists.Count)
			Spawn();
		else
			onSpawnEndCallback?.Invoke();
	}

	IEnumerator SpawnSingle(SpawnInfoList spawnList, float interval)
	{
		var infoList = spawnList.list;
		for(int i = 0; i < infoList.Count; i++)
		{
			MapObjectSpawner.Instance.SpawnObject(infoList[i].monster, infoList[i].tilePos);
			yield return new WaitForSeconds(interval);
		}
	}

	IEnumerator SpawnPair(SpawnInfoList spawnList, float interval, int pairCount)
	{
		var infoList = spawnList.list;
		for(int i = 0; i < infoList.Count; i++)
		{
			MapObjectSpawner.Instance.SpawnObject(infoList[i].monster, infoList[i].tilePos);

			if ((i+1) % pairCount == 0)
				yield return new WaitForSeconds(interval);
		}
	}

	IEnumerator SpawnAll(SpawnInfoList spawnList)
	{
		var infoList = spawnList.list;
		for(int i = 0; i < infoList.Count; i++)
		{
			MapObjectSpawner.Instance.SpawnObject(infoList[i].monster, infoList[i].tilePos);
		}


		yield return null;
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
	public float interval;
	public int pairCount;

	public List<sSpawnInfo> list;
}