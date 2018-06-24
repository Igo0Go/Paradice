using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour {

    public GameObject Box1;
    public GameObject Box2;
    public float Speed;
    public bool Stop;
    public bool Reset;

    private Vector3 _moveVector;
    private Vector3 _start;
    private Vector3 _target;

	void Start () {
        _start = Box1.transform.position;
        _target = Box2.transform.position;
        _moveVector = _target - _start;
	}
	
	void Update () {

        if(Reset)
        {
            ResetBox();
        }

        if(!Stop)
        {
            MoveToTarget();
        }
	}

    private void MoveToTarget()
    {
        var x = Vector3.Distance(transform.position, _target);
        if(Speed*0.1f < x)
        {
            transform.position += _moveVector.normalized * Speed * 0.01f;
        }
        else
        {
            transform.position += _moveVector.normalized * x;
            transform.position = _start;
        }
    }

    public void ResetBox()
    {
        var c = _start;
        _start = _target;
        _target = c;
        _moveVector = _target - _start;
        Reset = false;
    }
}
