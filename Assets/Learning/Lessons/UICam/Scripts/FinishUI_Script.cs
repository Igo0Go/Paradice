using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class FinishUI_Script : MonoBehaviour
{
    public ScoreData score;
    public int index;

    public PauseScript pauseScript;
    public GameObject finishPanel;
    public GameObject Error;
    public GameObject Goal;
    public Text textName;
    public Text textScore;

    private RelictusController _relictusController;

    private void Finish()
    {
        Time.timeScale = 0;
        textScore.text = _relictusController.Energy.value.ToString();
        finishPanel.SetActive(true);
        Error.SetActive(false);
        Goal.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseScript.enabled = false;

        score.scores[index].Add(new ScoreField() { name = LoadLevel.namePlayer, score = _relictusController.Energy.value });
        score.Save();
        _relictusController.Energy.value = 100;

    }

    private void Start()
    {
        Error.SetActive(true);
        Goal.SetActive(true);
        finishPanel.SetActive(false);
        if (textName != null)
            textName.text = LoadLevel.namePlayer + ", " + textName?.text;
        _relictusController = gameObject.GetComponent<RelictusController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Finish"))
        {
            Finish();
        }
    }
}