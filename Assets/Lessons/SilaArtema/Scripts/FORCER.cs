using System.Collections;
using System.Collections.Generic;
using Lessons.SilaArtema;
using UnityEngine;

public class FORCER : MonoBehaviour
{
    public float grabPower = 10.0f;
    public float throwPower = 10.0f;
    public RaycastHit hit;
    public float RayDistance = 3.0f;
    private bool Grab = false; // взять
    private bool Throw = false; // кинуть
    public Transform offset;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!hit.rigidbody)
                Physics.Raycast(transform.position, transform.forward, out hit, RayDistance, ~(1 << 9));

            if (hit.rigidbody)
            {
                Grab = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Grab)
            {
                Grab = false;
                Throw = true;
            }
        }


        if (Grab)
        {
            if (hit.rigidbody)
            {
                hit.rigidbody.velocity =
                    (offset.position - (hit.transform.position + hit.rigidbody.centerOfMass)) * grabPower;
                hit.rigidbody.GetComponent<ForceReaction>().BoostAvailable.enabled = true;
            }
        }

        if (Throw)
        {
            if (hit.rigidbody)
            {
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
}