using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMainScript : MonoBehaviour, IMechanic {

    public List<GameObject> Parts;
    public 

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update () {
		
	}

    void IMechanic.Stop(bool value)
    {
        
    }

    void IMechanic.Action()
    {
        throw new System.NotImplementedException();
    }

    void IMechanic.Activate()
    {
        throw new System.NotImplementedException();
    }
}
