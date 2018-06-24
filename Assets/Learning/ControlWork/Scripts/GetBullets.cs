using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBullets : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag.Equals("Pistol"))
		{
			other.GetComponent<Weapon>().NumberOfBullets = 20;
		}

		if (other.tag.Equals("Rifle"))
		{
			other.GetComponent<Weapon>().NumberOfBullets = 30;
		}
		
		//Destroy(gameObject);
	}
}
