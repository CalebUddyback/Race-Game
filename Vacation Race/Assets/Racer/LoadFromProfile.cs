using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class LoadFromProfile : MonoBehaviour
{
    readonly string _FILEPATH = "Assets/Racer Profiles/";

    readonly string _COMPEDGEPATH = "Assets/Competitive Edges/";

    public RacerProfile racerProfile;

    public RacerProfile Profile { get; } // Use in future to prevent external profile changing

    public IEnumerator Loading(RacerProfile profile)
    {
        racerProfile = profile;

        gameObject.name = racerProfile.name;

        //RacerProfile racer = GetComponent<Racer_Script>().racer = (RacerProfile)AssetDatabase.LoadAssetAtPath(_FILEPATH + gameObject.name + ".asset", typeof(RacerProfile));

        if (transform.Find("Hud"))
            transform.Find("Hud").GetComponent<Racer_UI>().racer_name.text = racerProfile.name;

        //Cosmetics

        //Skin
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_SkinColor", racerProfile.skin_Color);
        //Eye
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_EyeColor", racerProfile.eye_Color);
        //Shirt
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_ShirtColor", racerProfile.shirt_Color);
        //Pants
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_PantsColor", racerProfile.pant_Color);
        //Shoe
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_ShoeColor", racerProfile.shoe_Color);

        Object[] allStyles = Resources.LoadAll("Head/");
        GameObject headAddon = Instantiate(allStyles[racerProfile.head_Addon] as GameObject, transform.Find("Sprite/Head Addon Point"));

        headAddon.GetComponent<SpriteRenderer>().material.SetColor("_PrimaryColor", racerProfile.head_Addon_Color);

        allStyles = Resources.LoadAll("Face/");
        Instantiate(allStyles[racerProfile.face_Addon] as GameObject, transform.Find("Sprite/Head Addon Point"));

        if(racerProfile.comp_edge != "")
            gameObject.AddComponent(System.Type.GetType("HotOffTheBlocks,Assembly-CSharp"));

        yield return new WaitForEndOfFrame();   // this allows events to be added to calls!

        //Destroy(this);
    }
}
