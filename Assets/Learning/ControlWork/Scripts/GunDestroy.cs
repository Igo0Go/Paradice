using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDestroy : MonoBehaviour
{
	private byte _health;
	
	void Start ()
	{
		_health = 3;
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Bullet"))
		{
			_health--;
			Destroy(other.gameObject);
			if (_health == 0)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
