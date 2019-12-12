using UnityEngine;

public class PortalObject : MapObject
{
	public string _sceneName;

	private void Awake()
	{
		SetMapObjectType(eMapObjectType.PORTAL);

		//에디터에서 세팅한 포지션을 변수값에 저장해줘야 됨
		SetPosition(transform.position);
	}

	private void Start()
	{
		Vector3Int CellPos = TileSystem.Instance.GetGrid().WorldToCell(_position);
		SetTilePosition(CellPos.x, CellPos.y);
		TileSystem.Instance.GetTileCell(CellPos.x, CellPos.y).SetObject(this, eTileLayer.TRIGGER);
	}

	public override void ReceiveMessage(MessageParam msgParam)
	{
		switch(msgParam.message)
		{
			case "Interact":
				GameManager.Instance.LoadMap(_sceneName);
				break;
			default:
				break;
		}
	}
}
