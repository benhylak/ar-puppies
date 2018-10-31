using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public GameObject _objectToTrack;
	public Camera _camera;
	float angularVelocity = 10f;

	float bufferZone = 0.3f;

	Quaternion _targetRot;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera>();
		_targetRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPoint = _camera.WorldToViewportPoint(_objectToTrack.transform.position);
 		bool comfortablyOnScreen = screenPoint.z > 0 
		 	&& screenPoint.x > 0 + bufferZone 
			&& screenPoint.x < 1 - bufferZone
			&& screenPoint.y > 0 + bufferZone
			&& screenPoint.y < 1 - bufferZone;
		
		if(!comfortablyOnScreen)
		{
			var lookDir = _objectToTrack.transform.position - this.transform.position;
			_targetRot = Quaternion.LookRotation(lookDir);
		}

		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _targetRot, 1f * Time.deltaTime);
	}
}
