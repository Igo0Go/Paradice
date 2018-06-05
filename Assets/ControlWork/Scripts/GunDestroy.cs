using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDestroy : MonoBehaviour
{

	private byte _health;
	void Start ()
	{
		_health = 2;
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Bullet"))
		{
			_health--;

			if (_health > 1)
			{
				Destroy(this.gameObject);
			}
		}

	}
}
