using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public GameObject FinalButton;
    public Text ScoreText;

    private int _score;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            ScoreText.text = "Счёт: " + _score;
        }
    }
                                       
    void Start () {
        _score = 0;
        FinalButton.SetActive(false);
        ScoreText.text = "Счёт: " + _score;
	}
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Finish"))
        {
            FinalButton.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
