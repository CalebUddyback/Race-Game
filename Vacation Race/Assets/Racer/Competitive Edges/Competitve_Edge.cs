using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Competitve_Edge : MonoBehaviour
{
    [HideInInspector]
    public Racer_Script racer;

    private void Start()
    {
        racer = GetComponent<Racer_Script>();
        StatChanges();
    }

    public virtual void StatChanges() { }

    public abstract bool Conditon();
}
