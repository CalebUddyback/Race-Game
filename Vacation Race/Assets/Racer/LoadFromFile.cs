using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class LoadFromFile : MonoBehaviour
{
    readonly string _FILEPATH = "Assets/Racer Profiles/";

    public bool doneLoading = false;

    public void Load() => StartCoroutine(Loading());

    IEnumerator Loading()
    {
        Racer racer = GetComponent<Racer_Script>().racer = (Racer)AssetDatabase.LoadAssetAtPath(_FILEPATH + gameObject.name + ".asset", typeof(Racer));

        if (transform.Find("Hud"))
            transform.Find("Hud").GetComponent<Racer_UI>().racer_name.text = gameObject.name;

        //Cosmetics

        //Skin
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_SkinColor", racer.skin_Color);
        //Eye
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_EyeColor", racer.eye_Color);
        //Shirt
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_ShirtColor", racer.shirt_Color);
        //Pants
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_PantsColor", racer.pant_Color);
        //Shoe
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_ShoeColor", racer.shoe_Color);

        Object[] allStyles = Resources.LoadAll("Head/");
        GameObject headAddon = Instantiate(allStyles[racer.head_Addon] as GameObject, transform.Find("Sprite"));

        headAddon.GetComponent<SpriteRenderer>().material.SetColor("_PrimaryColor", racer.head_Addon_Color);

        allStyles = Resources.LoadAll("Face/");
        Instantiate(allStyles[racer.face_Addon] as GameObject, transform.Find("Sprite"));


        doneLoading = true;

        yield return null;
    }
}
