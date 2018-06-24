using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

	private Vector3 _position;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{

	}

	private void OnTriggerEnter(Collider other)  
	{
		if (this.tag.Equals("Target"))
		{
			Debug.Log("+");

			Destroy(this.gameObject);
			Destroy(other.gameObject);


			_position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), Random.Range(12f, 20f));


			Instantiate(this.gameObject, _position, this.transform.rotation);
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
