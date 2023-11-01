using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Racer", menuName ="Racer")]
public class RacerProfile : ScriptableObject
{
    public Color skin_Color;
    public Color eye_Color;
    public Color shirt_Color;
    public Color pant_Color;
    public Color shoe_Color;

    public int head_Addon;
    public Color head_Addon_Color;

    public int face_Addon;


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

    public string comp_edge = "";
}
