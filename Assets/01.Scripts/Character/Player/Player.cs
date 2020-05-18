using UnityEngine;

public class Player : Character
{
	private void Awake()
	{
		SetMapObjectType(eMapObjectType.PLAYER);
	}

	public override void InitState()
	{
		base.InitState();

		ReplaceState(eStateType.DAMAGE, new PlayerDamageState());
		_curState = _stateMap[eStateType.IDLE];
	}

	public override void InitStatus()
	{
		base.InitStatus();

		_status.maxHp = 10000;
		_status.hp = 10000;

		_status.attack = 10000;
		_attackCoolTime = 0.0f;
	}
}
