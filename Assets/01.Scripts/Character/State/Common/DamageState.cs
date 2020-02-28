using System.Collections.Generic;
using UnityEngine;

public class DamageState : State
{
	sStatus _status;
	protected eAttackType _curDamagedType;

	public override void Update()
	{
		base.Update();
	}

	public override void Start()
	{
		base.Start();
		_status = _character.GetStatus();

		DamageProcess();
	}

	public override void Stop()
	{
		base.Stop();

		_character.ResetDamagedInfo();
	}

	public virtual void DamageProcess()
	{
		//데미지 계산방식(계산식은 나중에 수정할 예정)
		//1.회피
		//2.회피 실패시 데미지를 받는다. (방어력에 따라 감소된 데미지를 받음)
		float random = Random.Range(0, 10);
		if (random < _status.avoid)
		{
			Debug.Log("Avoid attack");
		}
		else
		{
			var damageInfo = _character.GetReceiveDamagedInfo();
			int receiveDamage = Mathf.RoundToInt((1 - (_status.armor / 100)) * damageInfo.attackPoint);
			_curDamagedType = damageInfo.attackType;
			_character.DecreaseHp(receiveDamage);
		}

		//상속받은 클래스에서는 데미지를 받았을 때 어떤 행동을 취할지 오버라이딩 해줘야함
	}
}
