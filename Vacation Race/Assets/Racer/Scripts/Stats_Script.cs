using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats_Script : MonoBehaviour
{
    public bool randomStats = false;

   // public int start_speed = 5;
    public int start_reaction = 5;
    public float acceleration = 1;
    public int top_speed = 8;
    public int stamina = 5;
 

    private void Awake()
    {
        if (randomStats)
        {
            start_reaction = Random.Range(0, 6);
            acceleration = Random.Range(0, 6);
            top_speed = Random.Range(0, 6);
            stamina = Random.Range(0, 6);
        } 
    }
}
