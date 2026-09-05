using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using RTLTMPro;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Soomla.Store;

public class CameraController : MonoBehaviour {

	//private GameObject videoRenderer, videoControllers, videoBG;
	private GameHandler gameHandler;

	public bool IsInNearView = false;
	public bool isViewPortLocked = false;
	public bool quizMode = false;
	public bool touchDisabled = false;
	public bool selectTeethAllow = true;
	public bool showPins = false;
	public bool isInMenu = true;

	private Text partTextHolder;

	public Teeth activeTeeth;

	private SceneLoader sceneLoader;

	public Transform target;
	public Material blackBG;
     public float distance = 2.0f;
	 public float zoomSpeed = 0.02f;
	 public InputField inp;
     public float xSpeed = 5f;
     public float ySpeed = 20f;
	 private float farXSpeed = 5f;
     private float farYSpeed = 20f;
     public float yMinLimit = -90f;
     public float yMaxLimit = 90f;
     public float distanceMin = 10f;
     public float distanceMax = 10f;
     public float smoothTime = 2f;
     float rotationYAxis = 0.0f;
     float rotationXAxis = 0.0f;
     float velocityX = 0.0f;
     float velocityY = 0.0f;
     private Rigidbody rigi;
     private Transform mouthModel;
	GameObject[] objs;

	private static GameObject alertBox;
	private static Text alertBoxText;

	private float farViewSpeedX = 3f;

	private Quaternion startRotation;
	private Vector3 startPosition;

	private Light pointLight;

	public static VideoScript  videoStreamer;
	//private GameObject videoPlayer;

	private Image lockButton;
	private Image infoButton;
	private Image pinsButton;
	private GameObject backButton;
	private GameObject quizButton;
	private Sprite lockClosedSprite, lockOpenedSprite;
	private Sprite infoSprite, openMenuSprite;
	private Sprite showPinsSprite, hidePinsSprite;
	private GameObject slider;
	private GameObject infoPanel;
	private Text infoTeethName;
	private RTLTextMeshPro infoTeethBio;
	private QuizPanel quizPanel;
	private GameObject settingsPanel, aboutPanel, videoPanel;
	private GameObject fade;
	private Slider fakSlider;
	private GameObject menu;
	private Dropdown settings_camxspeed, settings_camyspeed;
	private Slider settings_volume;

	private float offsetTime = 0.1f;

	private float settingXCamSpeed = 5f;
	private float settingYCamSpeed = 20f;

	private RectTransform infoTeethBioRect;

	public static Transform FakAxis;

	private Scrollbar infoScrollbar;
	//private GameObject centerPlayButton;



