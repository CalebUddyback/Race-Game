using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat_Bar : MonoBehaviour
{
    public Text statText;

    private int statPoints;

    public int Points
    {
        get
        {
            return statPoints;
        }
        set
        {
            statPoints = value;
            statText.text = statPoints.ToString();
        }
    }

    public void IncrementStat(int amount)
    {
        if (Points <= 0 && amount <= 0)
            return;

        Points = Points + transform.parent.GetChild(0).GetComponent<Points_Bar>().DelegatePoint(amount);
    }

}
