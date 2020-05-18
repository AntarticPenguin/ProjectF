using UnityEngine;

public class DeadState : State
{
	public override void Start()
	{
		base.Start();

		_character.GetAnimPlayer().Play("DEAD", null, null,
		() =>
		{
			_character.Kill();
		});
	}
}
