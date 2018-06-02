using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConveyorMainScript : MonoBehaviour,IMechanic{

    public ConveyorScript[] Parts;

    public float Speed;
    public GameObject Light;

    public bool Active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;
            if (value)
            {
                Activate();
            }
            else
            {
                Stop();
            }
        }
    }

    public bool _active;
    // Use this for initialization
    void Start () {
        Parts = GetComponentsInChildren<ConveyorScript>();
        Active = false;
        Light.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        
        //SetSpeedToPlate(Speed);
	}

    private void SetSpeedToPlate(float speed)
    {
        foreach(var c in Parts)
        {
            c.Speed = speed;
        }
    }

    private void Reverse()
    {
        foreach(var c in Parts)
        {
            c.ResetBox();
        }
    }

    public void Activate()
    {
        SetSpeedToPlate(5);
    }

    public void Stop()
    {
        SetSpeedToPlate(0);
    }

    public void Action()
    {
        Reverse();
    }

    public void LightActivator()
    {
        Light.SetActive(true);
    }
}