     // Use this for initialization
     void Awake () {
		//QualitySettings.vSyncCount = 0;
		sceneLoader = FindObjectOfType<SceneLoader>();
		Vector3 angles = transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;
		if (PlayerPrefs.HasKey ("CamXSpeed")) {
			farXSpeed = PlayerPrefs.GetFloat("CamXSpeed");
			xSpeed = farXSpeed;
		}
		if (PlayerPrefs.HasKey ("CamYSpeed")) {
			farYSpeed = PlayerPrefs.GetFloat("CamYSpeed");
			ySpeed = farYSpeed;
		}
         // Make the rigid body not change rotation
		rigi = GetComponent<Rigidbody>();
		if (rigi)
         {
             rigi.freezeRotation = true;
         }
         mouthModel = GameObject.Find("MouthModel").transform;
		FakAxis = GameObject.Find("FakAxis").transform;
         pointLight = GetComponentInChildren<Light>();
		 objs = GameObject.FindGameObjectsWithTag("obj");

		startRotation = transform.rotation;
		startPosition = transform.position;

		lockButton = GameObject.Find("Lock Button").GetComponent<Image>();
		pinsButton = GameObject.Find("Pins Button").GetComponent<Image>();
		HidePinsButton();
		lockOpenedSprite = Resources.Load<Sprite>("Icons\\LockOpened");
		lockClosedSprite = Resources.Load<Sprite>("Icons\\LockClosed");
		hidePinsSprite = Resources.Load<Sprite>("Icons\\Hidepins");
		showPinsSprite = Resources.Load<Sprite>("Icons\\ShowPins");
		infoSprite = Resources.Load<Sprite>("Icons\\Info");
		openMenuSprite = Resources.Load<Sprite>("Icons\\OpenMenu");
		infoPanel = GameObject.Find("Info Panel");
		infoScrollbar = GameObject.Find("Info Scrollbar").GetComponent<Scrollbar>();
		infoTeethName = infoPanel.transform.Find("Info TeethName").GetComponent<Text>();
		infoTeethBio = GameObject.Find("Info TeethBio").GetComponent<RTLTextMeshPro>();
		infoTeethBioRect = infoTeethBio.GetComponent<RectTransform>();
		alertBox = GameObject.Find("Alert Box");
		alertBoxText = alertBox.transform.Find("Massage").GetComponent<Text>();
		alertBox.SetActive(false);
		//infoPanel.SetActive(false);
		quizPanel = GameObject.Find("Quiz Panel").GetComponent<QuizPanel>();
		quizPanel.HideQuizPanel();
		slider = GameObject.Find("Slider");
		fade = GameObject.Find("Fade");
		fade.SetActive(false);
		//fade2 = GameObject.Find("Fade2");
		//fade2.SetActive(false);
		fakSlider = FindObjectOfType<SliderController>().GetComponent<Slider>();
		menu = GameObject.Find("Menu");
		aboutPanel = GameObject.Find("About Panel");
		aboutPanel.SetActive(false);
		//centerPlayButton = GameObject.Find("Center Play Button");
		//centerPlayButton.SetActive(false);
		videoStreamer = FindObjectOfType<VideoScript>();
		videoPanel = GameObject.Find("Video Panel");
		videoPanel.SetActive(false);
		GameObject menuVideosButton = GameObject.Find("menu_videos");
		if (menuVideosButton) {
			menuVideosButton.SetActive(false);
		}
		//videoPlayer = GameObject.Find("VideoPlayer");
		/*videoBG = GameObject.Find("Video BG");
		videoControllers = GameObject.Find("Video Player Panel");
		videoRenderer = GameObject.Find("Video Renderer");
		videoBG.SetActive(false);
		videoControllers.SetActive(false);
		videoRenderer.SetActive(false);*/
		//videoPanel.SetActive(false);
		//videoPlayer.SetActive(false);
		settingsPanel = GameObject.Find("Settings Panel");
		settings_camxspeed = settingsPanel.transform.Find("CamXSpeed Dropdown").GetComponent<Dropdown>();
		settings_camyspeed = settingsPanel.transform.Find("CamYSpeed Dropdown").GetComponent<Dropdown>();
		settings_volume = settingsPanel.transform.Find("Volume Slider").GetComponent<Slider>();
		CreateRestorePurchaseButton();
		settingsPanel.SetActive(false);
		LockViewport();

		partTextHolder = GameObject.Find ("PartTextHolder").gameObject.GetComponent<Text> ();
		//partTitle = partsNameText.transform.Find ("PartTitle").gameObject.GetComponent<Text> ();
		//SetPartTitleValue("");
		SetPartTextHolderValue("");
		infoButton = GameObject.Find("Info Button").GetComponent<Image>();
		backButton = GameObject.Find("Back Button");
		SetbackButtonActive(false);
		quizButton = GameObject.Find("Quiz Button");
		gameHandler = FindObjectOfType<GameHandler>();
     }


