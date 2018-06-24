using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{

	public float Speed;

	private float _x;
	private float _y;
	
	void Start ()
	{
		Speed = 1f;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update ()
	{
		_y = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Speed;
		_x += Input.GetAxis("Mouse Y") * Speed;
		_x = Mathf.Clamp (_x, -90, 80);
		transform.localEulerAngles = new Vector3(-_x, _y, 0);

		if (Input.GetKey(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	
}
