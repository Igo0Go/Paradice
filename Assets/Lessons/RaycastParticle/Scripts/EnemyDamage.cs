using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {

    public float Health;
    public GameObject MainObj;
    public float DeadTime;

	
    public void Func()
    {
        GetComponent<Animator>().SetTrigger("TheEnd");
        Invoke("Death", DeadTime);
    }


    private void Death()
    {
        Destroy(MainObj);
    }

}
