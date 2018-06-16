using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour {

    public MenuScene ActiveScene;
    public float CameraSpeed;

    private Quaternion _targetRotation;
    private Vector3 _targetPosition;
    private Vector3 _moveVector;
    private bool _newVector;
    private bool _move;
    private bool _rotate;

	void Start () {
        _newVector = false;
	}
	
	
	void Update () {
		if(_rotate)
        {
            CameraRotate();
        }
        if(_move)
        {
            CameraMove();
        }
	}

    private void GetCameraSettings()
    {
        _targetPosition = ActiveScene.CamPos1.transform.position;
        _targetRotation = ActiveScene.CamPos1.transform.rotation;
    }

    private void CameraRotate()
    {
        transform.rotation = _targetRotation;
        _rotate = false;
    }
        

    private void CameraMove()
    {
        if(_newVector)
        {
            _moveVector = _targetPosition = transform.position;
            _newVector = false;
        }
        float x = Vector3.Distance(transform.position, _targetPosition);
        if (CameraSpeed * Time.deltaTime < x)                                     
        {
            transform.position += _moveVector.normalized * CameraSpeed * Time.deltaTime; 
        }                                                                          
        else                                                                  
        {
            _move = false;
            transform.position += _moveVector.normalized * x;                       
        }
    }

    public void Next()
    {
        if(ActiveScene.Next != null)
        {
            ActiveScene = ActiveScene.Next;
            GetCameraSettings();
            transform.rotation = _targetRotation;
            transform.position = _targetPosition;
            //_rotate = true;
            //_move = true;
            //_newVector = true;
        }
        
    }

    public void Previos()
    {
        if(ActiveScene.Previos != null)
        {
            ActiveScene = ActiveScene.Previos;
            GetCameraSettings();
            transform.rotation = _targetRotation;
            transform.position = _targetPosition;
            //_rotate = true;
            //_move = true;
        }
    }

    public void Active()
    {
        ActiveScene.Anim.SetTrigger("Open");
    }
}
