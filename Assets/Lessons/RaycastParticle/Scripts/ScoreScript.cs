using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public GameObject FinalButton;
    public Text ScoreText;
    public Text TimeBonus;

    private int _time;
    private bool _key;
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
        _time = 500;
        _key = true;
        _score = 0;
        FinalButton.SetActive(false);
        ScoreText.text = "Счёт: " + _score;
        StartCoroutine(BonusScore());
    }//asd

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Finish"))
        {
            TimeBonus.text = "Бонус за время: " + (int)_time;
            FinalButton.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private IEnumerator BonusScore()
    {
        while (_key)
        {
            yield return new WaitForSeconds(1);
            _time -= 1;
        }
    }
}
