using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float Force;
	
	void Start () 
	{
		GetComponent<Rigidbody>().AddForce(transform.forward * Force, ForceMode.Impulse);
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "FallingTarget")
		{
			other.GetComponent<FallingTarget>().TakeDamage();
			Destroy(gameObject);
		}
	}
}
