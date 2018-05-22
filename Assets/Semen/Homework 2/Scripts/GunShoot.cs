using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class GunShoot : MonoBehaviour
{

	public GameObject Bullet;
	public float Forse;
	public GameObject Position;

	private GameObject _bullet;
	
	void Start ()
	{
		_bullet = new GameObject();
		Forse = 100;
	}
	
	
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			_bullet = Instantiate(Bullet);
			_bullet.transform.position = Position.transform.position;
			_bullet.transform.rotation = Position.transform.rotation;
			_bullet.GetComponent<Rigidbody>().AddForce(_bullet.transform.forward.normalized * Forse, ForceMode.Impulse);
		}
	}
}
