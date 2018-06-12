using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForceType
{
    impulse,
    reverse
}


public class ForceScript : MonoBehaviour {

    public ForceType Type;

	void Start () {
        Type = ForceType.impulse;
	}
}
