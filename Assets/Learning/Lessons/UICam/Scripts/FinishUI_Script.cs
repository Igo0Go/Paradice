using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class UIRecordTable
{
    public static List<WriteTableRecord> Records { get; set; }

    public static void Create()
    {
        Records = new List<WriteTableRecord>();
    }
}
//
public class FinishUI_Script : MonoBehaviour
{
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
        if (UIRecordTable.Records == null)
        {
            UIRecordTable.Create();
        }
        UIRecordTable.Records.Add(new WriteTableRecord(LoadLevel.namePlayer, _relictusController.Energy.value));
        _relictusController.Energy.value = 100;
    }

    private void Start()
    {
        Error.SetActive(true);
        Goal.SetActive(true);
        finishPanel.SetActive(false);
        textName.text = LoadLevel.namePlayer + ", " + textName.text;
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