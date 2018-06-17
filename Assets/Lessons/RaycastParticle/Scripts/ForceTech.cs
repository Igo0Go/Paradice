using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTech : MonoBehaviour {

    public bool ForceObject;
    public List<Animator> RefObj;

    private Animator _anim;

	void Start () {
        _anim = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Force") && ForceObject)
        {
            int f = other.GetComponentInParent<FORCER>().ForceType;
            Action(f);
        }
    }

    public void Action(int f)
    {
        _anim.SetInteger("Active", f);
        foreach (var c in RefObj)
        {
            c.SetInteger("Active", f);
        }
    }
}
