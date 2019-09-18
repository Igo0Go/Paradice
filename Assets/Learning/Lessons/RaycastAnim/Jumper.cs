using System.Collections;
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
    public bool onGround = false;
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

    private void Update()
    {
        Cam.transform.LookAt(transform.position);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        j = Input.GetKeyDown(KeyCode.Space);
    }
    private void FixedUpdate()
    {
        Move();
        CamMove();
    }
    float h, v;
    bool j;
    private void Move()
    {
        if (h != 0 || v != 0)
        {
            _moveVector = Cam.transform.right * h + Cam.transform.forward * v;
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
        onGround = Physics.Linecast(transform.position, transform.position + Physics.gravity.normalized * 1.5f, out var hit);
        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (j)
            {
                _vertSpeed = _jumpSpeed;
            }
        }
        _vertSpeed += _grav * Time.fixedDeltaTime;
        _moveVector = new Vector3(_moveVector.x * _speed * Time.fixedDeltaTime, _vertSpeed * Time.fixedDeltaTime,
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
