using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFieldScript : MonoBehaviour
{
    public Text NumberText;
    public Text NameText;
    public Text ScoreText;

    public string Number { get => NumberText.text; set => NumberText.text = value; }
    public string Name { get => NameText.text; set => NameText.text = value; }
    public string Score { get => ScoreText.text; set => ScoreText.text = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
