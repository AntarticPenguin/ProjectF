
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
}
