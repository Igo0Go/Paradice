using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ObjectMove : MonoBehaviour
{
	public GameObject Bullet;
	public GameObject StartBullet;
	
	private float speed;
	private Rigidbody rigidbody;
	private float horizontal;
	private float vertical;
	
	void Start ()
	{
		speed = 0.2f;
		rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
			Fire();
	}

	private void FixedUpdate()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		rigidbody.AddForce(((transform.right * horizontal) + (transform.forward * vertical)) * speed / Time.deltaTime);
	}

	void Fire()
	{
		Instantiate(Bullet, StartBullet.transform.position, StartBullet.transform.rotation);
	}
}
