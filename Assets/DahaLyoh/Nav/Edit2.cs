using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Edit2 : MonoBehaviour {

	public Transform target;

	NavMeshAgent agent;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void Update ()
	{
		//agent.SetDestination(target.position);
		//agent.Stop();
	}
}
