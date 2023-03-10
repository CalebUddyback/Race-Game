using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class LoadFromFile : MonoBehaviour
{
    public bool doneLoading = false;

    public void Load() => StartCoroutine(Loading());

    IEnumerator Loading()
    {
        //Load Racer Profile

        string readFromFilePath = Application.streamingAssetsPath + "/Racers/" + gameObject.name + ".txt";

        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        //Stats
        if (transform.Find("Hud"))
            transform.Find("Hud").GetComponent<Racer_UI>().racer_name.text = fileLines[1];

        gameObject.name = fileLines[1];

        GetComponent<Stats_Script>().start_reaction += int.Parse(fileLines[5]);
        GetComponent<Stats_Script>().acceleration += int.Parse(fileLines[7]);
        GetComponent<Stats_Script>().top_speed += int.Parse(fileLines[9]);
        GetComponent<Stats_Script>().stamina += int.Parse(fileLines[11]);

        //Cosmetics

        //Skin
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_SkinColor", new Color(float.Parse(fileLines[13]), float.Parse(fileLines[14]), float.Parse(fileLines[15])));
        //Eye
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_EyeColor", new Color(float.Parse(fileLines[17]), float.Parse(fileLines[18]), float.Parse(fileLines[19])));
        //Shirt
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_ShirtColor", new Color(float.Parse(fileLines[21]), float.Parse(fileLines[22]), float.Parse(fileLines[23])));
        //Pants
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_PantsColor", new Color(float.Parse(fileLines[25]), float.Parse(fileLines[26]), float.Parse(fileLines[27])));
        //Shoe
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_ShoeColor", new Color(float.Parse(fileLines[29]), float.Parse(fileLines[30]), float.Parse(fileLines[31])));

        Object[] allStyles = Resources.LoadAll("Head/");
        GameObject headAddon = Instantiate(allStyles[int.Parse(fileLines[33])] as GameObject, transform.Find("Sprite"));

        headAddon.GetComponent<SpriteRenderer>().material.SetColor("_PrimaryColor", new Color(float.Parse(fileLines[35]), float.Parse(fileLines[36]), float.Parse(fileLines[37])));

        allStyles = Resources.LoadAll("Face/");
        Instantiate(allStyles[int.Parse(fileLines[39])] as GameObject, transform.Find("Sprite"));


        doneLoading = true;

        yield return null;
    }
}
