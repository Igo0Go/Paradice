﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumper : MonoBehaviour {

    public PauseScript pauseScript;
    public GameObject Cam;
    public GameObject Body;
    public float ForceJump;
    public float ForceGrav;
    public float Speed;

    private Vector3 _moveVector;
    private Vector3 _standartCamPos;
    private Vector3 _camOfset;
    private Animator _anim;
    private const float k_GroundRayLength = 1f;
    private AsyncOperation _async;

    private CharacterController _controller;
    private float _speed;
    private float _rotationX;
    private float _rotationY;
    private Vector3 _savePosition;

    //гравитация
    private float _grav;
    private float _jumpSpeed;
    private float _vertSpeed;

    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _grav = -ForceGrav;
        _jumpSpeed = ForceJump;
        _standartCamPos = Cam.transform.position;
        _camOfset = Cam.transform.position - transform.position;
        _anim = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

   
    private void FixedUpdate()
    {
        Cam.transform.LookAt(transform.position);
        Move();
        CamMove();
    }

    //private void Jump()
    //{
    //    _rigidbody.AddForce(Vector3.up * ForceJump, ForceMode.Impulse);
    //}

    //private void MoveJumper()
    //{
    //    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
    //    {
    //        var x = Input.GetAxis("Horizontal");
    //        var z = Input.GetAxis("Vertical");
    //        _moveVector = Cam.transform.right * x + Cam.transform.forward * z;
    //        _moveVector = new Vector3(_moveVector.x, 0, _moveVector.z);
    //        if (_moveVector != Vector3.zero)
    //        {
    //            _anim.SetBool("Move", true);
    //            transform.position += _moveVector.normalized*Speed*Time.fixedDeltaTime;
    //            transform.forward = _moveVector.normalized;
    //        }
    //    }
    //    else
    //    {
    //        _anim.SetBool("Move", false);
    //    }
    //}
    private void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            _moveVector = Cam.transform.right * x + Cam.transform.forward * z;
            _moveVector = new Vector3(_moveVector.x, 0, _moveVector.z);
            if (_moveVector != Vector3.zero)
            {
                _anim.SetBool("Move", true);
                transform.position += _moveVector.normalized * Speed * Time.fixedDeltaTime;
                transform.forward = _moveVector.normalized;
            }
        }
        else
        {
            _anim.SetBool("Move", false);
        }
        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vertSpeed = _jumpSpeed;
            }
        }
        _vertSpeed += _grav * Time.deltaTime;
        _moveVector = new Vector3(_moveVector.x * _speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime,
            _moveVector.z * _speed * Time.fixedDeltaTime);
        if (_moveVector != Vector3.zero)
        {
            _controller.Move(_moveVector);
        }
    }
    private void CamMove()
    {
        Cam.transform.position = new Vector3(_standartCamPos.x, transform.position.y + _camOfset.y, _standartCamPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Durk"))
        {
            pauseScript.RetryButtonClick();
        }
    }//

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("SavePoint"))
        {
            ConsoleScript console = other.GetComponent<ConsoleScript>();
            if (Input.GetKeyDown(KeyCode.J) && console.Connect)
            {
                console.ActionConsole(gameObject);
            }
        }
    }
}
