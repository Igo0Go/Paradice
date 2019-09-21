using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public ScoreData score;
    public int index;

    public PauseScript pauseScript;
    public GameObject finishPanel;
    public GameObject scorePanel;
    public GameObject GoalPanel;
    public Text scoreTextFinish;
    public Text nameText;
    public Text ScoreText;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            ScoreText.text = "Счёт: " + value;
        }
    }

    public int Timer
    {
        get
        {
            return _timer;
        }
        set
        {
            _timer = value;
        }
    }

    private bool _key;
    private int _score;
    private int _timer;

    private void Finish()
    {
        _key = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        GoalPanel.SetActive(false);
        scorePanel.SetActive(false);
        finishPanel.SetActive(true);
        pauseScript.enabled = false;
        scoreTextFinish.text = (Score + Timer).ToString();

        score.scores[index].Add(new ScoreField() { name = LoadLevel.namePlayer, score = Score + Timer });
        score.Save();
    }

    void Start()
    {
        finishPanel.SetActive(false);
        scorePanel.SetActive(true);
        nameText.text = LoadLevel.namePlayer + ", " + nameText.text;
        Timer = 500;
        _key = true;
        Score = 0;
        StartCoroutine(BonusScore());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Finish"))
        {
            Finish();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator BonusScore()
    {
        while (_key && Timer > 0) 
        {
            yield return new WaitForSeconds(1);
            Timer -= 1;
        }
    }
}
