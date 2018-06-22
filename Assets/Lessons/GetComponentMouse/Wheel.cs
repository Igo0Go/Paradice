using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour, IMechanic {

    public GameObject Wheele;
    public GameObject Light;
    public float Speed;
    public bool Active;

    public void Action()
    {
        Speed = -Speed;
    }

    public void Activate()
    {
        Active = true;
    }

    public void LightActivator()
    {
        Light.SetActive(true);
    }

    public void Stop()
    {
        Active = false;
    }

    // Use this for initialization
    void Start () {
        Active = false;
        Light.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if (Active)
        {
            Wheele.transform.Rotate(0, Speed, 0);
        }
	}
}
