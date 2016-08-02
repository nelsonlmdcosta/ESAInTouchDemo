using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {


	public float sensX = 1.0f;
	public float sensY = 1.0f;

	float rotationY = 0.0f;
	float rotationX = 0.0f;
	private Quaternion previousPosition;
	private Camera cam;
	private bool downMouse2 = false;
	void Start(){
		
		cam = gameObject.GetComponent<Camera>();
	
	}
	void Update () {
		
		if (Input.GetMouseButton (2)) {
			rotationX += Input.GetAxis ("Mouse X") * sensX * Time.deltaTime;
			rotationY += Input.GetAxis ("Mouse Y") * sensY * Time.deltaTime;
			if (downMouse2 == false) {
				previousPosition = transform.localRotation;
				downMouse2 = true;
			}
			transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0) + previousPosition.eulerAngles;
			if (Input.GetMouseButtonUp (2)) {
				downMouse2 = false;
			}	
		} else {
			previousPosition = transform.localRotation;
		}
		cam.fieldOfView= Mathf.Clamp(cam.fieldOfView - Input.GetAxis("Mouse ScrollWheel")*10f, 2f,100f);
		if (Input.GetKeyDown(KeyCode.Z)) {
			cam.fieldOfView = 60f;
		}


	}

}
