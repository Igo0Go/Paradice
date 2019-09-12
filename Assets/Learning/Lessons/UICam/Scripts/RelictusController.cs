using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelictusController : MonoBehaviour
{

    public List<ObjectForMission> Keys;
    public List<GameObject> KeysImages;
    public PauseScript Pause;
    public AudioSource Music;
    public Animator Connect;
    public Animator Cam;
    public Text InterfaceText;
    public Text MissionText;
    public Slider Energy;
    public float Speed;
    public float RotateSpeed;
    public float MaxVert;
    public float MinVert;
    public int EnergySpeed = 1;
    public CamScript camScript;

    private CharacterController _controller;
    private Animator _anim;
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
        _savePosition = transform.position;
        _speed = Speed;
        _anim = GetComponent<Animator>();
        _grav = -9.8f;
        _jumpSpeed = 5;
        MissionText.text = "Доберитесь до медецинского отсека";
        InterfaceText.text = string.Empty;
        Energy.value = 100;
        _controller = GetComponent<CharacterController>();
        _rotationX = _rotationY = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (var c in KeysImages)
            c.SetActive(false);
    }

    float x, z;
    bool y, lS;
    private void RelictusSet()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        y = Input.GetKeyDown(KeyCode.Space);
        lS = Input.GetKey(KeyCode.LeftShift);
        //v = Input.GetKeyDown(KeyCode.V);
        //shoot = Input.GetMouseButtonDown(0);
    }
    private void Update()
    {
        RelictusSet();
    }
    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            if (Energy.value > 0)
            {
                Energy.value -= EnergySpeed * Time.deltaTime;
                RelictusMove();
                MaxSpeed();
            }
            else
            {
                FatlError();
            }
        }
    }
    private void LateUpdate()
    {
        if (Time.timeScale > 0)
            if (Energy.value > 0)
                Rotate();
    }
    private void RelictusMove()
    {
        _moveVector = transform.right * x + transform.forward * z;

        if (x != 0 || z != 0)
            _anim.SetFloat("RunWalk", Mathf.Clamp(_moveVector.magnitude * _speed / 24, 0, 1));
        else
            _anim.SetFloat("RunWalk", 0);

        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (y)
            {
                _vertSpeed = _jumpSpeed;
                Energy.value -= 1;
            }
        }
        _vertSpeed += _grav * Time.deltaTime;
        _moveVector = new Vector3(_moveVector.x * _speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime, _moveVector.z * _speed * Time.fixedDeltaTime);
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
        if (other.tag.Equals("JumpPoint"))
        {
            Connect.SetBool("Active", true);
            other.GetComponent<Animator>().SetBool("Active", true);
            EnergySpeed = 7;
        }
        if (other.tag.Equals("SavePoint"))
        {
            Connect.SetBool("Active", true);
            other.GetComponent<Animator>().SetBool("Active", true);
            _savePosition = other.transform.position;
            EnergySpeed = 2;
        }
        if (other.tag.Equals("Durk"))
        {
            _vertSpeed = 0;
            _speed = 0;
            transform.position = _savePosition;
            camScript.ChangeAnimFieldOfView(162);
            //Cam.SetTrigger("Portal");
            Energy.value -= 10;
        }
        if (other.tag.Equals("Gun"))
        {
            Energy.value += 5;
            Destroy(other.gameObject);
        }
        if (other.tag.Equals("Target"))
        {
            Connect.SetBool("Active", true);
            InterfaceText.text = other.GetComponent<ObjectReactor>().Message;
            _anim.SetTrigger("Connect");
        }
        if (other.tag.Equals("CameraChenger"))
        {
            MissionPoint MP = other.GetComponent<MissionPoint>();
            MissionText.text = MP.Message;
            if (MP.Clip != null)
            {
                Music.clip = MP.Clip;
            }

            Destroy(other.gameObject);
        }
        if (other.tag.Equals("FallingTarget"))
        {
            PortalScript ps = other.GetComponent<PortalScript>();
            if (ps.Active)
            {
                //Cam.SetTrigger("Portal");
                camScript.ChangeAnimFieldOfView(162);
                transform.position = other.GetComponent<PortalScript>().Ref.pos.position;
                transform.forward = other.transform.forward;
                ps.Ref.Active = false;
                ps.Ref.Mirror.SetActive(false);
                ps.Mirror.SetActive(false);
                ps.Active = false;
                Energy.value -= 5;
                ps.PortalTime = 0;
            }

        }
        if (other.tag.Equals("Look"))
        {
            if (other.GetComponent<NewConsoleScript>().Connect)
            {
                InterfaceText.text = "Чтобы использовать консоль, нажмите E";
                Connect.SetBool("Active", true);
                _anim.SetTrigger("Connect");
            }
        }
        if (other.tag.Equals("Shoot"))
        {
            Connect.SetBool("Active", true);
            if (Keys.Contains(other.GetComponent<KeyTerminal>().Key))
            {
                InterfaceText.text = "Чтобы использовать ключ, нажмите E";
            }
            else
            {
                InterfaceText.text = "Требуется ключ";
            }
            _anim.SetTrigger("Connect");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("JumpPoint") || other.tag.Equals("SavePoint"))
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
        if (other.tag.Equals("Look"))
        {
            InterfaceText.text = string.Empty;
        }
        if (other.tag.Equals("Shoot"))
        {
            Connect.SetBool("Active", false);
            InterfaceText.text = string.Empty;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Target"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.GetComponent<ObjectReactor>().Action(this);
                Energy.value -= 2;
            }
        }
        if (other.tag.Equals("Look"))
        {
            if (Input.GetKeyDown(KeyCode.E) && other.GetComponent<NewConsoleScript>().Connect)
            {
                other.GetComponent<NewConsoleScript>().Use();
                Energy.value -= 3;
            }
        }
        if (other.tag.Equals("Shoot"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.GetComponent<KeyTerminal>().UsingKey(this);
                Energy.value -= 1;
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

    private void MaxSpeed()
    {
        if (lS && _anim.GetFloat("RunWalk") > 0)//sad
        {
            _speed = Speed * 2;
            EnergySpeed = 5;
        }
        else
        {
            _speed = Speed;
            EnergySpeed = 0;
        }
    }

    private void FatlError()
    {
        _anim.SetTrigger("Fatal");
        Cursor.lockState = CursorLockMode.None;
        if (Input.anyKeyDown)
        {
            Pause.RetryButtonClick();
        }
    }
}
