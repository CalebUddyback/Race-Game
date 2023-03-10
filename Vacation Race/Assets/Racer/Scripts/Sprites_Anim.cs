using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites_Anim : MonoBehaviour
{
    private Animator anim;

    private Racer_Script racer_Script;

    private void Start()
    {
        anim = GetComponent<Animator>();

        racer_Script = transform.parent.root.GetComponent<Racer_Script>();

        racer_Script.Event_Idle += Animation_Idle;
        racer_Script.Event_Run += Animation_Run;
        racer_Script.Event_Ghosts += Animation_Ghosts;
        racer_Script.Event_Walk += Animation_Walk;
        racer_Script.Event_HandKnees += Animation_HandKnees;
    }

    Coroutine ghostCoroutine;

    public void Animation_Ghosts(bool check)
    {
        if (check)
        {
            if(GetComponent<SpriteRenderer>().enabled)
                ghostCoroutine = StartCoroutine(Ghosts());
        }
        else
        {
            if (ghostCoroutine != null)
            {
                StopCoroutine(ghostCoroutine);
            }
        }

    }

    IEnumerator Ghosts()
    {

        // Burst Effect

        GameObject current_ghost = Instantiate(racer_Script.ghost, transform.position, transform.rotation);

        Sprite current_sprite = GetComponent<SpriteRenderer>().sprite;

        current_ghost.GetComponent<SpriteRenderer>().sprite = current_sprite;

        current_ghost.name = gameObject.name + " Ghost";
        current_ghost.GetComponent<Animator>().speed = racer_Script.ghost_deathSpeed;


        while (true)
        {
            yield return new WaitForSeconds(racer_Script.ghost_frequency);

            current_ghost = Instantiate(racer_Script.ghost, transform.position, transform.rotation);
            current_sprite = GetComponent<SpriteRenderer>().sprite;
            current_ghost.GetComponent<SpriteRenderer>().sprite = current_sprite;

            current_ghost.transform.localScale = transform.lossyScale;
            current_ghost.GetComponent<SpriteRenderer>().material.color = racer_Script.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_ShirtColor");

            current_ghost.name = gameObject.name + " Ghost";
            current_ghost.GetComponent<Animator>().speed = racer_Script.ghost_deathSpeed;

            yield return null;
        }
    }

    public void Animation_Idle() => anim.SetTrigger("Idle");
    public void Animation_Run() => anim.SetTrigger("Run");
    public void Animation_Walk() => anim.SetTrigger("Walk");
    public void Animation_HandKnees() => anim.SetTrigger("HandKnees");
}
