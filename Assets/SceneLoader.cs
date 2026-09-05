using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	private GameObject sceneLoadingGameObject;
	private Slider sceneLoadingSlider;
	public string videoName;

	private bool isSceneLoading = false;
	private AsyncOperation loadingOperation;

	private static SceneLoader instance;

	void Awake ()
	{
		QualitySettings.vSyncCount  = 0;
		sceneLoadingSlider = GameObject.Find ("Scene Loading Slider").GetComponent<Slider> ();
		sceneLoadingGameObject = GameObject.Find ("Scene Loading");


		if (!instance) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy(gameObject);
		}

		sceneLoadingGameObject.SetActive(false);
	}

	void Update () {
		if (isSceneLoading) {
			if (!sceneLoadingGameObject.activeSelf) {
				sceneLoadingGameObject.SetActive (true);
			}
			sceneLoadingSlider.value = loadingOperation.progress;
		}

		if (isSceneLoading && loadingOperation.isDone) {
			isSceneLoading = false;
			sceneLoadingGameObject.SetActive(false);
		}
	}

	public void LoadLevel(string levelName) {
		loadingOperation = SceneManager.LoadSceneAsync(levelName);
		isSceneLoading = true;
	}
	
}
