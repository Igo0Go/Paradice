using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class ScoreViewScript : MonoBehaviour
{
    public Button[] buttons = new Button[LoadLevel.countChallenge];
    public ScoreData score;

    public GameObject contentListView;
    public GameObject scoreField;

    public int currtentScore = -1;
    public List<GameObject> gameObjects = new List<GameObject>();

    public RectTransform rectTransform;

    float x;
    float y;
    float w;
    float h;

    private void Start()
    {
        rectTransform = contentListView.GetComponent<RectTransform>();
        x = rectTransform.rect.x;
        y = rectTransform.rect.y;
        w = rectTransform.rect.width;
        h = rectTransform.rect.height;
        buttons[0].onClick.AddListener(Button1_Click);
        buttons[1].onClick.AddListener(Button2_Click);
        buttons[2].onClick.AddListener(Button3_Click);
    }

    private void OnEnable()
    {
        Button_Click(0);
    }

    public void Button1_Click() => Button_Click(0);
    public void Button2_Click() => Button_Click(1);
    public void Button3_Click() => Button_Click(2);

    public void Button_Click(int index)
    {
        if (currtentScore != -1)
        {
            foreach (var g in gameObjects)
            {
                Destroy(g);
            }

            gameObjects.Clear();
        }
        score.scores[index].Sort();
        for (int i = 0; i < score.scores[index].Count; i++)
        {
            ScoreField f = score.scores[index][i];
            var go = Instantiate(scoreField, contentListView.transform);
            gameObjects.Add(go);
            var sfs = go.GetComponent<ScoreFieldScript>();
            sfs.Name = f.name;
            sfs.Score = f.score.ToString();
            sfs.Number = (i + 1).ToString();
        }

        //rectTransform.rect.Set(x, y, w, h);

        currtentScore = index;
    }
}
