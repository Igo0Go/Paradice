using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	
	private byte _health;
	
	// Use this for initialization
	void Start ()
	{
		_health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (_health < 1)
		{
			SceneManager.LoadScene("ControlWork");
		}
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Bullet"))
		{
			_health -= 10;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10, 25, 100, 100), "Здоровье: " + _health);
	}
}
