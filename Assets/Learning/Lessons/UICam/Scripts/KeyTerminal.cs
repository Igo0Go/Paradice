using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTerminal : MonoBehaviour {

    public List<Animator> Portals;
    public ObjectForMission Key;
    public GameObject SpecKey;
    

	// Use this for initialization
	void Start () {
        SpecKey.SetActive(false);
	}
	
	
	void Update () {
		
	}

    public void UsingKey(RelictusController relictus)
    {
        if(relictus.Keys.Contains(Key))
        {
            relictus.RemoveSpecKey(Key);
            relictus.InterfaceText.text = string.Empty;
            SpecKey.SetActive(true);
            foreach (var c in Portals)
            {
                c.SetBool("Active", true);
            }
            Destroy(gameObject);
        }
    }


}
