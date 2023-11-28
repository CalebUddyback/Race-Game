using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotOffTheBlocks : Competitve_Edge
{
    public override void StatChanges()
    {
        //racer.adjustedStats[RacerProfile.Stat.SSPD] += 0.1f;
    }

    public override bool Conditon()
    {
        if (racer.stepsTaken == 0)
        {
           //racer.current_speed = racer.adjustedStats[RacerProfile.Stat.SSPD];

            racer.GetComponent<GhostMaker>().MakeGhost(Color.yellow);
            return true;
        }
        else
        {
            return false;
        }
    }
}
