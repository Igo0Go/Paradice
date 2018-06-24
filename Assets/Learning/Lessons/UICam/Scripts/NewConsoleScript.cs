using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewConsoleScript : MonoBehaviour {

    public List<Animator> UsingObjects;
    public bool Active;
    public bool Connect;
    //sad
    private float _time;
    private float _connectTime;

    void Start () {
        _connectTime = 1;
        Connect = true;
	}
	
	void Update () {
        if (!Connect)
        {
            Timer();
        }
    }

    public void Use()
    {
        foreach(var c in UsingObjects)
        {
            Active = !Active;
            c.SetBool("Active", Active);
            _time = 0;
            Connect = false;
        }
    }

    private void Timer()
    {
        if (_time < _connectTime)
        {
            _time += Time.deltaTime;
        }
        else
        {
            Connect = true;
        }
    }
}
