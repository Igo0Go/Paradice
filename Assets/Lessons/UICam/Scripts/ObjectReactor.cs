using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectForMission
{
    RedKey,
    GreenKey,
    YellowKey
}

public class ObjectReactor : MonoBehaviour {

    public string Message;
    public ObjectForMission Key;

    private Animator _anim;

	
	void Start () {
        _anim = GetComponent<Animator>();
	}
	

	void Update () {
		
	}

    public void Action(RelictusController relictus)
    {
        relictus.GetSpecKey(Key);
        _anim.SetBool("Active", true);
        Message = string.Empty;
    }
}
