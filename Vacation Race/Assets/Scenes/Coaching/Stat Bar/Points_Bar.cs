using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points_Bar : MonoBehaviour
{
    public Text pointsText;

    private int points;
    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
            pointsText.text = points.ToString();
        }
    }

    public int DelegatePoint(int amount)
    {
        if (amount > 0)
        {
            if (Points > 0)
            {
                Points--;
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            Points++;
            return -1;
        }
    }
}
