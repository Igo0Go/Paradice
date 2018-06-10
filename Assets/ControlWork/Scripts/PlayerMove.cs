﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
	
    private float _x;
    private float _y;
    private CharacterController _controller; //sad
    private Vector3 _moveVector;

    //гравитация
    private float _grav;
    private float _jumpSpeed;
    private float _vertSpeed;

    

    void Start()
    {
        _grav = -9.8f;
        _jumpSpeed = 5;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _y = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Speed;
        _x += Input.GetAxis("Mouse Y") * Speed;
        _x = Mathf.Clamp(_x, -90, 80);
        transform.localEulerAngles = new Vector3(-_x, _y, 0);
        
        RelictusMove();
        MaxSpeed();
    }

    private void RelictusMove()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        _moveVector = transform.right * x + transform.forward * z;
        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vertSpeed = _jumpSpeed;
            }
        }

        _vertSpeed += _grav * Time.deltaTime;
        _moveVector = new Vector3(_moveVector.x * Speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime,
            _moveVector.z * Speed * Time.fixedDeltaTime);
        if (_moveVector != Vector3.zero)
        {
            _controller.Move(_moveVector);
        }
    }


    private void MaxSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 8;
        }
        else
        {
            Speed = 4;
        }
    }
}