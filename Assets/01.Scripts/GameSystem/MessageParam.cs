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
	public sAttackInfo(eAttackType type, int attackRange, int attackPoint)
	{
		attackType = type;
		this.attackRange = attackRange;
		this.attackPoint = attackPoint;
	}

	public eAttackType attackType;
	public int attackRange;
	public float attackPoint;
}

public enum MessageProtocol
{

}