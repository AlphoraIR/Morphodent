using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teeth : MonoBehaviour {

	public const int FreeTeethCount = 3;

	public string teethName;
	[TextArea(4,2000)] public string teethBio;
	public float bioHeight = 3441f;

	public GameObject activeView;
	public Part activePart;
	public Part lastSelectedPart;

	private GameObject lastActiveView;
	//private TouchCam cam;
	private CameraController cam;
	private Transform mouthModel;
	private QuizPanel quizPanel;

	private GameHandler gameHandler;

	//GameObject[] objs;

	private float nearViewLight = 0.6f;
	private float farViewLight = 3f;

	private float nearViewSpeedX = 10f;


	public Quaternion farViewRotation;

	private bool isRight;
	private bool isUp;

	//private GameObject canvas;
	private GameObject buccalView;
	private GameObject lingualView;
	private GameObject distalView;
	private GameObject mesialView;
	private GameObject incisalView;

	private Material ans_red, ans_green, defaultMaterial;

	private string lastView;

	private float minNegIncisalRange, minPosIncisalRange, maxNegIncisalRange, maxPosIncisalRange;

	private BoxCollider coll;

	int teethNum;

	private float teethXRotation = 0f;


	public void SetColliderState (bool state) {
		coll.enabled = state;
	}

	// Use this for initialization
	void Awake ()
	{
		teethNum = int.Parse (gameObject.name);
		if (teethNum <= 8) {
			isRight = true;
			isUp = true;
		} else if (teethNum <= 16) {
			isRight = false;
			isUp = true;
		} else if (teethNum <= 24) {
			isRight = false;
			isUp = false;
		} else if (teethNum <= 32) {
			isRight = true;
			isUp = false;
		}

		if (isUp) {
			minNegIncisalRange = -140f;
			maxNegIncisalRange = -60f;
			minPosIncisalRange = 220f;
			maxPosIncisalRange = 300f;
		} else {
			minNegIncisalRange = -320f;
			maxNegIncisalRange = -245f;
			minPosIncisalRange = 40f;
			maxPosIncisalRange = 115f;
		}

		switch(teethNum) {
			case 1:
			case 2:
			case 3:
			case 32:
			case 31:
			case 30:
				teethXRotation = -90f;
				break;
			case 4:
			case 5:
			case 29:
			case 28:
				teethXRotation = -60f;
				break;
			case 6:
			case 27:
				teethXRotation = -45f;
				break;
			case 7:

				teethXRotation = -30f;
				break;
			case 8:
			case 9:
				teethXRotation = 0f;
				break;
			case 10:
				teethXRotation = 30f;
				break;
			case 11:
			case 22:
				teethXRotation = 45f;
				break;
			case 13:
			case 12:
			case 21:
			case 20:

				teethXRotation = 60f;
				break;

			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
				teethXRotation = 90f;
					break;
		}

		coll = GetComponent<BoxCollider>();

		//cam = FindObjectOfType<TouchCam> ();
		cam = FindObjectOfType<CameraController> ();
		mouthModel = GameObject.Find ("MouthModel").transform;
		quizPanel = FindObjectOfType<QuizPanel>();
		//objs = GameObject.FindGameObjectsWithTag ("obj");
		ans_red = Resources.Load<Material>("Materials\\Red");
		ans_green = Resources.Load<Material>("Materials\\Green");
		defaultMaterial = GetComponent<MeshRenderer>().material;


		//canvas = transform.Find ("Canvas").gameObject;
		buccalView = transform.Find ("Buccal").gameObject;
		lingualView = transform.Find ("Lingual").gameObject;
		distalView = transform.Find ("Distal").gameObject;
		mesialView = transform.Find ("Mesial").gameObject;
		incisalView = transform.Find ("Incisal").gameObject;

		gameHandler = FindObjectOfType<GameHandler>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (cam.quizMode) {
			return;
		}
		if (activePart) {
			if (!activePart.transform.parent.gameObject.activeSelf) {
				//cam.SetPartTitleValue("");
				cam.SetPartTextHolderValue ("");
				lastSelectedPart.SetColorToUnselected ();
			}
		}


		if (!cam.IsInNearView) {
			return;
		}

		lastActiveView = activeView;

		float camYRot = cam.transform.rotation.eulerAngles.y - 180f;
		float camXRot = cam.transform.rotation.eulerAngles.x;
		//print(camXRot);

		if ((camXRot <= maxNegIncisalRange && camXRot >= minNegIncisalRange) || (camXRot <= maxPosIncisalRange && camXRot >= minPosIncisalRange)) {
			activeView = incisalView;
		} else {
			if ((camYRot >= 300 && camYRot <= 360) || (camYRot >= 0 && camYRot <= 60)) {
				activeView = buccalView;
			} else if (camYRot > 60 && camYRot < 120 || (camYRot > -240 && camYRot < -300)) {
				if (isRight) {
					activeView = distalView;
				} else if (!isRight) {
					activeView = mesialView;
				}
			} else if (camYRot >= 120 && camYRot <= 240 || (camYRot >= -240 && camYRot <= -120)) {
				activeView = lingualView;
			} else if ((camYRot > 240 && camYRot < 300) || (camYRot > -120 && camYRot < -60)) {
				if (isRight) {
					activeView = mesialView;
				} else if (!isRight) {
					activeView = distalView;
				}
			}
		}

		if (activeView && lastActiveView && activeView != lastActiveView) {
			cam.SetPartTextHolderValue ("");
			if (lastSelectedPart) {
				lastSelectedPart.SetColorToUnselected ();
			}
			HideView (lastActiveView);
		}
		if (activeView && cam.showPins) {
			UnhideView (activeView);
		} else if (!cam.showPins && activeView) {
			HideView (activeView);
		}


	}

	void OnMouseDown ()
	{
		if (!cam.IsInNearView && !cam.isViewPortLocked) {
			if (!cam.quizMode) {
				if (cam.selectTeethAllow) {
					if (teethNum <= FreeTeethCount) {
						IsolateThisTeeth();
					} else if (PlayerPrefs.HasKey(StoreAssets.FULL_VERSION_ITEM_ID + "Purchased") && PlayerPrefs.GetInt (StoreAssets.FULL_VERSION_ITEM_ID + "Purchased") == 1) {
						IsolateThisTeeth();
					} else {
						gameHandler.BuyGame(StoreAssets.FULL_VERSION_ITEM_ID);
					}

				}
			} else {
				if (quizPanel.askedTeeth == this) {
					cam.isViewPortLocked = true;
					DeselectLastRedTeeth ();
					SetColorToGreen ();
					quizPanel.RegisterCorrectAnswer ();
					quizPanel.GetComponent<AudioSource>().Play();
					Invoke("AskNextTeeth", 1f);
				} else {
					DeselectLastRedTeeth ();
					SetColorToRed();
					quizPanel.RegisterWrongAnswer ();
					Invoke("SetColorToDefault", 1f);
					quizPanel.lastRedTeeth = this;
				}
			}
		}
	}

	private void IsolateThisTeeth() {
		UnhideAllViews();
		HideAllViews();
		cam.activeTeeth = this;
		cam.ResetCam ();
		SetColliderState (false);
		farViewRotation = transform.rotation;
		if (transform.parent.gameObject.name.Contains ("Up") || transform.parent.gameObject.name.Contains ("Down")) {
			transform.rotation = Quaternion.Euler (180, teethXRotation, 0);
		}
		HideOtherObjects ();
		cam.SetInfoButtonNearviewSprite();
		cam.SetbackButtonActive (true);
		cam.HideSlider();
		cam.UnhidePinsButton();
		cam.showPins = false;
		cam.ySpeed = cam.GetFarYSpeed() * (5f/6);
		cam.zoomSpeed = 0.005f;
	}

	void HideOtherObjects () {
		cam.IsInNearView = true;
		foreach (GameObject obj in cam.GetObjs()) {
			if (obj != this.gameObject) {
				obj.SetActive(false);
			}
		}
		cam.target = this.transform;
		//cam.zoomDistance = 2.5f;
		cam.distance = 2.5f;
		cam.xSpeed = nearViewSpeedX;
		cam.distanceMax = 2.5f;
		cam.distanceMin = 1.2f;
	}

	public void SetColorToDefault() {
		GetComponent<MeshRenderer>().material = defaultMaterial;
	}

	public void SetColorToGreen() {
		GetComponent<MeshRenderer>().material = ans_green;
	}

	public void SetColorToRed() {
		GetComponent<MeshRenderer>().material = ans_red;
	}

	private void AskNextTeeth() {
		SetColorToDefault();
		HideTeeth();
		cam.isViewPortLocked = false;
		quizPanel.NextQuestion();
	}	

	private void HideTeeth() {
		quizPanel.AddToHiddenTooth(this);
		gameObject.SetActive(false);
	}

	private void DeselectLastRedTeeth () {
		Teeth lastRedTeeth = quizPanel.lastRedTeeth;
		if (lastRedTeeth) {
			quizPanel.lastRedTeeth.SetColorToDefault();
		}
	}

	public void HideAllViews() {
		HideView(buccalView);
		HideView(lingualView);
		HideView(mesialView);
		HideView(distalView);
		HideView(incisalView);
	}

	public void UnhideAllViews() {
		UnhideView(buccalView);
		UnhideView(lingualView);
		UnhideView(mesialView);
		UnhideView(distalView);
		UnhideView(incisalView);
	}

	public void HideView (GameObject view) {
		Part[] childParts = view.GetComponentsInChildren<Part>(true);
		foreach(Part part in childParts) {
			part.gameObject.SetActive(false);
		}
	}

	public void UnhideView (GameObject view) {
		Part[] childParts = view.GetComponentsInChildren<Part>(true);
		foreach(Part part in childParts) {
			part.gameObject.SetActive(true);
		}
	}
}
