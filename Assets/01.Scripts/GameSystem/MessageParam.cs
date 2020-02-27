public class MessageParam
{
	public MapObject sender;
	public MapObject receiver;
	public string message;
	public sAttackInfo attackInfo;
}

public struct sAttackInfo
{
	public float attackPoint;
	public eAttackType attackType;
}

public enum MessageProtocol
{

}