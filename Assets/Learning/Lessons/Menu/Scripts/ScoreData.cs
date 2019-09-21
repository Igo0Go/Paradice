using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ScoreData", menuName = "Score Data")]
public class ScoreData : ScriptableObject
{
    public List<ScoreField>[] scores = new List<ScoreField>[LoadLevel.countChallenge];

    public void Save()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
    }

    public ScoreData()
    {
        for (int i = 0; i < LoadLevel.countChallenge; i++)
        {
            scores[i] = new List<ScoreField>();
        }
    }
}

[Serializable]
public class ScoreField : IComparable<ScoreField>, IComparer<ScoreField>
{
    public string name;
    public float score;

    public int Compare(ScoreField x, ScoreField y)
    {
        return x.CompareTo(y);
    }

    public int CompareTo(ScoreField other)
    {
        return score.CompareTo(other.score);
    }
}