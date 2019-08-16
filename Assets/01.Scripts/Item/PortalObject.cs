using UnityEngine;

public class PortalObject : MapObject
{
	sPortalInfo _portalInfo = new sPortalInfo();

	private void Awake()
	{
		_objectType = eMapObjectType.PORTAL;
	}

	public void SetPortalInfo(sPortalInfo info)
	{
		_portalInfo = info;
	}

	public override void ReceiveMessage(MessageParam msgParam)
	{
		switch(msgParam.message)
		{
			case "Interact":
				GameManager.Instance.LoadMap(_portalInfo);
				break;
			default:
				break;
		}
	}
}

public struct sPortalInfo
{
	public string portalName;
	public string nextMap;
	public int tileX;
	public int tileY;
}
