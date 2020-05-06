public class MessageParam
{
	public MapObject sender;
	public MapObject receiver;
	public string message;
	public sDamageInfo damageInfo;
}

public struct sDamageInfo
{
	public eDamageType attackType;
	public float damagePoint;
}

public struct sAttackInfo
{
	public sAttackInfo(eAttackRangeType rangeType, int attackRange, int attackPoint)
	{
		attackRangeType = rangeType;
		this.attackRange = attackRange;
		this.attackPoint = attackPoint;
	}

	public eAttackRangeType attackRangeType;
	public int attackRange;
	public float attackPoint;
}

public enum MessageProtocol
{

}