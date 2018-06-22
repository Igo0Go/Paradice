using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WriteTableRecord
{
    [SerializeField]
    public string Name { get; private set; }
    [SerializeField]
    public float Result { get; private set; }

    public WriteTableRecord(string name, float result)
    {
        Name = name;
        Result = result;
    }
}

public static class JumperRecordTable
{
    public static List<WriteTableRecord> Records { get; set; }

    public static void Create()
    {
        Records = new List<WriteTableRecord>();
    }

}

public class FinishScript : MonoBehaviour
{
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
            textTimer.text = ((int)_time).ToString();
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
                textTimer.text = ((int)_time).ToString();
                timerText.SetActive(false);
                f = !f;
            }
        }

    }

    private void Finish()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _finish = true;
        finishPanel.SetActive(true);
        timerText.SetActive(false);
        timeText.text = _time.ToString();
        nameText.text = LoadLevel.namePlayer + ", " + nameText.text;
        GameObject.Find("GameMenuPanel").GetComponent<PauseScript>().enabled = false;
        if (JumperRecordTable.Records == null)
        {
            JumperRecordTable.Create();
        }
        JumperRecordTable.Records.Add(new WriteTableRecord(LoadLevel.namePlayer, _time));

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Finish"))
        {
            Finish();
        }
    }
}