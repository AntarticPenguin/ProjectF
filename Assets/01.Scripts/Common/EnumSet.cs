public enum eTileLayer
{
	NONE,
	GROUND,
	ITEM,
	ON_GROUND,
	TRIGGER,
	RANGE,
	BLOCK,
	MAXCOUNT,
}

//타일 안, 밖(모서리)
public enum eTileDirection
{
	NORTH_WEST,
	NORTH_EAST,
	SOUTH_EAST,
	SOUTH_WEST,
	IN_TILE,
}

//캐릭터들의 움직이는 방향
public enum eDirection
{
	//NORTH,
	NORTH_EAST = 0,
	//EAST,
	SOUTH_EAST,
	//SOUTH,
	SOUTH_WEST,
	//WEST,
	NORTH_WEST,
	MAX
}

public enum eMapObjectType
{
	NONE,
	TILEOBJECT,
	CHARACTER,
	PLAYER,
	ENEMY,
	ITEM,
	PORTAL,
}

public enum eStateType
{
	NONE,
	IDLE,
	MOVE,
	ATTACK,
	DAMAGE,
	CHASE,
	PATHFIND,
}

public enum eBoundary
{
	UP = 0,
	DOWN,
	LEFT,
	RIGHT,
	MAX,
}

public enum eTileAsset
{
	Brick,
	Dirt,
	Grass,
	Lava,
	Sand,
	Snow,
	Stairs,
	Stone,
	Water,
	Wood,
	MAX,
}

public enum eTilemapType
{
	TRIGGER,
	GROUND,
	BLOCK,
}

public enum eDamageType
{
	NORMAL,			//데미지만 전달
	STUN,			//스턴 및 짧은 경직 추가
}

public enum eAttackRangeType
{
	STRAIGHT,
}

public class EnumSet
{
 
}
