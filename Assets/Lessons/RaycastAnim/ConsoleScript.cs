﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleScript : MonoBehaviour {

    public List<GameObject> Objects;
    public GameObject Pos;
    public GameObject ConsoleLight;
    public bool Connect;

    private Vector3 _position;
    private Animator _consoleAnim;
    private bool _active;
    private float _time;
    private float _connectTime;

	// Use this for initialization
	void Start () {
        _connectTime = 1;
        _position = Pos.transform.position;
        _active = false;
        ConsoleLight.SetActive(true);
        Connect = true;
        _consoleAnim = GetComponent<Animator>();

    }

    private void Update()
    {
        if(!Connect)
        {
            Timer();
        }
    }


    public void ActionConsole(GameObject actor)
    {
        _active = !_active;
        foreach(var c in Objects)
        {
            c.GetComponent<Animator>().SetBool("Active", _active);
        }
        _consoleAnim.SetBool("Active", _active);
        actor.transform.position = new Vector3(_position.x, actor.transform.position.y, _position.z);
        Vector3 moveVector = transform.position - actor.transform.position;
        moveVector = new Vector3(moveVector.x, 0, moveVector.z);
        actor.transform.forward = moveVector;
        actor.GetComponent<Animator>().SetTrigger("Action");
        _time = 0;
        ConsoleLight.SetActive(false);
        Connect = false;
    }

    private void Timer()
    {
        if(_time < _connectTime)
        {
            _time += Time.deltaTime;
        }
        else
        {
            Connect = true;
            ConsoleLight.SetActive(true);
        }
    }
}