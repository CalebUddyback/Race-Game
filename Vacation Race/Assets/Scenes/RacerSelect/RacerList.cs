using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerList : MonoBehaviour
{
    public int maxRacers;

    public List<string> racers = new List<string>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
