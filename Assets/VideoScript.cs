using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoScript : MonoBehaviour {

	public string videoName;
	//private string url;
	private GameObject loading;

	private VideoPlayer vid;
	private Slider slider;

	private Image playButton, volumeButton;
	private Sprite playSprite, pauseSprite, muteSprite, speakerSprite;
	private AudioSource audioSource;
	private GameObject videoPlayerPanel;
	private SceneLoader sceneLoader;

	private CameraController cam;

	public float panleHideOffset = 2f;
	private float lastTimeTouchedScreen = 0f;
	private bool isPanelHidden = false;

	private GameObject centerPlayButton;

	bool skiped = false;
	public bool changeSliderByUser = false;

	private VideoPlayer.FrameReadyEventHandler callBack;
	private GameObject videoRenderer, videoControllers, videoBG;

	// Use this for initialization
	void Awake () {
		//url = "http://morphodent.mrde3ign.ir/" + videoKey + ".mp4";
		//cam = FindObjectOfType<CameraController>();
		//QualitySettings.vSyncCount = 0;
		QualitySettings.vSyncCount  = 0;
		playSprite = Resources.Load<Sprite> ("Icons\\Play");
		pauseSprite = Resources.Load<Sprite> ("Icons\\Pause");
		muteSprite = Resources.Load<Sprite> ("Icons\\Mute");
		speakerSprite = Resources.Load<Sprite> ("Icons\\Speaker");
		playButton = GameObject.Find ("Play Button").GetComponent<Image> ();
		volumeButton = GameObject.Find ("Volume Button").GetComponent<Image> ();
		vid = GetComponent<VideoPlayer> ();
		slider = GameObject.Find ("Video Slider").GetComponent<Slider> ();
		audioSource = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();
		videoPlayerPanel = GameObject.Find ("Video Player Panel");
		videoBG = GameObject.Find("Video BG");
		sceneLoader = FindObjectOfType<SceneLoader>();
		loading = GameObject.Find("Loading");

		//vid.url = "http://morphodent.mrde3ign.ir/" + sceneLoader.videoName + ".mp4";
		//vid.Prepare();
		//vid.Play();

		/*videoControllers = GameObject.Find("Video Player Panel");
		videoRenderer = GameObject.Find("Video Renderer");
		videoBG.SetActive(false);
		videoControllers.SetActive(false);
		videoRenderer.SetActive(false);
		vid.Play();*/
	}

	void Start() {
		//vid.Play();
	}


	
	// Update is called once per frame
	void Update ()
	{
		float framePecentage = float.Parse (vid.frame.ToString ()) / (float.Parse (vid.frameCount.ToString ()));


		if (!changeSliderByUser) {
			slider.value = framePecentage;
		}


		if (framePecentage.ToString () == "NaN" && !loading.activeSelf) {
			UnhideLoading ();
		} else if (loading.activeSelf && framePecentage > 0f) {
			if (!skiped) {
				SkipVid ();
				skiped = true;
			}
			//hide fade
			Invoke ("HideLoading", 1f);
		}
		//PlayerPrefs.SetFloat(videoName, vid.frame);
		//print (float.Parse (vid.frame.ToString ()) / (float.Parse (vid.frameCount.ToString ())));

		if (Input.GetMouseButton (0)) {
			lastTimeTouchedScreen = Time.timeSinceLevelLoad;
			if (isPanelHidden) {
				UnhidePanel ();
			}
		}


		if (!isPanelHidden && (Time.timeSinceLevelLoad - lastTimeTouchedScreen > panleHideOffset)) {
			HidePanel ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			PrepareToExit();
		}
	}

	public void PauseButtonPressed ()
	{
		if (vid.isPlaying) {
			Pause();
		} else {
			Play();
		}
	}

	private void Pause() {
		vid.Pause();
		playButton.sprite = playSprite;
	}

	private void Play() {
		vid.Play();
		playButton.sprite = pauseSprite;
	}

	public void GoBack () {
		vid.frame -= 1000;
	}

	public void GoForward () {
		vid.frame += 1000;
	}

	public void RotateScreen ()
	{
		if (Screen.orientation == ScreenOrientation.Portrait) {
			Screen.orientation = ScreenOrientation.Landscape;
		} else {
			Screen.orientation = ScreenOrientation.Portrait;
		}

	}

	public void MuteButtonPressed () {
		if (audioSource.mute) {
			audioSource.mute = false;
			volumeButton.sprite = speakerSprite;
		} else {
			audioSource.mute = true;
			volumeButton.sprite = muteSprite;
		}

		}

	private void HidePanel() {
		videoPlayerPanel.SetActive(false);
		isPanelHidden = true;
	}

	private void UnhidePanel() {
		videoPlayerPanel.SetActive(true);
		isPanelHidden = false;
	}

	private void SkipVid() {
		if (PlayerPrefs.HasKey (videoName) && PlayerPrefs.GetFloat (videoName) > 10f) {
			Debug.Log("skiping to frame " + PlayerPrefs.GetFloat (videoName));
			vid.frame = long.Parse(PlayerPrefs.GetFloat(videoName).ToString());
		}

	}

	public void PrepareToExit() {
		PlayerPrefs.SetFloat(videoName, vid.frame);
		vid.Stop();
		Screen.orientation = ScreenOrientation.Portrait;
		sceneLoader.LoadLevel("Game");

		//SceneManager.LoadScene(0);
		//AsyncOperation operation = SceneM

		//HideLoading();
		//vid.Stop();
		//skiped = false;
		//videoBG.SetActive(false);
		//videoControllers.SetActive(false);
		//videoRenderer.SetActive(false);
	}

	private void HideLoading() {
		loading.SetActive(false);
	}

	private void UnhideLoading() {
		loading.SetActive(true);
	}

	void OnApplicationQuit() {
		PlayerPrefs.SetFloat(videoName, vid.frame);
		vid.Stop();
	}

	public void OnSliderValueChange () {
		print("ccccc");
	}

	public void OnSliderTouched() {
		changeSliderByUser = true;
	}

	public void SliderPointerDown() {
		changeSliderByUser = true;
		print("pointer down");
	}

	public void SliderPointerUp() {
		print(slider.value * float.Parse(vid.frameCount.ToString()));
		if (vid.frame > 0) {
			vid.frame = long.Parse(  Mathf.Round(slider.value * float.Parse(vid.frameCount.ToString())).ToString() );
		}
		changeSliderByUser = true;

		print("SliderPointerUp up");
	}

}
