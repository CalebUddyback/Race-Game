using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer_Script : MonoBehaviour
{
    public float current_speed;
    public int current_stamina;

    private float stepPower;
    private float dudDecay = 0.2f;

    public GameObject ghost;

    public GameObject sweat;

    public GameObject crown;

    public event System.Action Event_Idle;
    public event System.Action Event_Walk;
    public event System.Action Event_Run;
    public event System.Action Event_HandKnees;

    public int stepsTaken = 0;
    public int powerSteps = 0;
    public int dudSteps = 0;
    public float top_speed = 0;

    public bool finished = false;


    public void Idle() => Event_Idle?.Invoke();

    public void Walk() => Event_Walk?.Invoke();

    public void Run() => Event_Run?.Invoke();

    public void HandsKnees() => Event_HandKnees?.Invoke();



    public void GetSet() => Event_Run?.Invoke();

    public void GO() => StartCoroutine(StartPhase());

    private Dictionary<RacerProfile.Stat, float> adjustedStats;


    public void AdjustStats(Dictionary<RacerProfile.Stat, float> baseStats)
    {
        srct = baseStats[RacerProfile.Stat.SRCT];
        sspd = baseStats[RacerProfile.Stat.SSPD];
        acc =  baseStats[RacerProfile.Stat.ACC];
        pwr =  baseStats[RacerProfile.Stat.PWR];
        stm =  baseStats[RacerProfile.Stat.STM];
        com =  baseStats[RacerProfile.Stat.COM];


        adjustedStats = new Dictionary<RacerProfile.Stat, float>()
        {
            {RacerProfile.Stat.SRCT,  baseStats[RacerProfile.Stat.SRCT]                                                                      },
            {RacerProfile.Stat.SSPD, (baseStats[RacerProfile.Stat.SSPD]     *   0.25f )     +    1f         },
            {RacerProfile.Stat.ACC,  (baseStats[RacerProfile.Stat.ACC]     *   0.07f )     +    9.3f       },
            {RacerProfile.Stat.PWR,  (baseStats[RacerProfile.Stat.PWR]     *   0.017f)     +    0.7f       },
            {RacerProfile.Stat.STM,  (baseStats[RacerProfile.Stat.STM]     *   3     )     +    10         },
            {RacerProfile.Stat.COM,   baseStats[RacerProfile.Stat.COM]                                     },
        };

        current_stamina = (int)adjustedStats[RacerProfile.Stat.STM];
    }


    public float srct, sspd, acc, pwr, stm, com, dd;

    private void Update() //for debuging
    {
        //srct = adjustedStats[RacerProfile.Stat.SRCT];
        //sspd = adjustedStats[RacerProfile.Stat.SSPD];
        //acc = adjustedStats[RacerProfile.Stat.ACC];
        //pwr = adjustedStats[RacerProfile.Stat.PWR];
        //stm = adjustedStats[RacerProfile.Stat.STM];
        //com = adjustedStats[RacerProfile.Stat.COM];
        //
        //dd = dudDecay;
    }

    IEnumerator StartPhase()
    {
        //float startDelay = 1f - (racerProfile.start_Reaction / 10f);

        yield return new WaitForSeconds(0);

        StartCoroutine(SpeedController());

        Step();

        //Debug.Log(gameObject.name + "'s Start Delay: " + startDelay + " Start Speed: " + current_speed);
    }

    public void Step()
    {
        if (finished)
        {
            Deccelerate();
        }
        else if (GetComponent<Competitve_Edge>() != null && GetComponent<Competitve_Edge>().Conditon())
        {
            
        }
        else if (stepsTaken == 0)
        {
            current_speed = adjustedStats[RacerProfile.Stat.SSPD];
        }
        else if (current_stamina > 0)
        {
            PowerStep();

            adjustedStats[RacerProfile.Stat.PWR] -= adjustedStats[RacerProfile.Stat.PWR] * 0.05f;

            current_stamina--;

            if (current_stamina <= 0)
                sweat.SetActive(true);
        }
        else
        {
            DudStep();

            if(adjustedStats[RacerProfile.Stat.COM] > 0)
                adjustedStats[RacerProfile.Stat.COM] -= (adjustedStats[RacerProfile.Stat.COM] * 0.005f);
        }

        if(!finished)
            stepsTaken++;
    }

    IEnumerator SpeedController()
    {
        while (true)
        {
            transform.Translate(Vector2.down * current_speed * Time.deltaTime);

            if (current_speed > top_speed)
                top_speed = current_speed;

            yield return null;
        }
    }


    bool PowerStep()
    {
        if (Random.Range(0f, 10f) < adjustedStats[RacerProfile.Stat.ACC])
        {
            current_speed += adjustedStats[RacerProfile.Stat.PWR];

            GetComponent<GhostMaker>().MakeGhost(Color.blue, 1.5f);

            powerSteps++;

            adjustedStats[RacerProfile.Stat.ACC] -=  adjustedStats[RacerProfile.Stat.ACC] * 0.015f;

            return true;
        }

        return false;
    }

    bool DudStep()
    {
        

        if (adjustedStats[RacerProfile.Stat.COM] < Random.Range(0f, 10f))
        {
            current_speed -= dudDecay;

            if (current_speed <= 1)
            {
                current_speed = 1;
                Event_Walk?.Invoke();
            }

            GetComponent<GhostMaker>().MakeGhost(Color.red);

            dudSteps++;

            dudDecay += 0.01f;

            return true;
        }

        return false;
    }


    public void FinishedPhase()
    {
        stepPower = -6f;
        finished = true;
    }

    void Deccelerate()
    {
        if (current_speed + stepPower < 1)
        {
            current_speed = 0;
            Event_HandKnees?.Invoke();
        }
        else
            current_speed += stepPower;
    }

    public void Eliminate()
    {
        GetComponent<GhostMaker>().MakeGhost(Color.white, 1.5f);
        Destroy(gameObject);
    }

}
