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

			if (_health == 0)
			{
				Destroy(this.gameObject);
			}
		}
	}

	/*public void Shoot(GameObject target)
	{
		GameObject bul = Instantiate(Bullet, _gun.transform.position, _gun.transform.rotation);
		bul.GetComponent<Rigidbody>().AddForce(transform.forward * Force, ForceMode.Impulse);
	}*/
}
