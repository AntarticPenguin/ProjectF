using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageState : DamageState
{
	public override void DamageProcess()
	{
		base.DamageProcess();

		//데미지 계산 이후 공격타입에 따른 행동
		switch (_curDamagedType)
		{
			case eAttackType.NORMAL:
				break;
			case eAttackType.STUN:      //스턴
				_nextState = eStateType.IDLE;
				break;
			default:
				break;
		}
	}
}