     void LateUpdate ()
	{
		if (!isViewPortLocked && target) {
			if (!touchDisabled && Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) {
				//Android: !touchDisabled && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved
				//Windows: !touchDisabled && Input.GetMouseButton(0)
				selectTeethAllow = true;
				velocityX += xSpeed * Input.GetAxis ("Mouse X") * distance * 0.02f;
				velocityY += ySpeed * Input.GetAxis ("Mouse Y") * 0.02f;
			} else if (Input.touchCount == 2) {
				selectTeethAllow = false;
				float loc_x = 0.0F;
				float loc_y = 0.0F;
				Touch f0 = Input.GetTouch (1);
				Touch f1 = Input.GetTouch (0);
				Vector3 f0Delta = new Vector3 (-f0.deltaPosition.x, -f0.deltaPosition.y, 0);
				Vector3 f1Delta = new Vector3 (-f1.deltaPosition.x, -f1.deltaPosition.y, 0);
				float toDirection = Vector3.Dot (f0Delta, f1Delta);
				float dx = 0f;
				float dy = 0f;

				if (f1.phase == TouchPhase.Moved && f0.phase == TouchPhase.Moved) {
					if (toDirection > 0) {
						Vector3 f0Dir = Vector3.zero;
						dy += f0Delta.y * 0.01F;
						dx += f0Delta.x * 0.01F;
						loc_x = f0Delta.x * 0.01F;
						loc_y = f0Delta.y * 0.01F;
					} else {
						Vector2 curDist = f0.position - f1.position;
						Vector2 prevDist = (f0.position - f0.deltaPosition) - (f1.position - f1.deltaPosition);
						float delta = curDist.magnitude - prevDist.magnitude;
						distance = Mathf.Clamp (distance - delta * zoomSpeed, distanceMin, distanceMax);
					}
				}
			} else if (Input.touchCount == 3 && !IsInNearView) {
				selectTeethAllow = false;
				float loc_x = 0.0F;
				float loc_y = 0.0F;
				Touch f0 = Input.GetTouch (0);
				Touch f1 = Input.GetTouch (1);
				Touch f2 = Input.GetTouch (2);
				Vector3 f0Delta = new Vector3 (-f0.deltaPosition.x, -f0.deltaPosition.y, 0);
				Vector3 f1Delta = new Vector3 (-f1.deltaPosition.x, -f1.deltaPosition.y, 0);
				Vector3 f2Delta = new Vector3 (-f2.deltaPosition.x, -f2.deltaPosition.y, 0);
				float dx = 0f;
				float dy = 0f;

				if (f1.phase == TouchPhase.Moved && f0.phase == TouchPhase.Moved) {
					float toDirection = Vector3.Dot (f0Delta, f1Delta);
					if (toDirection > 0) {
						Vector3 f0Dir = Vector3.zero;
						dy += f0Delta.y * 0.01F;
						dx += f0Delta.x * 0.01F;
						loc_x = f0Delta.x * 0.01F;
						loc_y = f0Delta.y * 0.01F;
					} else {
						Vector2 curDist = f0.position - f1.position;
						Vector2 prevDist = (f0.position - f0.deltaPosition) - (f1.position - f1.deltaPosition);
						float delta = curDist.magnitude - prevDist.magnitude;
						fakSlider.value += delta;
					}
				} else if (f2.phase == TouchPhase.Moved && f0.phase == TouchPhase.Moved) {
					float toDirection = Vector3.Dot (f0Delta, f2Delta);
					if (toDirection > 0) {
						Vector3 f0Dir = Vector3.zero;
						dy += f0Delta.y * 0.01F;
						dx += f0Delta.x * 0.01F;
						loc_x = f0Delta.x * 0.01F;
						loc_y = f0Delta.y * 0.01F;
					} else {
						Vector2 curDist = f0.position - f2.position;
						Vector2 prevDist = (f0.position - f0.deltaPosition) - (f2.position - f2.deltaPosition);
						float delta = curDist.magnitude - prevDist.magnitude;
						fakSlider.value += delta;
					}
				} else if (f2.phase == TouchPhase.Moved && f1.phase == TouchPhase.Moved) {
					float toDirection = Vector3.Dot (f1Delta, f2Delta);
					if (toDirection > 0) {
						Vector3 f0Dir = Vector3.zero;
						dy += f0Delta.y * 0.01F;
						dx += f0Delta.x * 0.01F;
						loc_x = f0Delta.x * 0.01F;
						loc_y = f0Delta.y * 0.01F;
					} else {
						Vector2 curDist = f1.position - f2.position;
						Vector2 prevDist = (f1.position - f1.deltaPosition) - (f2.position - f2.deltaPosition);
						float delta = curDist.magnitude - prevDist.magnitude;
						fakSlider.value += delta;
					}
				}
			} else {
				selectTeethAllow = true;
			}

			rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
			rotationXAxis = ClampAngle (rotationXAxis, yMinLimit, yMaxLimit);
			Quaternion fromRotation = Quaternion.Euler (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
			Quaternion toRotation = Quaternion.Euler (rotationXAxis, rotationYAxis, 0);
			Quaternion rotation = toRotation;

			/*if(Input.GetKey(KeyCode.UpArrow)) {
				float newDistance = distance - 0.1f;
				distance = Mathf.Clamp(newDistance, distanceMin, distanceMax);
			} else if(Input.GetKey(KeyCode.DownArrow)) {
				float newDistance = distance + 0.1f;
				distance = Mathf.Clamp(newDistance, distanceMin, distanceMax);
	         }*/


			//distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 2, distanceMin, distanceMax);
			/*RaycastHit hit;
	         if (Physics.Linecast(target.position, transform.position, out hit))
	         {
	             //distance -= hit.distance;
	         }*/
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position;
			velocityX = Mathf.Lerp (velocityX, 0, Time.deltaTime * smoothTime);
			velocityY = Mathf.Lerp (velocityY, 0, Time.deltaTime * smoothTime);
		
			blackBG.SetFloat ("_Rotation", -transform.rotation.eulerAngles.y);
			if (IsInNearView) {
				pointLight.intensity = distance / 2.333f;
			} else {
				pointLight.intensity = distance / 4f;
			}



		}


		//Back Button Actions:
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!isInMenu) {
				if (IsInNearView) {
					if (!quizMode) {
						BackToModel ();
					} else {
						quizPanel.QuizButtonPressed ();
					}
				} else {
					if (!quizMode) {
						InfoButtonPressed ();
					} else {
						quizPanel.QuizButtonPressed ();
					}
				}
			} else {
				if (aboutPanel.activeSelf) {
					aboutpanel_exitPressed ();
				} else if (settingsPanel.activeSelf) {
					SetPrefs ();
				} else if (videoPanel.activeSelf) {
					videopanel_exitPressed ();
				}
			}
		}

     }

	private void CreateRestorePurchaseButton () {
		GameObject btnObj = new GameObject("RestorePurchaseButton", typeof(RectTransform));
		btnObj.transform.SetParent(settingsPanel.transform, false);

		RectTransform rt = btnObj.GetComponent<RectTransform>();
		rt.anchorMin = new Vector2(0.5f, 0f);
		rt.anchorMax = new Vector2(0.5f, 0f);
		rt.pivot = new Vector2(0.5f, 0f);
		rt.anchoredPosition = new Vector2(0f, 40f);
		rt.sizeDelta = new Vector2(360f, 70f);

		RTLTextMeshPro label = btnObj.AddComponent<RTLTextMeshPro>();
		label.text = "بازیابی خرید";
		label.font = infoTeethBio.font;
		label.fontSize = 30f;
		label.alignment = TextAlignmentOptions.Center;
		label.color = Color.white;
		label.raycastTarget = true;

		Button btn = btnObj.AddComponent<Button>();
		btn.targetGraphic = label;
		btn.onClick.AddListener(RestorePurchasesPressed);
	}

	public void RestorePurchasesPressed () {
		ShowAlertBox("در حال بازیابی خرید...");
		SoomlaStore.RestoreTransactions();
	}

	public void SetPrefs () {
		if (PlayerPrefs.HasKey ("CamXSpeed")) {
			farXSpeed = PlayerPrefs.GetFloat("CamXSpeed");
			xSpeed = farXSpeed;
		}
		if (PlayerPrefs.HasKey ("CamYSpeed")) {
			farYSpeed = PlayerPrefs.GetFloat("CamYSpeed");
			ySpeed = farYSpeed;
		}
		quizPanel.SetPrefs();
		settingsPanel.SetActive(false);
	}


     public static float ClampAngle(float angle, float min, float max) {
         if (angle < -360F)
             angle += 360F;
         if (angle > 360F)
             angle -= 360F;
         return Mathf.Clamp(angle, min, max);
     }

     void OnTriggerEnter() {
     	//isInTerrain = true;

     }


	public void HideVideoPanel () {
		videoPanel.SetActive(false);
	 }

	public void StartVideoPlayer (string videoKey)
	{
		ShowAlertBox("بخش ویدیوهای آموزشی موقتا بسته است.");

		//sceneLoader.videoName = videoKey;
		//sceneLoader.LoadLevel("Vid");
		//SceneManager.LoadScene(1);
		//videoPlayer.SetActive(true);
		//videoStreamer.videoName = videoKey;
		//videoStreamer.SetURL();
		//SetVideoURL ();
		/*videoBG.SetActive(true);
		videoControllers.SetActive(true);
		videoRenderer.SetActive(true);
		//Invoke("SetVideoURL", 0.5f);*/
	}



     public void BackToModel ()
	{
		if (IsInNearView) {
			showPins = false;
			pinsButton.sprite = showPinsSprite;
			isViewPortLocked = false;
			SetPartTextHolderValue ("");
			SetbackButtonActive (false);
			lockButton.sprite = lockOpenedSprite;
			Teeth[] activeObjs = GameObject.FindObjectsOfType<Teeth> ();
			foreach (Teeth obj in activeObjs) {
				obj.gameObject.transform.rotation = obj.farViewRotation;
				obj.SetColliderState (true);
			}
			activeTeeth.HideAllViews ();
			ShowAllObjects ();
			if (activeTeeth.lastSelectedPart) {
				activeTeeth.lastSelectedPart.SetColorToUnselected ();
			}
			target = mouthModel;
			distance = 9f;
			xSpeed = farViewSpeedX;
			ySpeed = farYSpeed;
			distanceMax = 12f;
			distanceMin = 6f;
			ySpeed = 12f;
			zoomSpeed = 0.025f;
			ResetCam ();
			SetInfoButtonFarviewSprite ();
			UnhideSlider ();
			HidePinsButton ();
		}

     }

	void ShowAllObjects () {
		IsInNearView = false;
		foreach (GameObject obj in objs) {
			obj.SetActive(true);
		}
	}

	public void ResetCam () {
		velocityX = 0;
		velocityY = 0;
		rotationXAxis = 0;
		rotationYAxis = 180;
	}

	public void ViewPortLockPressed () {
		if (!isViewPortLocked) {
			LockViewport ();
		} else {
			UnlockViewport ();
		}
	}

	public void LockViewport () {
		isViewPortLocked = true;
		lockButton.sprite = lockClosedSprite;
		velocityX = 0;
		velocityY = 0;
	}

	public void UnlockViewport () {
		isViewPortLocked = false;
		lockButton.sprite = lockOpenedSprite;
	}

	public void InfoButtonPressed () {
		if (IsInNearView) {
			isInMenu = true;
			isViewPortLocked = true;
			fade.SetActive (true);
			infoPanel.SetActive (true);
			partTextHolder.gameObject.SetActive (false);
			infoTeethName.text = activeTeeth.teethName;
			infoTeethBio.text = activeTeeth.teethBio;
			infoTeethBioRect.sizeDelta = new Vector2(infoTeethBioRect.sizeDelta.x, activeTeeth.bioHeight);
			infoScrollbar.value = 1f;
		} else {
			menu.SetActive(true);
			infoPanel.SetActive (true);
			LockViewport();
		}
	}

	public void InfoPanelCloseButtonPressed () {
		isViewPortLocked = false;
		fade.SetActive(false);
		infoPanel.SetActive(false);
		partTextHolder.gameObject.SetActive(true);
	}

	public void SetInfoButtonFarviewSprite() {
		infoButton.sprite = openMenuSprite;
	}

	public void SetInfoButtonNearviewSprite() {
		infoButton.sprite = infoSprite;
	}

	public void HideInfoButton() {
		infoButton.gameObject.SetActive(false);
	}

	public void UnhideInfoButton() {
		infoButton.gameObject.SetActive(true);
	}

	public void SetbackButtonActive (bool value) {
		backButton.SetActive(value);
	}

	/*public void SetPartTitleValue(string text) {
		partTitle.text = text;
	}*/

	public void SetPartTextHolderValue(string text) {
		partTextHolder.text = text;
	}

	public GameObject[] GetObjs () {
		return objs;
	}

	public void HideBackButton() {
		backButton.SetActive(false);
	}

	public void UnhideBackButton() {
		backButton.SetActive(true);
	}

	public void HideLockButton() {
		lockButton.gameObject.SetActive(false);
	}

	public void UnhideLockButton() {
		lockButton.gameObject.SetActive(true);
	}


	public void HideQuizButton() {
		quizButton.SetActive(false);
	}

	public void UnhideQuizButton() {
		quizButton.SetActive(true);
	}

	public void HidePinsButton() {
		pinsButton.gameObject.SetActive(false);
	}

	public void UnhidePinsButton() {
		pinsButton.gameObject.SetActive(true);
	}

	public void HideSlider () {
		slider.transform.parent.gameObject.SetActive(false);
	}

	public void UnhideSlider () {
		slider.transform.parent.gameObject.SetActive(true);
	}

	private float GetAngleSign (float angle) {
		print(angle);
		if (angle >= -90 && angle <= 90) {
			return 1;
		} else {
			return -1;
		}
	}

	public void menu_viewmodelPressed() {
		isInMenu = false;
		if(menu.activeSelf) {
			menu.SetActive(false);
		} else {
			menu.SetActive(true);
		}
		infoPanel.SetActive (false);
		BackToModel();
		UnlockViewport();
	}

	public void menu_videosPressed() {
		ShowAlertBox("بخش ویدیوهای آموزشی موقتا بسته است.");
	}

	public void menu_settingsPressed () {
		settingsPanel.SetActive (true);
		if (PlayerPrefs.HasKey ("SettingsCamXIndex")) {
			settings_camxspeed.value =  PlayerPrefs.GetInt("SettingsCamXIndex");
		}
		if (PlayerPrefs.HasKey ("SettingsCamYIndex")) {
			settings_camyspeed.value =  PlayerPrefs.GetInt("SettingsCamYIndex");
		}
		if (PlayerPrefs.HasKey ("Volume")) {
			settings_volume.value = PlayerPrefs.GetFloat("Volume");
		}
	}

	public void menu_aboutPressed () {
		aboutPanel.SetActive(true);
	}

	public void menu_exitPressed () {
		Application.Quit();
	}

	public void videopanel_exitPressed() {
		videoPanel.SetActive(false);
	}

	public void videoplayer_exitPressed() {
		videoStreamer.PrepareToExit();
		//videoPlayer.SetActive(false);
	}

	public void aboutpanel_exitPressed() {
		aboutPanel.SetActive(false);
	}

	public void settings_camxspeedValueChanged () {
		settings_camxspeed.transform.Find ("Label").GetComponent<Text> ().text = settings_camxspeed.options [settings_camxspeed.value].text;
		float val = 2;
		switch (settings_camxspeed.value) {
			case 0:
				val *= 4f;
				break;
			case 1:
				val *= 3f;
				break;
			case 2:
				val *= 2f;
				break;
			case 3:
				val *= 1f;
				break;
			case 4:
				val *= 0.75f;
				break;
			case 5:
				val *= 0.5f;
				break;
			case 6:
				val *= 0.25f;
				break;
		}
		PlayerPrefs.SetFloat("CamXSpeed", val);
		PlayerPrefs.SetInt("SettingsCamXIndex", settings_camxspeed.value);
	}

	public void settings_camyspeedValueChanged () {
		settings_camyspeed.transform.Find ("Label").GetComponent<Text> ().text = settings_camyspeed.options [settings_camyspeed.value].text;
		float val = 12;
		switch (settings_camyspeed.value) {
			case 0:
				val *= 4f;
				break;
			case 1:
				val *= 3f;
				break;
			case 2:
				val *= 2f;
				break;
			case 3:
				val *= 1f;
				break;
			case 4:
				val *= 0.75f;
				break;
			case 5:
				val *= 0.5f;
				break;
			case 6:
				val *= 0.25f;
				break;
		}
		PlayerPrefs.SetFloat("CamYSpeed", val);
		PlayerPrefs.SetInt("SettingsCamYIndex", settings_camyspeed.value);
	}

	public void settings_volumeValueChanged() {
		PlayerPrefs.SetFloat("Volume", settings_volume.value);
	}

	public float GetFarYSpeed() {
		return farYSpeed;
	}

	public float GetFarXSpeed() {
		return farXSpeed;
	}

	public void pinsButtonPressed ()
	{
		if (showPins) {
			HidePins();
		} else {
			ShowPins();
		}
	}

	public void ShowPins () {
		showPins = true;
		pinsButton.sprite = hidePinsSprite;
		if (IsInNearView) {
			activeTeeth.UnhideView(activeTeeth.activeView);
		}
	}	

	public void HidePins() {
		showPins = false;
		pinsButton.sprite = showPinsSprite;
		SetPartTextHolderValue ("");
		if (IsInNearView && activeTeeth.lastSelectedPart) {
			activeTeeth.lastSelectedPart.SetColorToUnselected ();
		}
	}	

	public static void ShowAlertBox (string massage) {
		alertBox.SetActive(true);
		alertBoxText.text = massage;
	}

	public static void AlertBoxCloseButtonPressed () {
		alertBox.SetActive(false);
	}

	public void TelegramIconPressed() {
		Application.OpenURL("https://t.me/DentalYar_app");
	}

	public void InstagramIconPressed() {
		Application.OpenURL("https://www.Instagram.com/dentalyar.app");
	}


}
