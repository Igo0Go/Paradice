using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RUN : MonoBehaviour {

    public GameObject TargetPoint;
    public GameObject AttackSphere;
    public GameObject Cam;
    public LayerMask Obstacles;
    public LayerMask NoPlayer;
    public float Speed;
    public float CamSpeed;
    public float sdt;//asd
    public float JumpForce;
    public float Grav;
    public float CamDistanceHorizontal
    {
        get {return _camDistanceHorizontal; }
        set
        {
            _camDistanceHorizontal = value;
            GetDistance();
        }
    }

    private float _camDistanceHorizontal;
    
    private CharacterController _controller;
    private Vector3 _moveVector;
    private Vector3 _camTarget;
    private Vector3 _camVector;
    public float _camDistance;

    private float _vertSpeed;
   
    void Start () {
        AttackSphere.SetActive(false);
        _camTarget = transform.position;
        _controller = GetComponent<CharacterController>();
        GetDistance();
        CamTarget();
	}

    private void LateUpdate()
    {
        CamMoveToTarget();
        CamObstaclesReactions();
        Cam.transform.LookAt(TargetPoint.transform.position);
    }

    void Update ()
    {
        CamDistanceHorizontal = sdt;
        Move();

        if(Input.GetKeyDown(KeyCode.K))
        {
            AttackSphere.SetActive(true);
            Invoke("FinalAttack", 0.5f);
        }
	}

    private void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
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
        _moveVector = new Vector3(_moveVector.x * Speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime, _moveVector.z * Speed * Time.fixedDeltaTime);
        if (_moveVector != Vector3.zero)
        {
            CamTarget();
            _controller.Move(_moveVector);
        }
    }

    private void CamTarget()
    {
        _camTarget = TargetPoint.transform.position;
        _camTarget = new Vector3(_camTarget.x, transform.position.y + 3, _camTarget.z);
        _camVector = _camTarget - Cam.transform.position;
        //_camVector = new Vector3(_camVector.x, 0, _camVector.z);
        _camTarget += _camVector.normalized * -CamDistanceHorizontal;
    }

    private void CamMoveToTarget()
    {
        if(Cam.transform.position != _camTarget)
        {
            _camVector = _camTarget - Cam.transform.position;
            float step = Speed * Time.fixedDeltaTime;
            float distance = Vector3.Distance(_camTarget, Cam.transform.position);
            if(step < distance)
            {
                Cam.transform.position += _camVector.normalized * step;
            }
            else
            {
                Cam.transform.position = _camTarget;
            }
        }
    }

    private void CamObstaclesReactions()
    {
        float distance = Vector3.Distance(Cam.transform.position, TargetPoint.transform.position);
        RaycastHit hit;
        if(Physics.Raycast(TargetPoint.transform.position, Cam.transform.position - TargetPoint.transform.position, out hit, distance ,Obstacles))
        {
            Cam.transform.position = hit.point;
            Cam.transform.position = new Vector3(Cam.transform.position.x, TargetPoint.transform.position.y, Cam.transform.position.z);
        }
        else if(distance < _camDistance && !Physics.Raycast(Cam.transform.position, -Cam.transform.forward, 0.1f, Obstacles))
        {
            
            CamMoveToTarget();
        }
    }

    private void GetDistance()
    {
        _camDistance = Mathf.Sqrt(Mathf.Pow(CamDistanceHorizontal, 2) + Mathf.Pow(Cam.transform.position.y - TargetPoint.transform.position.y, 2));
    }

    private void FinalAttack()
    {
        AttackSphere.SetActive(false);
    }
}
