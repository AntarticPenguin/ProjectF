using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public GameObject _loadingScreen;
	public Slider _slider;
	public Text _progressText;

	public void LoadLevel(string sceneName)
	{
		if (Application.CanStreamedLevelBeLoaded(sceneName))
			StartCoroutine(LoadAsynchronously(sceneName));
		else
			Debug.Log("Scene doesn't exist");
	}

	IEnumerator LoadAsynchronously(string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
		_loadingScreen.SetActive(true);

		while(!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			_slider.value = progress;
			_progressText.text = progress * 100.0f + "%";

			yield return null;
		}
		_loadingScreen.SetActive(false);
	}
}
