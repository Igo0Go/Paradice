using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
	[HideInInspector]
	public byte NumberOfBullets;
	public float Force;
	public GameObject Target;
	public GameObject Bullet;
	public GameObject Player; 
	public GameObject Position;

	private bool _shoot;
	private string _weaponType;
	
	void Start ()
	{
		switch (gameObject.tag)
		{
			case "Pistol":
				NumberOfBullets = 20;
				break;
			case "Rifle":
				NumberOfBullets = 30;
				break;
		}

		_shoot = false;
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			if (NumberOfBullets > 0 && _shoot && tag == _weaponType)
			{
				GameObject bullet = Instantiate(Bullet, Target.transform.position, Target.transform.rotation);
				bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward.normalized * Force, ForceMode.Impulse);
				NumberOfBullets--;
			}
		}
	}

	private void OnMouseDown()
	{
		transform.SetParent(Player.transform);
		transform.position = Position.transform.position;
		transform.rotation = Position.transform.rotation;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		_shoot = true;
		_weaponType = tag;
	}

	
}
