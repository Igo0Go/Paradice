using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelictusController : MonoBehaviour {

    public Animator Connect;
    public Slider Energy;
    public float Speed;
    public float RotateSpeed;
    public float MaxVert;
    public float MinVert;
    public int EnergySpeed;

    private CharacterController _controller;
    private Vector3 _moveVector;
    private float _rotationX;
    private float _rotationY;
    //sad
    void Start () {
        Energy.value = 100;
        EnergySpeed = 1;
        _controller = GetComponent<CharacterController>();
        _rotationX = _rotationY = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	void Update () {
        Energy.value -= 0.001f * EnergySpeed*Time.deltaTime;
        RelictusMove();
        Rotate();
        MaxSpeed();
	}

    private void RelictusMove()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            _moveVector = transform.right * x + transform.forward * z;
            _moveVector = new Vector3(_moveVector.x, 0, _moveVector.z);
            if (_moveVector != Vector3.zero)
            {
                EnergySpeed = 5;
                _controller.Move(_moveVector.normalized * Speed * Time.fixedDeltaTime);
            }
        }
    }

    private void Rotate()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            var h = Input.GetAxis("Mouse X");
            var v = Input.GetAxis("Mouse Y");
            _rotationX = transform.localEulerAngles.y + h * RotateSpeed;
            _rotationY += v * RotateSpeed;
            _rotationY = Mathf.Clamp(_rotationY, MinVert, MaxVert);
            transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
            EnergySpeed = 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("JumpPoint"))
        {
            Connect.SetBool("Active", true);
            other.GetComponent<Animator>().SetBool("Active", true);
            EnergySpeed = 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("JumpPoint"))
        {
            Connect.SetBool("Active", false);
            other.GetComponent<Animator>().SetBool("Active", false);
            EnergySpeed = 1;
        }
    }

    private void MaxSpeed()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 8;
            EnergySpeed = 10;
        }
        else
        {
            Speed = 4;
            EnergySpeed = 1;
        }
    }
}
