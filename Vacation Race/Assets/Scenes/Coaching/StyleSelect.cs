using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleSelect : MonoBehaviour
{
    public Text addonNumText;

    public GameObject addonInstance;

    public Color addonColor;

    public Object[] allAddons;

    private void Awake()
    {
        allAddons = Resources.LoadAll(transform.parent.parent.gameObject.name + "/");
    }

    public void StyleIncrement(int i)
    {
        int x = int.Parse(addonNumText.text) + i;

        if (x > allAddons.Length-1)
            x = 0;
        else if (x < 0)
            x = allAddons.Length-1;

        addonNumText.text = x.ToString();

        

        if (addonInstance)
        {
            addonColor = addonInstance.GetComponent<SpriteRenderer>().material.GetColor("_PrimaryColor");

            GameObject.Find("Racer").GetComponent<Racer_Script>().Event_Idle -= addonInstance.GetComponent<Sprites_Anim>().Animation_Idle;
            GameObject.Find("Racer").GetComponent<Racer_Script>().Event_Run -= addonInstance.GetComponent<Sprites_Anim>().Animation_Run;
            Destroy(addonInstance);
        }

        addonInstance = Instantiate(allAddons[x] as GameObject, GameObject.Find("Racer/Sprite/Head Addon Point").transform);
        addonInstance.GetComponent<SpriteRenderer>().material.SetColor("_PrimaryColor", addonColor);

        GameObject.Find("Racer").GetComponent<Racer_Script>().Idle();

    }
}
