using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
//using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class Creator : MonoBehaviour
{

	public GameObject ObjectParent;
	public GameObject ObjectToCreate;
	public Vector3 ObjectPosition;
	public Vector3 ObjectScale;
	public bool CreateObject;

	private GameObject _object;

	void Start ()
	{
		_object = new GameObject();
		ObjectPosition = new Vector3(0, 0, 5);
		ObjectScale = new Vector3(1, 1, 1);
		CreateObject = false;
	}
	
	void Update () {
		if (CreateObject)
		{
			Create();
			CreateObject = !CreateObject;
		}
	}

	public void Create()
	{
		_object = Instantiate(ObjectToCreate, ObjectParent.transform);
		_object.transform.localPosition = ObjectPosition;
		_object.transform.localScale = ObjectScale;
	}
}

