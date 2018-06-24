using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour {

	
	
	void Start ()
	{
		StartCoroutine(DestroyMe());
	}
	
	void Update (){
		
	}
	
	
	IEnumerator DestroyMe() {
		yield return new WaitForSeconds(3f);
		Destroy(this.gameObject);
	}


}
