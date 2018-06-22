using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuperCube : MonoBehaviour {

    public bool Sticky;
    public GameObject Light;
    public PlayerCameraScript Cam;

    private Rigidbody _rb;
    private AsyncOperation _async;
    
    void Start () {
        _rb = GetComponent<Rigidbody>();
        Sticky = false;
        Light.SetActive(false);
        _async = SceneManager.LoadSceneAsync("InputLesson");
        _async.allowSceneActivation = false;
    }
	
	// Update is called once per frame
	void Update () {
       
        GetSticky();
	}

    private void GetSticky()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Light.SetActive(!Light.activeSelf);
            Sticky = !Sticky;
            if (Sticky == false)
            {
                transform.parent = null;
                _rb.isKinematic = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Sticky)
        {
            transform.parent = collision.transform;
            _rb.isKinematic = true;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Sticky)
        {
            transform.parent = collision.transform;
            _rb.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("CameraChenger"))
        {
            Debug.Log(other.gameObject.GetComponent<CameraChenged>().Target);
            Cam.Target = other.gameObject.GetComponent<CameraChenged>().Target;
            Cam.TargetPos = other.gameObject.GetComponent<CameraChenged>().CamPos;
            Cam.Move = true;
        }
        if (other.tag.Equals("Durk"))
        {
            _async.allowSceneActivation = true;
        }
    }
}
