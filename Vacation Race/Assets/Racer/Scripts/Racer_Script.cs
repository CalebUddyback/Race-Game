using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer_Script : MonoBehaviour
{
    public Racer racer;

    private float drain_speed = 2f;

    public int current_speed;
    public int current_stamina;

    public GameObject ghost;

    [HideInInspector]
    public float ghost_frequency = 0.07f;
    [HideInInspector]
    public float ghost_deathSpeed = 5;

    public GameObject sweat;

    public event System.Action<bool> Event_Ghosts;
    public event System.Action Event_Idle;
    public event System.Action Event_Walk;
    public event System.Action Event_Run;
    public event System.Action Event_HandKnees;

    void Start()
    {
        //current_stamina = racer.stamina;
    }

    public void Idle() => Event_Idle?.Invoke();

    public void GetSet() => Event_Run?.Invoke();

    public void GO() => StartCoroutine(StartPhase());

    IEnumerator StartPhase()
    {
        float startDelay = 1 / (float)Random.Range(2, 7 + racer.start_Reaction);

        yield return new WaitForSeconds(startDelay);

        current_speed = Random.Range(4, 7);

        StartCoroutine(SpeedController());
        StartCoroutine(StaminaController());

        Debug.Log(gameObject.name + "'s Start Delay: " + startDelay + " Start Speed: " + current_speed);
    }

    IEnumerator SpeedController()
    {
        while (transform.position.y > -110)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -110), Time.deltaTime * current_speed);

            yield return null;
        }   
    }

    IEnumerator StaminaController()
    {
        StartCoroutine(AccelerationPhase());

        while (HasStamina())
        {
            yield return new WaitForSeconds(drain_speed);
            current_stamina--;
            yield return null;
        }

        StartCoroutine(FatiguePhase());
    }

    bool HasStamina()
    {
        if (current_stamina > 0)
            return true;
        else
            return false;
    }

    IEnumerator AccelerationPhase()
    {
        drain_speed /= 2;

        while (current_speed < racer.top_Speed && HasStamina())
        {
            yield return new WaitForSeconds(1 / (1 + racer.acceleration));
            current_speed++;
            yield return null;
        }

        if (current_speed == racer.top_Speed)
        {
            Event_Ghosts?.Invoke(true);
        }

        drain_speed *= 2;
    }

    IEnumerator FatiguePhase()
    {
        Event_Ghosts?.Invoke(false);

        sweat.SetActive(true);

        while (current_speed > 1)
        {
            current_speed--;
            yield return new WaitForSeconds(drain_speed);
            yield return null;
        }

        Event_Walk?.Invoke();
    }

    public void FinishedPhase()
    {
        StopAllCoroutines();
        StartCoroutine(SpeedController());
        StartCoroutine(Deccelerate());

        Event_Ghosts?.Invoke(false);
    }

    IEnumerator Deccelerate()
    {
        drain_speed = 0.2f;

        while (current_speed > 0)
        {
            yield return new WaitForSeconds(drain_speed);
            current_speed--;
            yield return null;
        }

        Event_HandKnees?.Invoke();
    }

    public void Eliminate()
    {
        Event_Ghosts?.Invoke(true);
        Event_Ghosts?.Invoke(false);
        Destroy(gameObject);
    }

}
