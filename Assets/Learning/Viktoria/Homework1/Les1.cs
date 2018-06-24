using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType1
{
	Teleport,
	Smooth
}

public class MyTransform : MonoBehaviour
{
	public List<GameObject> GOs;

	public MovementType1 MoveType;
	public float Speed;
	public bool Move;
	
	private byte step = 0;
	bool key = true;
	private Vector3 moveVector;
	private Vector3 Position;
	
	void Start ()
	{
		MoveType = MovementType1.Teleport;
		Move = false;
		Speed = 3;
	}
	
	void Update () 
	{
		if (Move)
		{
			PlayerMove();
			
			if(step < GOs.Count - 1) step++;
			else
			{
				step = 0;
			}
		}
	}

	private void PlayerMove()                    //вообще-то так до конца и не поняла эту штуку (*-_-)
	{
		if (MoveType == MovementType1.Smooth)
		{
			if (key)
			{
				Position = new Vector3(GOs[step].transform.position.x,transform.position.y,GOs[step].transform.position.z);
				moveVector = GOs[step].transform.position - transform.position;
				key = !key;
			}
			
			if (Speed * 0.02f < Vector3.Distance(Position, transform.position))
			{
				transform.position += moveVector * Speed * 0.02f;
			}
			
			Move = false;
		}
		
		if (MoveType == MovementType1.Teleport)
		{
			Vector3 pos = new Vector3(GOs[step].transform.position.x, transform.position.y, GOs[step].transform.position.z);
			transform.LookAt(pos);
			transform.position = pos;
			
			Move = false;
		}
	}
}
