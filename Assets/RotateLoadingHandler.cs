using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateLoadingHandler : MonoBehaviour {

	private float rotateSpeed = 200f;
	private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		rectTransform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
	}
}
