using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Racer_UI : MonoBehaviour
{
    public Text racer_name;
    public Text speed_text;
    public Image stamina_bar;
    public Image speed_bar;

    private Racer_Script racer_Script;
    private RacerProfile racer_Stats;

    // Start is called before the first frame update
    void Start()
    {
        racer_Script = transform.parent.GetComponent<Racer_Script>();
        racer_Stats = transform.parent.GetComponent<LoadFromProfile>().racerProfile;
    }

    // Update is called once per frame
    void Update()
    {
        speed_text.text = racer_Script.current_speed.ToString();

        stamina_bar.fillAmount = (float)racer_Script.current_stamina / racer_Stats.stamina;
        speed_bar.fillAmount = (float)racer_Script.current_speed / racer_Stats.composure;

        if (GetComponent<Racer_UI>().stamina_bar.gameObject.activeSelf == true)
        {
            if (racer_Script.current_speed == racer_Stats.composure)
                speed_bar.GetComponent<Animator>().SetBool("Blink", true);
            else
                speed_bar.GetComponent<Animator>().SetBool("Blink", false);
        }

    }
}
