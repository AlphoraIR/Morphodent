using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCam : MonoBehaviour {

	public Transform target;

	public float distanceMin, distanceMax;


	Vector3 f0Dir= Vector3.zero;
	public float zoomDistance = 5;
	float theta= 0.0F;
	float fai= 0.0F;
	float dx= 0.0F;
	float dy= 0.0F;
	float loc_x= 0.0F;
	float loc_y= 0.0F;
	float delta= 0.0F;
	float deltaWeight= 0.05F;
	Vector2 curDist= Vector2.zero;
	Vector2 prevDist= Vector2.zero;
	Transform dm;
	
	Vector3 upVal= Vector3.zero;
	Vector3 pos= new Vector3(0, 0, 0);
	Vector3 rot= new Vector3(0, 0, 0);

	public bool IsInNearView = false;
	public Material blackBG;

	private Transform mouthModel;
	GameObject[] objs;

	private float nearViewLight = 0.6f;
	private float farViewLight = 3f;

	private float farViewSpeedX = 3f;

	private Quaternion startRotation;
	private Vector3 startPosition;

	private Light pointLight;

	void Start() {	
         mouthModel = GameObject.Find("MouthModel").transform;
         pointLight = GetComponentInChildren<Light>();
		objs = GameObject.FindGameObjectsWithTag("obj");

		startRotation = transform.rotation;
		startPosition = transform.position;
     }

	void Update ()
	{
		/*if (Input.GetMouseButton (0)) {
			Touch f0 = Input.GetTouch(0);
			Vector3 f0Delta2 = new Vector3(f0.deltaPosition.x, -f0.deltaPosition.y, 0);
			f0Dir= f0Delta2;
			loc_x= Mathf.Deg2Rad*f0Dir.x*1;
			loc_y= -Mathf.Deg2Rad*f0Dir.y*1;
		}*/

		if (Input.touchCount == 1) {
			Touch f0 = Input.GetTouch(0);
			Vector3 f0Delta2 = new Vector3(f0.deltaPosition.x, -f0.deltaPosition.y, 0);
			f0Dir= f0Delta2;
			loc_x= Mathf.Deg2Rad*f0Dir.x*1;
			loc_y= -Mathf.Deg2Rad*f0Dir.y*1;
			
		} else if( Input.touchCount== 2 ) {
			loc_x= 0.0F;
			loc_y= 0.0F;
			Touch f0= Input.GetTouch(1);
			Touch f1= Input.GetTouch(0);
			Vector3 f0Delta= new Vector3(-f0.deltaPosition.x, -f0.deltaPosition.y, 0);
			Vector3 f1Delta= new Vector3(-f1.deltaPosition.x, -f1.deltaPosition.y, 0);
			float toDirection= Vector3.Dot(f0Delta, f1Delta);
			Debug.Log(toDirection);

			if(f1.phase == TouchPhase.Moved && f0.phase == TouchPhase.Moved){
				if( toDirection > 0 ) {
					f0Dir= Vector3.zero;
					dy+= f0Delta.y*0.01F;
					dx+= f0Delta.x*0.01F;
					loc_x= f0Delta.x*0.01F;
					loc_y= f0Delta.y*0.01F;
				} else {
					curDist = f0.position - f1.position;
					prevDist = (f0.position - f0.deltaPosition) - (f1.position - f1.deltaPosition);
					float delta = curDist.magnitude - prevDist.magnitude;
					zoomDistance= zoomDistance+(-delta*deltaWeight);
				}
			}
		} else {
			f0Dir= Vector3.zero;
			loc_x= 0.0F;
			loc_y= 0.0F;
		}
		zoomDistance = Mathf.Clamp(zoomDistance - Input.GetAxis("Mouse ScrollWheel") * 2, distanceMin, distanceMax);

		theta+= Mathf.Deg2Rad*f0Dir.x*1;
		fai+=   -Mathf.Deg2Rad*f0Dir.y*1;

		upVal.z= zoomDistance*Mathf.Cos(theta)*Mathf.Sin(fai+Mathf.PI/2);
		upVal.x= zoomDistance*Mathf.Sin(theta)*Mathf.Sin(fai+Mathf.PI/2);
		upVal.y= zoomDistance*Mathf.Cos(fai+Mathf.PI/2);

		transform.position= upVal;
		target.transform.Translate( Camera.main.transform.up*loc_y+Camera.main.transform.right*(loc_x), Space.World);
		transform.position+= target.position;
		Camera.main.transform.LookAt(target.position);

		blackBG.SetFloat("_Rotation", -transform.rotation.eulerAngles.y);
		pointLight.intensity = zoomDistance/2.333f;
	}

	public void BackToModel ()
	{	
		Teeth[] activeObjs = GameObject.FindObjectsOfType<Teeth>();
		foreach (Teeth obj in activeObjs) {
			obj.gameObject.transform.rotation = obj.farViewRotation;
			obj.SetColliderState(true);
		}
		ShowAllObjects ();
		pointLight.intensity = farViewLight;
		target = mouthModel;
		zoomDistance = 7f;
		//xSpeed = farViewSpeedX;
		distanceMax = 7.5f;
		distanceMin = 5f;

     }

	void ShowAllObjects () {
		IsInNearView = false;
		foreach (GameObject obj in objs) {
			obj.SetActive(true);
		}
	}

	public void ResetCam () {
		transform.rotation = startRotation;
		transform.position = startPosition;
	}
}
