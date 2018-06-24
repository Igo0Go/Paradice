using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTarget : MonoBehaviour
{
	private Rigidbody rg;
	
	void Start ()
	{
		rg = GetComponent<Rigidbody>();
	}
	
	void Update () {
		
	}

	public void TakeDamage()
	{
		Destroy(gameObject);
	}
}
