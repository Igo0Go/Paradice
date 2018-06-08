using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    public PortalScript Ref;
    public Transform pos;
    public GameObject Mirror;
    public bool Active;
    public float PortalTime;

	// Use this for initialization
	void Start () {
        Active = true;
        GetComponent<Animator>().SetBool("Active", false);
	}
	
	// Update is called once per frame
	void Update () {
		if(!Active)
        {
            Timer();
        }
	}

    private void Timer()
    {
        if(PortalTime < 2)
        {
            PortalTime += Time.deltaTime;
        }
        else
        {
            Mirror.SetActive(true);
            Active = true;
            Ref.Mirror.SetActive(true);
            Ref.Active = true;
        }
    }
}
