using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTech : MonoBehaviour {

    public List<Animator> RefObj;

    private Animator _anim;

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Force"))
        {
            int f = other.GetComponentInParent<FORCER>().ForceType;
            _anim.SetInteger("Active", f);
            foreach(var c in RefObj)
            {
                c.SetInteger("Active", f);
            }
        }
    }
}
