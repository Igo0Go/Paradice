﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class LookAndShoot : MonoBehaviour
{

	public List<GameObject> Guns;
	public GameObject Bullet;
	public GameObject Position;
	public float Force;
	
	
	void Start () {
		
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
					_gun.transform.LookAt(other.transform);
				}
			}

			
			if (this.tag.Equals("Shoot") || this.tag.Equals("LookAndShoot"))
			{
				
				foreach (var _gun in Guns)
				{
					
				}
			}
		}
	}
}
