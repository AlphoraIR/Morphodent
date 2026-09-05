using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizPanel : MonoBehaviour {

	//public bool quizMode = false;
	public Part askedPart;
	public Teeth askedTeeth;

	public Part lastRedPart;
	public Teeth lastRedTeeth;




	//private Transform mouthModel;
		//[SerializeField] private Button[] options;
	private CameraController cam;
	private List<Part> askedParts = new List<Part>();
	private List<Teeth> askedTooth = new List<Teeth>();
	private Text quizText;

	private Sprite quizEnterSprite, quizExitSprite;

	private Image quizButton;

	private List<Part> hiddenParts = new List<Part>();
	private List<Teeth> hiddenTooth = new List<Teeth>();

	private AudioSource audioSource;

	private const int ANSWERS_PER_LEVEL = 10;
	private const int SCORE_PER_CORRECT = 10;
	private const int STREAK_BONUS_PER_STEP = 2;
	private const int MAX_STREAK_BONUS_STEPS = 5;

	private Text scoreText;
	private int score;
	private int level;
	private int correctStreak;


	// Use this for initialization
	void Awake ()
	{
		//mouthModel = GameObject.Find("MouthModel").transform;
		cam = FindObjectOfType<CameraController> ();
		quizText = transform.Find ("QuizText").GetComponent<Text> ();
		quizButton = GameObject.Find ("Quiz Button").GetComponent<Image> ();
		quizEnterSprite = Resources.Load<Sprite> ("Icons\\QuizEnter");
		quizExitSprite = Resources.Load<Sprite> ("Icons\\QuizExit");
		audioSource = GetComponent<AudioSource> ();
		if (PlayerPrefs.HasKey ("Volume")) {
			audioSource.volume = PlayerPrefs.GetFloat("Volume");
		}

		score = PlayerPrefs.GetInt ("QuizScore", 0);
		level = 1 + PlayerPrefs.GetInt ("QuizTotalCorrect", 0) / ANSWERS_PER_LEVEL;

		scoreText = Instantiate (quizText, quizText.transform.parent);
		scoreText.name = "QuizScoreText";
		scoreText.rectTransform.anchoredPosition = quizText.rectTransform.anchoredPosition + new Vector2 (0f, 60f);
		scoreText.text = "";

		HideQuizPanel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetPrefs () {
		if (PlayerPrefs.HasKey ("Volume")) {
			audioSource.volume = PlayerPrefs.GetFloat("Volume");
		}
	}


	public void QuizButtonPressed ()
	{
		if (!cam.quizMode) {
			if (cam.isViewPortLocked) {
				cam.UnlockViewport();
			}
			EnterQuizMode ();
			NextQuestion ();
		} else if(!cam.isViewPortLocked) {
			ExitQuizMode();
		}

	}




	private Part GenPart (GameObject view) {
		Part result;
		result = view.transform.GetChild (Random.Range (0, view.transform.childCount)).gameObject.GetComponent<Part> ();
		while (askedParts.Contains (result)) {
			if (askedParts.Count >= view.transform.childCount) {
				ExitQuizMode();
				result = null;
				break;
			} else {
				result = view.transform.GetChild (Random.Range (0, view.transform.childCount)).gameObject.GetComponent<Part> ();
			}
		}
		return result;
	}

	private Teeth GenTeeth ()
	{
		if (askedTooth.Count >= 32) {
			ExitQuizMode ();
			return null;
		}
		Teeth result;
		result = FindObjectsOfType<Teeth>()[Random.Range (0, 32 - askedTooth.Count)];
		while (askedTooth.Contains (result)) {
			if (askedTooth.Count >= 32) {
				ExitQuizMode();
				result = null;
				break;
			} else {
				result = FindObjectsOfType<Teeth>()[Random.Range (0, 32 - askedTooth.Count)];
			}
		}
		return result;
	}

	public void NextQuestion ()
	{
		if (cam.IsInNearView) {
			if (askedPart) {
				askedPart.SetColorToUnselected ();
			}
			askedPart = GenPart (cam.activeTeeth.activeView);
			if (askedPart) {
				askedParts.Add (askedPart);
				quizText.text = askedPart.name + " ?";
			} else {
				quizText.text = "";
			}

		} else {
			if (askedTeeth) {
				askedTeeth.SetColorToDefault ();
			}
			askedTeeth = GenTeeth ();
			if (askedTeeth) {
				askedTooth.Add (askedTeeth);
				quizText.text = askedTeeth.teethName + " ?";
			} else {
				quizText.text = "";
			}
		}

	}


	private List<GameObject> GetChildWithTag (Transform parent, string tag)
	{
		List<GameObject> result = new List<GameObject>();
		for (int i = 0; i < parent.childCount; i++) {
			if (parent.GetChild (i).tag == tag) {
				result.Add(parent.GetChild(i).gameObject);
			}
		}
		return result;
	}

	private void EnterQuizMode ()
	{	
		cam.ShowPins();
		cam.HidePinsButton();
		askedParts.Clear();
		askedTooth.Clear();
		cam.quizMode = true;
		cam.HideBackButton ();
		cam.HideInfoButton ();
		cam.HideLockButton ();
		quizButton.sprite = quizExitSprite;
		cam.SetPartTextHolderValue ("");
		//cam.SetPartTitleValue ("");
		if (cam.IsInNearView) {
			if (cam.activeTeeth.activePart) {
				cam.activeTeeth.activePart.SetColorToUnselected ();
			}
		} else {

		}

		correctStreak = 0;
		UpdateScoreText ();

	}

	private void ExitQuizMode ()
	{
		if (cam.IsInNearView) {
			cam.HidePins();
			cam.UnhidePinsButton();
			UnhideAllHiddenParts ();
			cam.UnhideBackButton ();
		} else {
			UnhideAllHiddenTooth();
		}
		cam.UnhideInfoButton ();
		HideQuizPanel ();
		cam.quizMode = false;
		cam.UnhideLockButton ();
		quizButton.sprite = quizEnterSprite;
		askedParts.Clear();
		askedTooth.Clear();
		scoreText.text = "";
	}

	public void RegisterCorrectAnswer () {
		correctStreak++;
		int totalCorrect = PlayerPrefs.GetInt ("QuizTotalCorrect", 0) + 1;
		PlayerPrefs.SetInt ("QuizTotalCorrect", totalCorrect);

		int streakBonusSteps = Mathf.Min (correctStreak - 1, MAX_STREAK_BONUS_STEPS);
		score += SCORE_PER_CORRECT + streakBonusSteps * STREAK_BONUS_PER_STEP;
		PlayerPrefs.SetInt ("QuizScore", score);

		int newLevel = 1 + totalCorrect / ANSWERS_PER_LEVEL;
		if (newLevel > level) {
			level = newLevel;
			CameraController.ShowAlertBox ("تبریک! به سطح " + level + " رسیدید.");
		}

		UpdateScoreText ();
	}

	public void RegisterWrongAnswer () {
		correctStreak = 0;
		UpdateScoreText ();
	}

	private void UpdateScoreText () {
		scoreText.text = "سطح " + level + "   |   امتیاز " + score;
	}

	public void HideQuizPanel () {
		transform.Find("QuizText").GetComponent<Text>().text = "";
	}

	public void AddToHiddenParts(Part part) {
		hiddenParts.Add(part);
	}

	private void UnhideAllHiddenParts ()
	{
		foreach (Part part in hiddenParts) {
			part.gameObject.SetActive(true);
		}
		hiddenParts.Clear();
	}

	private void UnhideAllHiddenTooth ()
	{
		foreach (Teeth teeth in hiddenTooth) {
			teeth.gameObject.SetActive(true);
		}
		hiddenTooth.Clear();
	}

	public void AddToHiddenTooth(Teeth teeth) {
		hiddenTooth.Add(teeth);
	}


	/*private int GetRndInt (int min, int max, int[] exc)
	{
		int rndInt = Random.Range (min, max);
		if (rndInt == exc [0] || rndInt == exc [1] || rndInt == exc [2]) {
			GetRndInt (min, max, exc);
		} else {
			return rndInt;
		}
	}*/
}
