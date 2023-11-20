using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer_Script : MonoBehaviour
{
    private RacerProfile racerProfile;

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

    public Dictionary<RacerProfile.Stat, float> adjustedStat;

    private void Start()
    {
        racerProfile = GetComponent<LoadFromProfile>().racerProfile;

        adjustedStat = new Dictionary<RacerProfile.Stat, float>()
        {
            {RacerProfile.Stat.SRCT, 0                                                              },
            {RacerProfile.Stat.SSPD, (racerProfile.start_Speed      *   0.25f )     +    1f         },
            {RacerProfile.Stat.ACC,  (racerProfile.acceleration     *   0.07f )     +    9.3f       },
            {RacerProfile.Stat.PWR,  (racerProfile.power            *   0.017f)     +    0.7f       },
            {RacerProfile.Stat.STM,  (racerProfile.stamina          *   3     )     +    10         },
            //{RacerProfile.Stat.COM,  (racerProfile.composure        *   0.9f  )     +    1f         },
            {RacerProfile.Stat.COM,  (racerProfile.composure) },
        };

        current_stamina = (int)adjustedStat[RacerProfile.Stat.STM];
    }


    public float sspd, acc, pwr, stm, com, dd;

    private void Update() //for debuging
    {
        sspd = adjustedStat[RacerProfile.Stat.SSPD];
        acc = adjustedStat[RacerProfile.Stat.ACC];
        pwr = adjustedStat[RacerProfile.Stat.PWR];
        stm = adjustedStat[RacerProfile.Stat.STM];
        com = adjustedStat[RacerProfile.Stat.COM];

        dd = dudDecay;
    }

    IEnumerator StartPhase()
    {
        float startDelay = 1f - (racerProfile.start_Reaction / 10f);

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
            current_speed = adjustedStat[RacerProfile.Stat.SSPD];
        }
        else if (current_stamina > 0)
        {
            PowerStep();

            adjustedStat[RacerProfile.Stat.PWR] -= adjustedStat[RacerProfile.Stat.PWR] * 0.05f;

            current_stamina--;

            if (current_stamina <= 0)
                sweat.SetActive(true);
        }
        else
        {
            DudStep();

            if(adjustedStat[RacerProfile.Stat.COM] > 0)
                adjustedStat[RacerProfile.Stat.COM] -= (adjustedStat[RacerProfile.Stat.COM] * 0.005f);
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
        if (Random.Range(0f, 10f) < adjustedStat[RacerProfile.Stat.ACC])
        {
            current_speed += adjustedStat[RacerProfile.Stat.PWR];

            GetComponent<GhostMaker>().MakeGhost(Color.blue, 1.5f);

            powerSteps++;

            adjustedStat[RacerProfile.Stat.ACC] -=  adjustedStat[RacerProfile.Stat.ACC] * 0.015f;

            return true;
        }

        return false;
    }

    bool DudStep()
    {
        

        if (adjustedStat[RacerProfile.Stat.COM] < Random.Range(0f, 10f))
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
