using UnityEngine;

[CreateAssetMenu(fileName = "New Racer", menuName ="Racer")]
public class Racer : ScriptableObject
{
    public int start_Reaction;
    public int acceleration;
    public int top_Speed;
    public int stamina;

    public Color skin_Color;
    public Color eye_Color;
    public Color shirt_Color;
    public Color pant_Color;
    public Color shoe_Color;

    public int head_Addon;
    public Color head_Addon_Color;

    public int face_Addon;
}
