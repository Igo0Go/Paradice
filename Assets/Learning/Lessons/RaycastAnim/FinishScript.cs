using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FinishScript : MonoBehaviour
{
    public ScoreData score;
    public int index;

    public PauseScript pauseScript;

    public GameObject finishPanel;
    public GameObject console1;
    public GameObject timerText;
    public Text textTimer;
    public Text timeText;
    public Text nameText;

    private ConsoleScript _consoleScript;

    private float _time = 0;
    private bool _finish = false;
    private bool f = true;

    private void Start()
    {
        finishPanel.SetActive(false);
        timerText.SetActive(false);

        _finish = false;
        f = true;
        _time = 0;

        _consoleScript = console1.GetComponent<ConsoleScript>();
    }

    private void Update()
    {
        if (_consoleScript.active && !_finish)
        {
            _time += Time.deltaTime;
            textTimer.text = (Math.Round( _time,2).ToString());
            if (f)
            {
                timerText.SetActive(true);
                f = !f;
            }
        }
        if (!_consoleScript.active)
        {
            if (!f)
            {
                _time = 0;
                textTimer.text = (Math.Round(_time, 2).ToString()); 
                timerText.SetActive(false);
                f = !f;
            }
        }

    }

    private void Finish()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        _finish = true;
        finishPanel.SetActive(true);
        timerText.SetActive(false);
        timeText.text = _time.ToString();
        nameText.text = LoadLevel.namePlayer + ", " + nameText.text;
        pauseScript.enabled = false;

        score.scores[index].Add(new ScoreField() { name = LoadLevel.namePlayer, score = _time });
        score.Save();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Finish"))
        {
            Finish();
        }
    }
}