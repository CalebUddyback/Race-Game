using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Racer", menuName ="Racer")]
public class RacerProfile : ScriptableObject
{
    public string _name;

    public Color skin_Color;
    public Color eye_Color;
    public Color shirt_Color;
    public Color pant_Color;
    public Color shoe_Color;

    public int head_Addon;
    public Color head_Addon_Color;

    public int face_Addon;


    [Header("Stats")]

    public int points;

    public int start_Reaction;
    public int start_Speed;
    public int acceleration;
    public int power;
    public int stamina;
    public int composure;


    public enum Stat
    {
        SRCT,
        SSPD,
        ACC,
        PWR,
        STM,
        COM,
    };

    public Dictionary<Stat, float> GetBaseStats
    {
        get
        {

            Dictionary<Stat, float> newDictionary = new Dictionary<Stat, float>
            {
                {Stat.SRCT, 0            },
                {Stat.SSPD, start_Speed  },
                {Stat.ACC,  acceleration },
                {Stat.PWR,  power        },
                {Stat.STM,  stamina      },
                {Stat.COM,  composure    },
            };

            return newDictionary;
        }
    }

    public Stat[] upgrades = new Stat[7];

    public string comp_edge = "";
}
