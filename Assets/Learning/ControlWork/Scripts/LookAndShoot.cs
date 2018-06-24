using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class LookAndShoot : MonoBehaviour
{
	public List<GameObject> Guns;
	public GameObject Bullet;
	private Vector3 _position;
	public float Force;

	private GameObject[] _bullets;
	
	void Start ()
	{
		Force = 35.0f;
		_bullets = new GameObject[Guns.Count];
	}
	
	void Update () {
		
	}


	

	private void OnTriggerStay(Collider other)
	{
		if (other.tag.Equals("Player"))
		{
			if (this.tag.Equals("Look") || this.tag.Equals("LookAndShoot"))
			{
				foreach (var _gun in Guns)
				{
					if (_gun != null)
					{
						_gun.transform.LookAt(other.transform);
					}
				}
			}

			if (this.tag.Equals("Shoot") || this.tag.Equals("LookAndShoot"))
			{
				for (int k = 0; k != Guns.Count; k++)
				{
					if ((Guns[k] != null) && (_bullets[k] == null))
					{

						_bullets[k] = Instantiate(Bullet);
						_bullets[k].transform.position = Guns[k].transform.GetChild(0).position;
						_bullets[k].transform.rotation = Guns[k].transform.GetChild(0).rotation;
						
						_bullets[k].GetComponent<Rigidbody>().AddForce(_bullets[k].transform.forward * Force, ForceMode.Impulse);
					}
				}
			}
		}
	}
}
