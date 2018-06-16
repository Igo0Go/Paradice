using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour {

    public MenuScene ActiveScene;
    public float CameraSpeed;
    public float RotateSpeed;

    private Quaternion _targetRotation;
    private Vector3 _targetPosition;
    private Vector3 _moveVector;
    private bool _move;
    private bool _rotate;
    private int _rotateType;

    void Update()
    {
        if (_rotate)
        {
            CameraRotate();
        }
        if (_move)
        {
            CameraMove();
        }
    }

    private void GetCameraSettings()
    {
        _targetPosition = ActiveScene.CamPos1.transform.position;
        _targetRotation = ActiveScene.CamPos1.transform.rotation;
        _moveVector = _targetPosition - transform.position;
    }

    private void CameraRotate()
    {
        if (transform.rotation != _targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, 0.1f);
        }
        else
        {
            _rotate = false;
        }
    }

    private void CameraMove()
    {
        float x = Vector3.Distance(transform.position, _targetPosition);
        if (CameraSpeed * Time.deltaTime < x)
        {
            transform.position += _moveVector.normalized * CameraSpeed * Time.fixedDeltaTime;
        }
        else
        {
            _move = false;
            transform.position += _moveVector.normalized * x;
        }
    }

    private bool Vectors(Vector3 Face, Vector3 move)
    {

        Vector3 FaceVector = new Vector3(Face.x, 0, Face.z);
        Vector3 MoveVector = new Vector3(move.x, 0, move.z);
        float f = Vector3.Angle(FaceVector, MoveVector);
        Debug.Log(f);
        if (Vector3.Angle(FaceVector, MoveVector) < 1)
        {
            return false;
        }
        return true;
    }

    public void Next()
    {
        if (ActiveScene.Next != null)
        {
            Disactive();
            ActiveScene = ActiveScene.Next;
            GetCameraSettings();
            _rotateType = 1;
            _move = true;
            _rotate = true;
        }

    }

    public void Previos()
    {
        if (ActiveScene.Previos != null)
        {
            Disactive();
            ActiveScene = ActiveScene.Previos;
            GetCameraSettings();
            _rotateType = -1;
            _move = true;
            _rotate = true;
        }
    }

    public void Active()
    {
        ActiveScene.Anim.SetBool("Open", true);
    }

    public void Disactive()
    {
        ActiveScene.Anim.SetBool("Open", false);
    }
}
