using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReaction : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Target"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
