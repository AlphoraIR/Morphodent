using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Part : MonoBehaviour {

	//private Text textHolder;
	//private Text title;

	private Material green, pink;

	private Material ans_red, ans_green;

	//private TouchCam cam;
	private CameraController cam;

	private Teeth parentTeeth;

	private QuizPanel quizPanel;


	// Use this for initialization
	void Awake () {
		parentTeeth = GetComponentInParent<Teeth>();
		//cam = FindObjectOfType<TouchCam> ();
		cam = FindObjectOfType<CameraController> ();
		quizPanel = FindObjectOfType<QuizPanel>();
		ans_red = Resources.Load<Material>("Materials\\Red");
		ans_green = Resources.Load<Material>("Materials\\Green");
		green = Resources.Load<Material>("Materials\\Selection\\Green");
		pink = Resources.Load<Material>("Materials\\Selection\\Pink");
	}

	void Update () {
	}
	
	// Update is called once per frame

	void OnMouseDown ()
	{
		if (cam.IsInNearView) {
			if (!cam.quizMode) {
				//cam.SetPartTitleValue("Part Name:");
				cam.SetPartTextHolderValue(gameObject.name);
				SelectPart(gameObject);
				SetColorToSelected();
				parentTeeth.activePart = this;
			} else {
				if (quizPanel.askedPart == this) {
					cam.isViewPortLocked = true;
					DeselectLastRedPart();
					SetColorToGreen ();
					quizPanel.RegisterCorrectAnswer ();
					quizPanel.GetComponent<AudioSource>().Play();
					Invoke("AskNextPart", 1f);
				} else {
					DeselectLastRedPart ();
					SetColorToRed();
					quizPanel.RegisterWrongAnswer ();
					Invoke("SetColorToUnselected", 1f);
					quizPanel.lastRedPart = this;
				}
			}
		}

	}

	void SelectPart (GameObject part)
	{
		if (parentTeeth.lastSelectedPart) {
			parentTeeth.lastSelectedPart.SetColorToUnselected();
		}
		SetColorToSelected();
		parentTeeth.lastSelectedPart = this;
	}

	private void SetPartColorTo (Material mat)
	{	
		GetComponent<MeshRenderer>().material = mat;
		GameObject partAxis = transform.Find("Axis").gameObject;
		partAxis.GetComponent<MeshRenderer>().material = mat;
	}	

	public void SetColorToUnselected() {
		SetPartColorTo(pink);
	}

	public void SetColorToSelected() {
		SetPartColorTo(green);
	}

	public void SetColorToRed() {
		SetPartColorTo(ans_red);
	}

	public void SetColorToGreen() {
		SetPartColorTo(ans_green);
	}

	private void DeselectLastRedPart () {
		Part lastRedPart = quizPanel.lastRedPart;
		if (lastRedPart) {
			quizPanel.lastRedPart.SetColorToUnselected();
		}
	}

	private void AskNextPart() {
		SetColorToUnselected();
		HidePart();
		cam.isViewPortLocked = false;
		quizPanel.NextQuestion();
	}

	private void HidePart() {
		quizPanel.AddToHiddenParts(this);
		gameObject.SetActive(false);
	}
}
