using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public Text TimeText;
    public GameObject ReturnMenuButton;

    private float _time;

	// Use this for initialization
	void Start () {
        _time = 0;
        TimeText.text = _time.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;
        TimeText.text = _time.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Finish"))
        {
            Cursor.lockState = CursorLockMode.None;
            Destroy(this);
        }
    }
}
