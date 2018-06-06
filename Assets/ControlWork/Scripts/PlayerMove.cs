using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	public float Speed;
	public float RotationSpeed;
	
	private float _x;
	private float _y;
	private Rigidbody _body;
	private Vector3 _moveVector;
	private float horizontal;
	private float vertical;
	
	void Start ()
	{
		Speed = 1.5f;
		RotationSpeed = 1f;
		_body = GetComponent<Rigidbody>();
	}
	
	void Update () {
		_y = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * RotationSpeed;
		_x += Input.GetAxis("Mouse Y") * Speed;
		_x = Mathf.Clamp(_x, -90, 80);
		transform.localEulerAngles = new Vector3(-_x, _y, 0);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.visible = true;
		}
	}

	private void FixedUpdate()
	{
		/*if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Speed = 2;
			}
			else
			{
				Speed = 1;
			}
			
			_body.drag = 0;
			
			float _right = Input.GetAxisRaw("Horizontal");
			float _forward = Input.GetAxisRaw("Vertical");

			_body.AddForce(transform.forward * _forward * Speed, ForceMode.VelocityChange);
			_body.AddForce(transform.right * _right * Speed, ForceMode.VelocityChange);
			
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");
			_body.AddForce(((transform.right * horizontal) + (transform.forward * vertical)) * Speed);
			
			if(Mathf.Abs(_body.velocity.x) > Speed)
			{
				_body.velocity = new Vector3(Mathf.Sign(_body.velocity.x) * Speed, _body.velocity.y, _body.velocity.z);
			}
			if(Mathf.Abs(_body.velocity.z) > Speed)
			{
				_body.velocity = new Vector3(_body.velocity.x, _body.velocity.y, Mathf.Sign(_body.velocity.z) * Speed);
			}
		}
		else
		{
			_body.drag = 20;
		}*/
		
		if (Input.GetKey(KeyCode.LeftShift))
		{
			Speed = 2;
		}
		else
		{
			Speed = 1;
		}
		
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		_body.AddForce(((transform.right * horizontal) + (transform.forward * vertical)) * Speed);
	}

}
