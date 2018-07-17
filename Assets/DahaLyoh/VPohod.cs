using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPohod : MonoBehaviour
{
    public int RotateAI;
    public int SpeedAI;

    [SerializeField] private float dist;
    private CharacterController Contrl;
    public Transform TargetPos;
    private float RayL = 2;
    private float RayCenterL = 3;
    private RaycastHit hit;
    private float offsetRay = 0.7f;
    private Vector3 RunAngle;
    private float _time = 2;
    private float _time2 = 1;
    public bool Run = true;
    public bool Stay = false;
    private Vector3 TargetRayRotate;

    private RaycastHit hit10;

//hello
    // Use this for initialization
    void Start()
    {
        Contrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(TargetPos.position, transform.position);
        if (Run)
        {
            RunAngle = (TargetPos.position - transform.position).normalized;
            Vector3 RayCenter = transform.position + (transform.right * offsetRay / 10);
            Vector3 RayLeft = transform.position - (transform.right * offsetRay);
            Vector3 RayRight = transform.position + (transform.right * offsetRay);

            if (Physics.Raycast(RayCenter, transform.forward, out hit, RayCenterL))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                RunAngle += hit.normal * 5;
            }
            if (Physics.Raycast(RayLeft, transform.forward, out hit, RayL))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                RunAngle -= hit.normal * 5;
            }
            if (Physics.Raycast(RayRight, transform.forward, out hit, RayL))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                RunAngle += hit.normal * 5;
            }
            else
            {
                Debug.DrawRay(RayCenter, transform.forward * RayCenterL, Color.yellow);
                Debug.DrawRay(RayLeft, transform.forward * RayL, Color.yellow);
                Debug.DrawRay(RayRight, transform.forward * RayL, Color.yellow);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RunAngle),
                SpeedAI * Time.deltaTime);

            Contrl.Move(transform.forward * SpeedAI * Time.deltaTime);
            Contrl.Move(Vector3.down * 5f * Time.deltaTime);
            if (Physics.Raycast(RayLeft, transform.forward, out hit, RayL) &&
                Physics.Raycast(RayRight, transform.forward, out hit, RayL))
            {
                _time -= Time.deltaTime;
                if (_time < 0)
                {
                    Run = false;
                    Stay = true;
                    _time = 1f;
                }
            }
            else
            {
                _time = 2f;
            }
        }

        if (Stay)
        {
            _time -= Time.deltaTime;
            if (_time > 0)
            {
                transform.Rotate(0, 3, 0);
            }


            TargetRayRotate = (TargetPos.position - transform.position).normalized;
            Ray Search = new Ray(transform.position, TargetRayRotate);
            Debug.DrawLine(Search.origin, hit10.point, Color.green);
            if (Physics.Raycast(Search, out hit10))
            {
                Run = true;
                Stay = false;
            }
        }

        if (dist < 1f)
        {
            Run = false;
        }
        else
        {
            Run = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Target"))
        {
            Run = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Target"))
        {
            Run = true;
        }
    }
}