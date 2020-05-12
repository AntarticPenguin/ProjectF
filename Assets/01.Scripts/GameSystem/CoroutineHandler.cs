using System.Collections;

public class CoroutineHandler : SingletonMonobehavior<CoroutineHandler>
{
	public override void InitStart()
	{
		DontDestroyOnLoad(this);
	}

	public void DoCoroutine(IEnumerator enumerator)
	{
		StartCoroutine(enumerator);
	}
}
