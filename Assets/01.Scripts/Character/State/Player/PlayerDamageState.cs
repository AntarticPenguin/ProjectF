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
				_nextState = eStateType.IDLE;
				break;
			case eAttackType.STUN:      //스턴
				_character.GetAnimPlayer().Play("DAMAGE", null, null, 
					()=>
					{
						//endEvent
						_character.GetAnimator().SetTrigger(_character.LookAt().ToString());
						_nextState = eStateType.IDLE;
					});
				break;
			default:
				_nextState = eStateType.IDLE;
				break;
		}
	}
}
