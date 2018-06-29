using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RUN : MonoBehaviour {
    public GameObject Cam;
    public float Speed;
    public float CamSpeed;
    private CharacterController _controller;
    private Vector3 _moveVector;
    private float _camDistance;
    public float Grav;
    public float JumpForce;
    private float _vertSpeed;
    private float _speed;
    // Use this for initialization
    void Start () {
        _controller = this.GetComponent<CharacterController>();
        _speed = Speed;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}
    private void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            if(z != 0)
            {
                CamMove();
            }
            _moveVector = Cam.transform.right * x + Cam.transform.forward * z;
            _moveVector = new Vector3(_moveVector.x, 0, _moveVector.z);
            transform.forward = _moveVector;
        }

        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vertSpeed = JumpForce;
            }
        }
        _vertSpeed += Grav * Time.deltaTime;
        _moveVector = new Vector3(_moveVector.x * _speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime, _moveVector.z * _speed * Time.fixedDeltaTime);
        if (_moveVector != Vector3.zero)
        {
            _controller.Move(_moveVector);
        }
    }

    private void CamMove()
    {
        float distance = Vector3.Distance(Cam.transform.position, transform.position);
        Vector3 lookVector = transform.position - Cam.transform.position;
        if (distance <_camDistance)
        {
            float stepCam = CamSpeed * Time.deltaTime;
            if(stepCam < _camDistance)
            {
                Cam.transform.position += lookVector.normalized * stepCam;
            }
            else
            {
                Cam.transform.position = transform.position - lookVector.normalized * _camDistance;
            }
        }
        if (distance > _camDistance)
        {
            float stepCam = CamSpeed * Time.deltaTime;
            if (stepCam > _camDistance)
            {
                Cam.transform.position -= lookVector.normalized * stepCam;
            }
            else
            {
                Cam.transform.position = transform.position + lookVector.normalized * _camDistance;
            }
        }
    }
}
