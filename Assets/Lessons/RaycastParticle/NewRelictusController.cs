using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class NewRelictusController : MonoBehaviour {
    public List<ObjectForMission> Keys;
    public List<GameObject> KeysImages;
    public Animator Connect;
    public Animator Cam;
    public Text InterfaceText;
    public Text MissionText;
    public Slider Energy;
    public Slider Health;
    public float Speed;
    public float RotateSpeed;
    public float MaxVert;
    public float MinVert;
    public int EnergySpeed;

    private CharacterController _controller;
    private Animator _anim;
    private AsyncOperation _async;
    private Vector3 _moveVector;
    private float _speed;
    private float _rotationX;
    private float _rotationY;
    private Vector3 _savePosition;

    //гравитация
    private float _grav;
    private float _jumpSpeed;
    private float _vertSpeed;

    void Start()
    {
        Health.value = 100;
        _savePosition = transform.position;
        _speed = Speed;
        _anim = GetComponent<Animator>();
        _grav = -9.8f;
        _jumpSpeed = 5;
        MissionText.text = "Доберитесь до медецинского отсека";
        InterfaceText.text = string.Empty;
        Energy.value = 100;
        EnergySpeed = 1;
        _controller = GetComponent<CharacterController>();
        _rotationX = _rotationY = 0;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (var c in KeysImages)
        {
            c.SetActive(false);
        }
        _async = EditorSceneManager.LoadSceneAsync("RaycastParticle");
        _async.allowSceneActivation = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Health.value -= 30;
        }


        Energy.value += EnergySpeed * Time.deltaTime;
        
        if (Health.value > 0)
        {
            RelictusMove();
            Rotate();
            MaxSpeed();
        }
        else
        {
            FatlError();
        }
    }

    private void RelictusMove()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        _moveVector = transform.right * x + transform.forward * z;
        _anim.SetFloat("RunWalk", Mathf.Clamp(_moveVector.magnitude * _speed / 24, 0, 1));
        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vertSpeed = _jumpSpeed;
            }
        }
        _vertSpeed += _grav * Time.deltaTime;
        _moveVector = new Vector3(_moveVector.x * _speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime, _moveVector.z * _speed * Time.fixedDeltaTime);
        if (_moveVector != Vector3.zero)
        {
            _controller.Move(_moveVector);
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
        }
    }
    //sad
    private void MaxSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = Speed * 2;
            EnergySpeed = -3;
        }
        else
        {
            _speed = Speed;
            EnergySpeed = 1;
        }
    }

    private void FatlError()
    {
        _anim.SetTrigger("Fatal");
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        _async.allowSceneActivation = true;
    }

    public void GetSpecKey(ObjectForMission key)
    {
        InterfaceText.text = string.Empty;
        switch (key)
        {
            case ObjectForMission.GreenKey:
                {
                    KeysImages[0].SetActive(true);
                    Keys.Add(key);
                    break;
                }
            case ObjectForMission.YellowKey:
                {
                    KeysImages[1].SetActive(true);
                    Keys.Add(key);
                    break;
                }
            case ObjectForMission.RedKey:
                {
                    KeysImages[2].SetActive(true);
                    Keys.Add(key);
                    break;
                }
        }
    }

    public void RemoveSpecKey(ObjectForMission key)
    {
        switch (key)
        {
            case ObjectForMission.GreenKey:
                {
                    KeysImages[0].SetActive(false);
                    Keys.Remove(key);
                    break;
                }
            case ObjectForMission.YellowKey:
                {
                    KeysImages[1].SetActive(false);
                    Keys.Remove(key);
                    break;
                }
            case ObjectForMission.RedKey:
                {
                    KeysImages[2].SetActive(false);
                    Keys.Remove(key);
                    break;
                }
        }
    }

  
}
