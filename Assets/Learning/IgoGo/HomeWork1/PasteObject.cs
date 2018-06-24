using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasteObject : MonoBehaviour {

    public GameObject Obj;
    public Vector3 PastePosition;
    public Vector3 ObjectScale;
    public bool Paste;

    private bool _rotate;
    private GameObject _pasteObject;
	

	void Start () {
        _rotate = false;
        Paste = false;
        PastePosition = new Vector3(1, 1, 1);
        ObjectScale = new Vector3(1, 1, 1);
	}
	
  
	void Update () {
		if(Paste)
        {
            _pasteObject = PasteMetod();
        }
        if(_rotate)
        {
            //CameraRotate(_pasteObject);
        }
	}


    
    private GameObject PasteMetod()
    {
        var obj = Instantiate(Obj, gameObject.transform);
        obj.transform.localPosition = PastePosition;
        obj.transform.localScale = ObjectScale;
        Paste = false;
        _rotate = true;
        return obj;
    }

    private void CameraRotate(GameObject target)
    {
        Vector3 moveVector = target.transform.position - transform.position;          
        if (Vectors(transform.forward, moveVector))	
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector), 0.5f); 
        }                                                                                                   
        else
        {
            transform.forward = moveVector;
            _rotate = false;
        }
    }

    private bool Vectors(Vector3 Face, Vector3 move)  				
    {
        Vector3 FaceVector = new Vector3(Face.x, 0, Face.z);		
        Vector3 MoveVector = new Vector3(move.x, 0, move.z);		
        Debug.Log(Vector3.Angle(FaceVector, MoveVector));			
        if (Vector3.Angle(FaceVector, MoveVector) < 1)				
        {
            return false;
        }
        return true;
    }
}
