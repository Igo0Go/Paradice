using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove2 : MonoBehaviour
{
	public float Sensitivity = 5;
	public float smoothDampTime = 0.1f;
	
	private float yRotation;
	private float currentYRotation;
	private float yRotationVelocity;
	
	void Start () {
		
	}
	
	void Update ()
	{
		yRotation += Input.GetAxis("Mouse X") * Sensitivity;
		currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref currentYRotation, smoothDampTime);
		transform.rotation = Quaternion.Euler(0, yRotation, 0);
	}
}
