using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Jumper : MonoBehaviour {

    public GameObject Cam;
    public GameObject Body;
    public float ForceJump;
    public float ForceGrav;
    public float Speed;

    private Rigidbody _rigidbody;
    private Vector3 _moveVector;
    private Vector3 _standartCamPos;
    private Vector3 _camOfset;
    private Animator _anim;
    private const float k_GroundRayLength = 1f;
    private AsyncOperation _async;

 
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
        _standartCamPos = Cam.transform.position;
        _camOfset = Cam.transform.position - transform.position;
        _async = EditorSceneManager.LoadSceneAsync("RaycastAnim");
        _async.allowSceneActivation = false;
        _anim = GetComponent<Animator>();
    }

   
    private void FixedUpdate()
    {
        Cam.transform.LookAt(transform.position);
        MoveJumper();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position, Vector3.down, k_GroundRayLength, ~(1 << 8)))
            {
                Jump();
            }
        }
        else
        {
            _rigidbody.AddForce(Vector3.down * ForceGrav, ForceMode.Force);

        }
        CamMove();
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * ForceJump, ForceMode.Impulse);
    }

    private void MoveJumper()
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
                transform.position += _moveVector.normalized*Speed*Time.fixedDeltaTime;
                transform.forward = _moveVector.normalized;
            }
        }
        else
        {
            _anim.SetBool("Move", false);
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
            _async.allowSceneActivation = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("SavePoint"))
        {//sad
            ConsoleScript console = other.GetComponent<ConsoleScript>();
            if (Input.GetKeyDown(KeyCode.J) && console.Connect)
            {
                console.ActionConsole(gameObject);
            }
        }
    }
}
