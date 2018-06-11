using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonController : MonoBehaviour {

    public GameObject Player;
    public float DemonDistance;
    public float DemonAngel;

    private Animator _anim;
    private NavMeshAgent _NMA;
    private byte a;

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
        _NMA = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

        float f = DistanceToPlayer();

        if(f > DemonDistance)
        {
            DefaultState();
        }
        else if(f <= DemonAngel && f > 3.5f)
        {
            if(ISeeYou())
            {
                MoveToPlayer();
            }
        }
        else if(f <= 3.5f)
        {
            Attack();
        }
	}

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }

    private void DefaultState()
    {
        _anim.SetBool("Walk", false);
    }

    private bool ISeeYou()
    {
        Quaternion look = Quaternion.LookRotation(Player.transform.position - transform.position);
        float angel = Quaternion.Angle(transform.rotation, look);
        if(angel < DemonAngel)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Player.transform.position - transform.position);
            if (Physics.Raycast(ray, out hit, DemonDistance))
            {
                if(hit.transform.gameObject == Player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void MoveToPlayer()
    {
        Vector3 targer = Player.transform.position;
        _anim.SetBool("Walk", true);
        _NMA.destination = targer;
        _anim.SetBool("Attack", false);
    }

    private void Attack()
    {
        transform.LookAt(Player.transform.position);
        _anim.SetBool("Attack", true);
    }

    private void StopAttack()
    {
        _anim.SetBool("Attack", false);
    }
}
