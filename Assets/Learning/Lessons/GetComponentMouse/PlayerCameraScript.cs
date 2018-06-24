using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour {

    public GameObject TargetPos;
    public GameObject Target;

    public float Speed;						
    public bool Move;									

    private bool _keyMove;					
    private Vector3 _targetPosition;			
    private Vector3 _moveVector;

    void Start () {
        _keyMove = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(Move)
        {
            Smooth();
            transform.LookAt(Target.transform.position);
        }
    }

    private void Smooth()
    {
        if(_keyMove)
        {
            _targetPosition = TargetPos.transform.position;
            _moveVector = _targetPosition - transform.position;
            _keyMove = false;
        }
        float x = Vector3.Distance(transform.position, _targetPosition);    
        if (Speed * Time.deltaTime < x)                                    
        {
            transform.position += _moveVector.normalized * Speed * Time.deltaTime; 
        }                                                                           
        else                                                                        
        {
            _keyMove = !_keyMove;
            Move = false;
            transform.position += _moveVector.normalized * x;                        
        }
    }
}
