using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReactor : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Shoot"))
        {
            Destroy(gameObject);
        }
    }
}
