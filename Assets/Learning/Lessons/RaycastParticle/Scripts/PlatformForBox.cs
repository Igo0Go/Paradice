using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformForBox : MonoBehaviour {

    public int Action;
    public float Weight;
    public List<Animator> RefObj;

    private Animator _anim;
    private float _weight;

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_weight < Weight)
        {
            if(_weight + Time.deltaTime < Weight)
            {
                _weight += Time.deltaTime;
            }
            else
            {
                _weight = Weight;
            }
            
        }
        else if (_weight > Weight)
        {
            if (_weight - Time.deltaTime > Weight)
            {
                _weight -= Time.deltaTime;
            }
            else
            {
                _weight = Weight;
            }
        }
        _anim.SetFloat("Weight", _weight);
        if (_weight >= 1)
        {
            foreach(var c in RefObj)
            {
                c.SetInteger("Active", Action);
            }
        }
        if (_weight <= 0)
        {
            foreach (var c in RefObj)
            {
                c.SetInteger("Active", Action*(-1));
            }
        }
    }
}
