using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetColor
{
    Red,
    Green,
    Yelow,
    Blue,
    White
}

public enum MovingType
{
    Teleport,
    Smooth
}


public class TransformToObject : MonoBehaviour {

    public List<GameObject> Targets;
    public MovingType MoveType;
    public TargetColor Target;
    public float RotationSpeed;
    public float MoveSpeed;
    public bool Move;

    private bool _rotate;
    private bool _move;
    private GameObject _target;

	// Use this for initialization
	void Start () {
        _move = false;
        _rotate = false;
        Move = false;
        MoveType = MovingType.Teleport;
        Target = TargetColor.Blue;
	}
	
	// Update is called once per frame
	void Update () {
		if(Move)
        {
            if(Move)
            {
                GetTarget();
            }
            if(_rotate)
            {
                PlayerRotate(_target);
            }
            if(_move)
            {
                PlayerMove(_target);
            }
        }
	}

   
    private void PlayerRotate(GameObject target)
    {
        Vector3 moveVector = target.transform.position - transform.position;
        int c = GetSign(moveVector);
        if (Vectors(transform.forward, moveVector))
        {
            transform.Rotate(transform.up, c*RotationSpeed*Time.deltaTime);
        }
        else
        {
            _rotate = false;
            _move = true;
        }
    }

    private void PlayerMove(GameObject target)
    {
        if(MoveType == MovingType.Teleport)
        {
            transform.position = target.transform.position;
            _move = false;
            Move = false;
        }
        else
        {
            Vector3 targetPos = target.transform.position;
            Vector3 moveVector = targetPos - transform.position;
            var x = Vector3.Distance(targetPos, transform.position);
            if(MoveSpeed*Time.deltaTime < x)
            {
                transform.position += moveVector.normalized * MoveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += moveVector.normalized * x;
                _move = false;
                Move = false;
            }
        }
    }

    private bool Vectors(Vector3 Face, Vector3 move)
    {
        Vector3 FaceVector = new Vector3(Face.x, 0, Face.z);
        Vector3 MoveVector = new Vector3(move.x, 0, move.z);

        if (Vector3.Angle(FaceVector, MoveVector) < 1)
        {
            return false;
        }
        return true;
    }
    
    private int GetSign(Vector3 moveVector)	
    {
        int c = 0;
        if (transform.forward.x > moveVector.x)
        {
            c++;
        }
        else if (transform.forward.x < moveVector.x)		
        {
            c--;
        }
        if (transform.forward.z > moveVector.z)		
        {
            c++;
        }
        else if (transform.forward.z < moveVector.z)		
        {
             c--;
        }
        if(c==0)
        {
            return 1;
        }
        else
        {
            return c;
        }
    }

    private void GetTarget()
    {
        switch (Target)
        {
            case TargetColor.Blue:
                _target = Targets[0];
                _rotate = true;
                break;
            case TargetColor.Green:
                _target = Targets[1];
                _rotate = true;
                break;
            case TargetColor.Red:
                _target = Targets[2];
                _rotate = true;
                break;
            case TargetColor.White:
                _target = Targets[3];
                _rotate = true;
                break;
            case TargetColor.Yelow:
                _target = Targets[4];
                _rotate = true;
                break;
        }
    }
}
