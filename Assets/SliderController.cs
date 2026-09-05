using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

	[SerializeField] float maxAngle;
	[SerializeField] float minAngle;
	//public Transform upperFak;
	private Slider slider;
	private CameraController cam;
	private float lastFrameValue = 90f;


	// Use this for initialization
	void Awake () {
		slider = GetComponent<Slider>();
		//upperFak = GameObject.Find("FakAxis").transform;
		cam = FindObjectOfType<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (slider.value != lastFrameValue) {
			cam.touchDisabled = true;
			//upperFak.rotation = Quaternion.Euler(slider.value, 180, 90);
			CameraController.FakAxis.rotation = Quaternion.Euler(slider.value, 180, 90);
			lastFrameValue = slider.value;
		} else {
			cam.touchDisabled = false;
		}

	}
}
