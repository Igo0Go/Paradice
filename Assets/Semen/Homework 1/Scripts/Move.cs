using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoveType
{
	Steps,
	Smooth
}

public class Move : MonoBehaviour
{

	public float XposTarget;
	public float ZposTarget;
	public float Speed;
	public MoveType MoveType;
	public bool Go;

	private const float Y = 1f;
	private Vector3 _targetPosition;
	private Vector3 _moveVector;
	
	void Start ()
	{
		Go = false;
		Speed = 0.3f;
		_targetPosition = new Vector3();
		_moveVector = new Vector3();
		MoveType = MoveType.Steps;
	}
	
	void Update ()
	{
		if (Go)
		{
			GoGoGo();
		}
	}

	private void GoGoGo()
	{
		_targetPosition.x = XposTarget;
		_targetPosition.y = Y;
		_targetPosition.z = ZposTarget;

		_moveVector = _targetPosition - transform.position;
		transform.forward = _moveVector;	

		float distance = Vector3.Distance(transform.position, _targetPosition);
		
			if (MoveType == MoveType.Steps)
			{
				if (distance > 1)
				{
					transform.position += _moveVector * Speed * 1;
				}
				else
				{
					transform.position = _targetPosition;
					Go = !Go;
				}
			}

			if (MoveType == MoveType.Smooth)
			{
				if (distance > Speed * Time.deltaTime)
				{
					transform.position += _moveVector * Speed * Time.deltaTime;
				}
				else
				{
					transform.position = _targetPosition;
					Go = !Go;
				}
			}
		
	}
}
