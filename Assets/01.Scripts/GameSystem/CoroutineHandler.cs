using System.Collections;

public class CoroutineHandler : SingletonMonobehavior<CoroutineHandler>
{
	public override void Init()
	{
		base.Init();

		DontDestroyOnLoad(this);
	}

	public void DoCoroutine(IEnumerator enumerator)
	{
		StartCoroutine(enumerator);
	}
}
