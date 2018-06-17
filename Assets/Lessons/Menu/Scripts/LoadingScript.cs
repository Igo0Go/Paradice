using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour {

    public Slider slider;

    private AsyncOperation _asyncOperation;

    // Use this for initialization
    void Start () {
        _asyncOperation = SceneManager.LoadSceneAsync(LoadLevel.name);
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = _asyncOperation.progress;
	}
}
