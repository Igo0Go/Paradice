using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovementType
{
    Teleport,
    Steps,
    Smooth
}

public class LessonTransform : MonoBehaviour {
    
    public GameObject Sphere;
    public MovementType MoveType;
    public float XposTarget;
    public float ZposTarget;
    public float Speed;
    public float RotateSpeed;
    public bool Move;
    public bool Rotate;
    
    private bool keyRotate;
    private const float y = 1f;
    private bool keyMove;
    private Vector3 TargetPosition;
    private Vector3 moveVector;

    private void Start()
    { //hi
        MoveType = MovementType.Teleport;
        keyRotate = true;
        keyMove = true;
        Rotate = false;
        Move = false;
    }

    private void Update()
    {
        if(Rotate)
        {
            PlayerRotate();
        }
        if (Move)
        {
            PlayerMove();
        }
    }

    private void PlayerRotate()
    {
        if (MoveType == MovementType.Teleport)
        {
            Vector3 Target = new Vector3(XposTarget, y, ZposTarget);
            transform.LookAt(Target);
            Rotate = false;
        }
        else if (MoveType == MovementType.Steps)
        {
            Vector3 TargetPosition = new Vector3(XposTarget, y, ZposTarget);
            if (keyRotate)
            {
                Instantiate(Sphere, TargetPosition, Quaternion.identity);
                keyRotate = false;
            }
            Vector3 moveVector = TargetPosition - transform.position;
            int c = GetSign(moveVector);
            if (Vectors(transform.forward, moveVector))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector),0.5f);
            }
            else
            {
                transform.forward = moveVector;
                Rotate = false;
            }
        }
        else if (MoveType == MovementType.Smooth)
        {
            Vector3 TargetPosition = new Vector3(XposTarget, y, ZposTarget);
            if (keyRotate)
            {
                Instantiate(Sphere, TargetPosition, Quaternion.identity);
                keyRotate = false;
            }
            Vector3 moveVector = TargetPosition - transform.position;
            int c = GetSign(moveVector);
            if (Vectors(transform.forward, moveVector))
            {
                transform.Rotate(transform.up, c * RotateSpeed * Time.deltaTime);
            }
            else
            {
                transform.forward = moveVector;
                Rotate = false;
            }
        }
    }

    private void PlayerMove()
    {
        if (MoveType == MovementType.Teleport)
        {
            transform.position = new Vector3(XposTarget, y, ZposTarget);
            Move = false;
        }
        else if (MoveType == MovementType.Steps)
        {
            TargetPosition = new Vector3(XposTarget, y, ZposTarget);
            moveVector = TargetPosition - transform.position;
            if (Vector3.Distance(transform.position, TargetPosition) > 0.5f)
            {
                transform.position += moveVector*0.5f;
            }
            else
            {
                transform.position = TargetPosition;
                Move = false;
            }
        }
        else if (MoveType == MovementType.Smooth)
        {
            
            if (keyMove)
            {
                TargetPosition = new Vector3(XposTarget, y, ZposTarget);
                moveVector = TargetPosition - transform.position;
                keyMove = !keyMove;
            }
            float x = Vector3.Distance(transform.position, TargetPosition);
            if (Speed * Time.deltaTime < x)
            {
                transform.position += moveVector.normalized * Speed * Time.deltaTime;
            }
            else
            {
                keyMove = !keyMove;
                Move = false;
                transform.position += moveVector.normalized * x;
            }
        }
    }

    private int GetSign(Vector3 moveVector)
    {
        if (transform.forward.x > moveVector.x)
        {
            Debug.Log("Я слева");
            return -1;
        }
        else if (transform.forward.x < moveVector.x)
        {
            Debug.Log("Я справа");
            return +1;
        }
        else if (transform.forward.z > moveVector.z)
        {
            Debug.Log("Я снизу");
            return -1;
        }
        else if (transform.forward.z < moveVector.z)
        {
            Debug.Log("Я сверху");
            return +1;
        }
        return 1;
    }

    private bool Vectors(Vector3 Face, Vector3 move)
    {
        Vector3 FaceVector = new Vector3(Face.x, 0, Face.z);
        Vector3 MoveVector = new Vector3(move.x, 0, move.z);
        Debug.Log(Vector3.Angle(FaceVector, MoveVector));
        if (Vector3.Angle(FaceVector, MoveVector) < 1)
        {
            return false;
        }
        return true;
    }
}
