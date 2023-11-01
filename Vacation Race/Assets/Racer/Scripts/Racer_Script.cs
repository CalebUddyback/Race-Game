using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer_Script : MonoBehaviour
{
    private RacerProfile racerProfile;

    public float current_speed;
    public int current_stamina;

    private float stepPower;

    public GameObject ghost;

    public GameObject sweat;

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
            {RacerProfile.Stat.SRCT, 0                                                       },
            {RacerProfile.Stat.SSPD, (racerProfile.start_Speed      *   0.05f)  +    2f      },
            {RacerProfile.Stat.ACC,  (racerProfile.acceleration     *   0.05f)  +    9.5f    },
            {RacerProfile.Stat.PWR,  (racerProfile.power            *   0.05f)  +    0.5f    },
            {RacerProfile.Stat.STM,  (racerProfile.stamina          +   1    )  *    7       },
            {RacerProfile.Stat.COM,  (racerProfile.composure        *   0.6f )  -    4f      },
        };
    }

    IEnumerator StartPhase()
    {
        float startDelay = 1f - (racerProfile.start_Reaction / 10f);

        yield return new WaitForSeconds(0);

        StartCoroutine(SpeedController());

        Step();

        Debug.Log(gameObject.name + "'s Start Delay: " + startDelay + " Start Speed: " + current_speed);
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

            if (current_stamina > 1 && PowerStep())
                current_stamina -= 2;
            else
                current_stamina--;

            if (current_stamina <= 0)
                sweat.SetActive(true);
        }
        else
        {
            DudStep();
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
        float success = 9.5f + (racerProfile.acceleration * 0.05f); // This should not be called every step

        success -= (powerSteps * 0.05f);

        if (Random.Range(0f, 10f) < success)
        {
            float actual_power = 0.5f + (racerProfile.power * 0.05f);

            actual_power -= (stepsTaken * 0.02f);

            current_speed += actual_power;

            GetComponent<GhostMaker>().MakeGhost(Color.blue, 1.5f);

            powerSteps++;

            return true;
        }

        return false;
    }

    bool DudStep()
    {
        float dud = 4f - (racerProfile.composure * 0.6f);

        if (Random.Range(0f, 10f) < dud)
        {
            current_speed -= 0.6f;

            if (current_speed <= 1)
            {
                current_speed = 1;
                Event_Walk?.Invoke();
            }

            GetComponent<GhostMaker>().MakeGhost(Color.red);

            dudSteps++;

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
