using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites_Anim : MonoBehaviour
{

    /* Animation speed conversion is controlled in the "Play_Speed" behaviour */


    private Animator anim;

    [HideInInspector]
    public Racer_Script racer_Script;

    public bool canHaveGhost = true;


    private void Awake()
    {
        anim = GetComponent<Animator>();

        Racer_Script par = null;

        Transform tran = transform;

        while (par == null)
        {
            tran = tran.transform.parent;
            par = tran.GetComponent<Racer_Script>();
        }

        racer_Script = par;

        //racer_Script = transform.parent.root.GetComponent<Racer_Script>();

        racer_Script.Event_Idle += Animation_Idle;
        racer_Script.Event_Set += Animation_Set;
        racer_Script.Event_Run += Animation_Run;
        racer_Script.Event_Walk += Animation_Walk;
        racer_Script.Event_HandKnees += Animation_HandKnees;      
    }

    public void Animation_Idle() => anim.SetTrigger("Idle");
    public void Animation_Set() => anim.SetTrigger("Set");
    public void Animation_Run() => anim.SetTrigger("Run");
    public void Animation_Walk() => anim.SetTrigger("Walk");
    public void Animation_HandKnees() => anim.SetTrigger("HandKnees");

    public void Step()
    {
        racer_Script.Step();
    }
}
