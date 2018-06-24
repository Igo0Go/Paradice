using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsCreator : MonoBehaviour
{

	public enum Coordinate1
	{
		Global,
		Local
	}

	public Coordinate1 Coordinate;

	public GameObject Object;
	public GameObject Parent;
	public Vector3 Position;
	public bool Create;

	private Vector3 newObjectPosition;

	void Start()
	{
		Coordinate = Coordinate1.Global;
		Create = false;
	}

	void Update()
	{
		if (Create)
		{
			if (Coordinate == Coordinate1.Global) newObjectPosition = Position;
			else
			{
				if (Coordinate == Coordinate1.Local) newObjectPosition = Parent.transform.localPosition + Position;
			}

			Instantiate(Object, newObjectPosition, Quaternion.identity);
			Create = false;
		}
	}
}
