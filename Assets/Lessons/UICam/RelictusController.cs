using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelictusController : MonoBehaviour {

    public List<ObjectForMission> Keys;
    public List<GameObject> KeysImages;
    public Animator Connect;
    public Text InterfaceText;
    public Text MissionText;
    public Slider Energy;
    public float Speed;
    public float RotateSpeed;
    public float MaxVert;
    public float MinVert;
    public int EnergySpeed;

    private CharacterController _controller;//sad
    private Vector3 _moveVector;
    private float _rotationX;
    private float _rotationY;

    //гравитация
    private float _grav;
    private float _jumpSpeed;
    private float _vertSpeed;

    void Start () {
        _grav = -9.8f;
        _jumpSpeed = 5;
        MissionText.text = "Доберитесь до медецинского отсека";
        InterfaceText.text = string.Empty;
        Energy.value = 100;
        EnergySpeed = 1;
        _controller = GetComponent<CharacterController>();
        _rotationX = _rotationY = 0;
        Cursor.lockState = CursorLockMode.Locked;
        foreach(var c in KeysImages)
        {
            c.SetActive(false);
        }
    }
	
	void Update () {
        Energy.value -= EnergySpeed*Time.deltaTime;
        RelictusMove();
        Rotate();
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
                Energy.value -= 1;//sad
            }
        }
        _vertSpeed += _grav * Time.deltaTime;
        _moveVector = new Vector3(_moveVector.x * Speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime, _moveVector.z * Speed * Time.fixedDeltaTime);
        if (_moveVector != Vector3.zero)
        {
            EnergySpeed = 3;
            _controller.Move(_moveVector);
        }
        else
        {
            EnergySpeed = 0;
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
            EnergySpeed = 1;
        }
        else
        {
            EnergySpeed = 0;
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
        if (other.tag.Equals("Target"))
        {
            Connect.SetBool("Active", true);
            InterfaceText.text = other.GetComponent<ObjectReactor>().Message;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("JumpPoint"))
        {
            Connect.SetBool("Active", false);
            other.GetComponent<Animator>().SetBool("Active", false);
            EnergySpeed = 0;
        }
        if (other.tag.Equals("Target"))
        {
            Connect.SetBool("Active", false);
            InterfaceText.text = string.Empty;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Target"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                other.GetComponent<ObjectReactor>().Action(this);
                Energy.value -= 2;
            }
        }
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

    private void MaxSpeed()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 8;
            EnergySpeed = 5;
        }
        else
        {
            Speed = 4;
            EnergySpeed = 0;
        }
    }
}
