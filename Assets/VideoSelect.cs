using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSelect : MonoBehaviour {

	public string videoKey;

	private string url;
	private string prefix = "http://morphodent.mrde3ign.ir/";
	private string sufix = ".mp4";

	//private GameObject videoPlayer;
	//private VideoScript videoStreamer;

	//private GameObject videoPanel;

	private CameraController cam;
	// Use this for initialization
	void Awake ()
	{
		cam = FindObjectOfType<CameraController>();
		url = prefix + videoKey + sufix;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ButtonClicked ()
	{
		cam.StartVideoPlayer(videoKey);
	}


}
