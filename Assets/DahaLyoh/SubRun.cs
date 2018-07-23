using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubRun : MonoBehaviour
{
	private CharacterController _NMA;
	public Transform Glavtrans;

	public NavMeshPath navPath;
	
	void Start () {
		_NMA = GetComponent<CharacterController>();
		navPath = new NavMeshPath();
	}
	
	
	
	// Update is called once per frame
	//
	void Update () {
		if (Vector3.Distance(transform.position, Glavtrans.position) > 0.5f )
		{
			MoveToGlav();
			//Gizmos.DrawLine(transform.position.normalized, GetDistinationNacMech(Glavtrans.position.normalized));
		}
		else
		{
			
		}
	}

	private void MoveToGlav()
	{
		_NMA.Move(((Glavtrans.position)-GetDistinationNacMech(transform.position)).normalized*5f*Time.deltaTime);


	}

	public Vector3 GetDistinationNacMech(Vector3 request)
	{
		NavMeshHit myHitik;
		if (NavMesh.SamplePosition(request, out myHitik, 100, NavMesh.AllAreas))
		{
			navPath.ClearCorners();
			NavMesh.CalculatePath(transform.position, myHitik.position, NavMesh.AllAreas, navPath);
			if (navPath != null && navPath.corners.Length > 1)
			{
				return navPath.corners[1] - navPath.corners[0];
			}
		}

		return myHitik.position;
	}
}
