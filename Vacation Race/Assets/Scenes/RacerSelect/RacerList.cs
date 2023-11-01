using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerList : MonoBehaviour
{
    public int maxRacers;

    public List<RacerProfile> racers = new List<RacerProfile>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
