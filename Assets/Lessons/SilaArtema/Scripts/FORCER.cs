using System.Collections;
using System.Collections.Generic;
using Lessons.SilaArtema;
using UnityEngine;
using UnityEngine.UI;

public class FORCER : MonoBehaviour
{
    public GameObject ForceZone;
    public RaycastHit hit;
    public Transform offset;
    public float grabPower = 10.0f;
    public float throwPower = 10.0f;
    public float RayDistance = 3.0f;
    public int ForceType;

    private Animator _anim;
    private NewRelictusController NRC;
    private float _forceTime;
    private bool Grab = false; // взять
    private bool Throw = false; // кинуть
    

    private void Start()
    {
        _anim = GetComponent<Animator>();
        ForceZone.SetActive(false);
        NRC = GetComponent<NewRelictusController>();
        _forceTime = 1;
    }
  
    void Update()
    {

        Timer();
        if(NRC.Energy.value > 15)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!hit.rigidbody)
                    Physics.Raycast(transform.position, transform.forward, out hit, RayDistance, ~(1 << 9));

                if (hit.rigidbody)
                {
                    Grab = true;
                    _anim.SetBool("UseForce", true);
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Grab = false;
                Throw = true;
                _anim.SetBool("UseForce", false);
                _anim.SetInteger("Force", 1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ForceType = 1;
                ForceZone.SetActive(true);
                _forceTime = 0;
                _anim.SetInteger("Force", 1);
                _anim.SetTrigger("ForceStart");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ForceType = -1;
                ForceZone.SetActive(true);
                _forceTime = 0;
                _anim.SetInteger("Force", -1);
                _anim.SetTrigger("ForceStart");
            }
        }
        

        if (Grab)
        {
            if (hit.rigidbody)
            {
                NRC.Energy.value -= 0.1f;
                NRC.EnergyTime = 0;
                hit.rigidbody.velocity =
                    (offset.position - (hit.transform.position + hit.rigidbody.centerOfMass)) * grabPower;
                hit.rigidbody.GetComponent<ForceReaction>().BoostAvailable.enabled = true;
            }
        }

        if(NRC.Energy.value == 0)
        {
            Grab = false;
            Throw = false;
            Invoke("Poff", 0.5f);
            _anim.SetBool("UseForce", false);
            _anim.SetInteger("Force", 0);
        }

        if (Throw)
        {
            if (hit.rigidbody)
            {
                NRC.Energy.value -= 3;
                hit.rigidbody.velocity = transform.forward * throwPower;
                Throw = false;
                Invoke("Poff", 0.5f);
            }
        }
    }

    void Poff()
    {
        hit.rigidbody.GetComponent<ForceReaction>().BoostAvailable.enabled = false;
    }

    private void Timer()
    {
        if(_forceTime < 0.5)
        {
            NRC.Energy.value -= 0.5f;
            NRC.EnergyTime = 0;
            _forceTime += Time.deltaTime;
        }
        else
        {
            ForceZone.SetActive(false);
        }
    }
}