using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour {

    public GameObject Force;
    public GameObject Box;
    public GameObject Cam;
    public GameObject Weapon;
    public GameObject Ammo;
    public float MoveSpeed;
    public float RotateSpeed;
    public float WeaponForce;
    public float MaxVert;
    public float MinVert;
    private float _time;
    private bool _forceKey1;
    private bool _forceKey2;
    public int AmmoCount;

    private ForceScript _forceType;
    private Vector3 _ammoPosition;
    private Vector3 _moveVector;
    private CharacterController _character;
    private float _rotationX;
    private float _rotationY;
    private bool _forceActive;

	void Start () {
        _forceType = Force.GetComponent<ForceScript>();
        Force.SetActive(false);
        _character = GetComponent<CharacterController>();
        AmmoCount = 7;
        _ammoPosition = Weapon.transform.position + Weapon.transform.forward.normalized*0.5f;
        _rotationX = _rotationY = 0;
        Cursor.lockState = CursorLockMode.Locked;
        _forceKey1 = true;
        _forceKey2 = true;
    }

    private void LateUpdate()
    {
        Rotate();//sad
        Move();
    }


    void Update () {
        if(Input.GetKeyDown(KeyCode.E) && _forceKey1)
        {
            _forceType.Type = ForceType.impulse;
            Force.SetActive(true);
            _time = 0;
            _forceKey1 = false;
        }
        if (Input.GetKeyDown(KeyCode.Q) && _forceKey1)
        {
            _forceType.Type = ForceType.reverse;
            Force.SetActive(true);
            _time = 0;
            _forceKey1 = false;
        }
        if (!_forceKey1)
        {
            Timer();
        }
        if(Input.GetMouseButtonDown(0) && AmmoCount != 0)
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            MoveSpeed *= 2;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            MoveSpeed /= 2;
        }
	}

    private void Shoot()
    {
        _ammoPosition = Weapon.transform.position + Weapon.transform.up.normalized * 0.5f;
        var obj = Instantiate(Ammo, _ammoPosition, Quaternion.identity);
        var rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(Weapon.transform.up.normalized * WeaponForce, ForceMode.Impulse);
        AmmoCount -= 1;
        if(AmmoCount == 0)
        {
            Box.SetActive(false);
        }
    }

    private void Move()
    {//sad
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            _moveVector = Cam.transform.right * x + Cam.transform.forward * z;
            _moveVector = new Vector3(_moveVector.x, 0, _moveVector.z);
            if (_moveVector != Vector3.zero)
            {
                _character.Move(_moveVector.normalized * MoveSpeed * Time.deltaTime);
            }
        }
    }

    private void Rotate()
    {
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            var h = Input.GetAxis("Mouse X");
            var v = Input.GetAxis("Mouse Y");
            _rotationX = transform.localEulerAngles.y + h*RotateSpeed;
            _rotationY += v*RotateSpeed;
            _rotationY = Mathf.Clamp(_rotationY, MinVert, MaxVert);
            transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        }
    }

    private void Timer()
    {
        if (_time < 0.2f)
        {
            _time += Time.deltaTime;
            Debug.Log(_time);
        }
        else if(_time < 2)
        {
            if(_forceKey2)
            {
                Force.SetActive(false);
                _forceKey2 = false;
            }
            _time += Time.deltaTime;
            Debug.Log(_time);
        }
        else
        {
            _forceKey1 = true;
            _forceKey2 = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Target"))
        {
            Box.SetActive(true);
            AmmoCount += 7;
            Destroy(other.gameObject);
        }
    }
}
