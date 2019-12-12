using UnityEngine;

public abstract class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
{
	static T _instance;
	public static T Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance = FindObjectOfType<T>();
				if (null == _instance)
				{
					GameObject go = new GameObject();
					_instance = go.AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	void Start()
	{
		Init();
	}

	public virtual void Init()
	{

	}

	//이름 , Init, Don't destory 설정은 따로
}

public abstract class Singleton<T> where T : class, new()
{
	static T _instance;

	public static T Instance
	{
		get
		{
			if(null == _instance)
			{
				_instance = new T();
			}
			return _instance;
		}
	}
}
