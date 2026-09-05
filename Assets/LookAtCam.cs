using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour {

	private CameraController cam;

	// Use this for initialization
	void Start () {
		cam = FindObjectOfType<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(cam.transform);
	}
}
