using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class LoadEditMenu : MonoBehaviour
{
    private RacerList racerList;


    public Points_Bar points;
    public Stat_Bar start_spd;
    public Stat_Bar accel;
    public Stat_Bar pwr;
    public Stat_Bar stm;
    public Stat_Bar cmp;


    public Transform editCanavas;
    public GameObject inputName;
    public Text nameText;
    public Transform skinColors;
    public Transform eyeColors;
    public Transform shirtColors;
    public Transform pantsColors;
    public Transform shoeColors;
    public GameObject headAddon;
    public Transform headAddonColor;
    public GameObject faceStyle;

    public Canvas loadingScreen;

    void Start()
    {
        if(GameObject.Find("RacerList")!=null)
            racerList = GameObject.Find("RacerList").GetComponent<RacerList>();

        StartCoroutine(Loading3());
    }

    IEnumerator Loading3()
    {
        Debug.Log("LOADING 3");

        Time.timeScale = 0;

        foreach (Transform child in editCanavas)
        {
            child.GetChild(1).gameObject.SetActive(true);
        }

        if ( racerList != null && racerList.racers.Count > 0)
        {   

            GameObject racerModel = GameObject.Find("Racer");

            racerModel.name = racerList.racers[0].name;

            yield return StartCoroutine(racerModel.GetComponent<LoadFromProfile>().Loading(racerList.racers[0]));

            racerModel.name = "Racer";

            nameText.gameObject.SetActive(true);
            nameText.text = racerList.racers[0].name;

            /*STATS*/

            points.Points = racerModel.GetComponent<LoadFromProfile>().racerProfile.points;

            start_spd.Points = racerModel.GetComponent<LoadFromProfile>().racerProfile.start_Speed;
            accel.Points = racerModel.GetComponent<LoadFromProfile>().racerProfile.acceleration;
            pwr.Points = racerModel.GetComponent<LoadFromProfile>().racerProfile.power;
            stm.Points = racerModel.GetComponent<LoadFromProfile>().racerProfile.stamina;
            cmp.Points = racerModel.GetComponent<LoadFromProfile>().racerProfile.composure;


            /*COSMETICS*/

            Material spriteMat = racerModel.transform.Find("Sprite").GetComponent<SpriteRenderer>().material;

            Color currSkinColor = spriteMat.GetColor("_SkinColor");

            for (int i = 0; i < skinColors.childCount; i++)
            {
                if (currSkinColor == skinColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    skinColors.GetComponent<ColorSelect>().SelectColor(skinColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currEyeColor = spriteMat.GetColor("_EyeColor"); ;

            for (int i = 0; i < eyeColors.childCount; i++)
            {
                if (currEyeColor == eyeColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    eyeColors.GetComponent<ColorSelect>().SelectColor(eyeColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShirtColor = spriteMat.GetColor("_ShirtColor");

            for (int i = 0; i < shirtColors.childCount; i++)
            {
                if (currShirtColor == shirtColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    shirtColors.GetComponent<ColorSelect>().SelectColor(shirtColors.GetChild(i).GetComponent<Button>());
                    break;
                }
                else
                {
                    print(currSkinColor + " /= " + shirtColors.GetChild(i).GetComponent<Button>().colors.normalColor);
                }
            }

            Color currPantsColor = spriteMat.GetColor("_PantsColor");

            for (int i = 0; i < pantsColors.childCount; i++)
            {
                if (currPantsColor == pantsColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    pantsColors.GetComponent<ColorSelect>().SelectColor(pantsColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShoeColor = spriteMat.GetColor("_ShoeColor");

            for (int i = 0; i < shoeColors.childCount; i++)
            {
                if (currShoeColor == shoeColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    shoeColors.GetComponent<ColorSelect>().SelectColor(shoeColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            yield return new WaitUntil(() => headAddon.GetComponent<StyleSelect>().allAddons.Length > 0);

            if (racerModel.transform.GetChild(0).GetChild(0))
            {
                string x = racerModel.transform.Find("Sprite/Head Addon Point").GetChild(0).name.Replace("Head_", "");
                x = x.Replace("(Clone)", "");

                headAddon.GetComponent<StyleSelect>().addonInstance = racerModel.transform.Find("Sprite/Head Addon Point").GetChild(0).gameObject;
                headAddon.GetComponent<StyleSelect>().addonNumText.text = x;
            }

            Color currHeadAddonColor = racerModel.transform.Find("Sprite/Head Addon Point").GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_PrimaryColor");

            for (int i = 0; i < headAddonColor.childCount; i++)
            {
                if (currHeadAddonColor == headAddonColor.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    headAddonColor.GetComponent<ColorSelect>().SelectColor(headAddonColor.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            yield return new WaitUntil(() => faceStyle.GetComponent<StyleSelect>().allAddons.Length > 0);

            if (racerModel.transform.GetChild(0).GetChild(1))
            {
                string a = racerModel.transform.Find("Sprite/Head Addon Point").GetChild(1).name.Replace("Face_", "");
                a = a.Replace("(Clone)", "");

                faceStyle.GetComponent<StyleSelect>().addonInstance = racerModel.transform.Find("Sprite/Head Addon Point").GetChild(1).gameObject;
                faceStyle.GetComponent<StyleSelect>().addonNumText.text = a;
            }

            /*END*/
        }
        else
        {
            inputName.SetActive(true);


            points.Points = 25;
            start_spd.Points = 0;
            accel.Points = 0;
            pwr.Points = 0;
            stm.Points = 0;
            cmp.Points = 0;

            skinColors.GetComponent<ColorSelect>().SelectColor(skinColors.GetChild(0).GetComponent<Button>());

            eyeColors.GetComponent<ColorSelect>().SelectColor(eyeColors.GetChild(0).GetComponent<Button>());

            shirtColors.GetComponent<ColorSelect>().SelectColor(shirtColors.GetChild(5).GetComponent<Button>());

            pantsColors.GetComponent<ColorSelect>().SelectColor(pantsColors.GetChild(0).GetComponent<Button>());

            shoeColors.GetComponent<ColorSelect>().SelectColor(shoeColors.GetChild(1).GetComponent<Button>());

            yield return new WaitUntil(() => headAddon.GetComponent<StyleSelect>().allAddons.Length > 0);
            headAddon.GetComponent<StyleSelect>().StyleIncrement(0);

            headAddonColor.GetComponent<ColorSelect>().SelectColor(headAddonColor.GetChild(2).GetComponent<Button>());

            yield return new WaitUntil(() => faceStyle.GetComponent<StyleSelect>().allAddons.Length > 0);
            faceStyle.GetComponent<StyleSelect>().StyleIncrement(0);
        }

        foreach (Transform child in editCanavas)
        {
            child.GetChild(1).gameObject.SetActive(false);
        }

        editCanavas.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);

        Time.timeScale = 1;

        loadingScreen.gameObject.SetActive(false);

        Debug.Log("DONE");

        yield return null;
    }

    IEnumerator Loading2()
    {
        Debug.Log("LOADING");

        Time.timeScale = 0;

        foreach (Transform child in editCanavas)
        {
            child.GetChild(1).gameObject.SetActive(true);
        }

        if (racerList.racers.Count > 0)
        {

            /*START*/

            GameObject racer = GameObject.Find("Racer");

            racer.name = racerList.racers[0].name;

            //racer.GetComponent<LoadFromProfile>().Load();

            //yield return new WaitUntil(() => racer.GetComponent<LoadFromProfile>().doneLoading);

            racer.name = "Racer";

            nameText.gameObject.SetActive(true);
            nameText.text = racerList.racers[0].name;


            Color currSkinColor = racer.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_SkinColor");

            for (int i = 0; i < skinColors.childCount; i++)
            {
                if (currSkinColor == skinColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    skinColors.GetComponent<ColorSelect>().SelectColor(skinColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currEyeColor = racer.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_EyeColor"); ;

            for (int i = 0; i < eyeColors.childCount; i++)
            {
                if (currEyeColor == eyeColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    eyeColors.GetComponent<ColorSelect>().SelectColor(eyeColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShirtColor = racer.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_ShirtColor");

            for (int i = 0; i < skinColors.childCount; i++)
            {
                if (currShirtColor == shirtColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    shirtColors.GetComponent<ColorSelect>().SelectColor(shirtColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currPantsColor = racer.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_PantsColor");

            for (int i = 0; i < pantsColors.childCount; i++)
            {
                if (currPantsColor == pantsColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    pantsColors.GetComponent<ColorSelect>().SelectColor(pantsColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShoeColor = racer.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_ShoeColor");

            for (int i = 0; i < shoeColors.childCount; i++)
            {
                if (currShoeColor == shoeColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    shoeColors.GetComponent<ColorSelect>().SelectColor(shoeColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            yield return new WaitUntil(() => headAddon.GetComponent<StyleSelect>().allAddons.Length > 0);

            if (racer.transform.GetChild(0).GetChild(0))
            {
                string a = racer.transform.GetChild(0).GetChild(0).name.Replace("Hair_", "");
                a = a.Replace("(Clone)", "");

                headAddon.GetComponent<StyleSelect>().addonInstance = racer.transform.GetChild(0).GetChild(0).gameObject;
                headAddon.GetComponent<StyleSelect>().addonNumText.text = a;
            }

            Color currHeadAddonColor = racer.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_PrimaryColor");

            for (int i = 0; i < headAddonColor.childCount; i++)
            {
                if (currHeadAddonColor == headAddonColor.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    headAddonColor.GetComponent<ColorSelect>().SelectColor(headAddonColor.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            yield return new WaitUntil(() => faceStyle.GetComponent<StyleSelect>().allAddons.Length > 0);

            if (racer.transform.GetChild(0).GetChild(1))
            {
                string a = racer.transform.GetChild(0).GetChild(1).name.Replace("Face_", "");
                a = a.Replace("(Clone)", "");

                faceStyle.GetComponent<StyleSelect>().addonInstance = racer.transform.GetChild(0).GetChild(1).gameObject;
                faceStyle.GetComponent<StyleSelect>().addonNumText.text = a;
            }

            /*END*/
        }
        else
        {
            inputName.SetActive(true);

            skinColors.GetComponent<ColorSelect>().SelectColor(skinColors.GetChild(0).GetComponent<Button>());

            eyeColors.GetComponent<ColorSelect>().SelectColor(eyeColors.GetChild(0).GetComponent<Button>());

            shirtColors.GetComponent<ColorSelect>().SelectColor(shirtColors.GetChild(5).GetComponent<Button>());

            pantsColors.GetComponent<ColorSelect>().SelectColor(pantsColors.GetChild(0).GetComponent<Button>());

            shoeColors.GetComponent<ColorSelect>().SelectColor(shoeColors.GetChild(1).GetComponent<Button>());

            yield return new WaitUntil(() => headAddon.GetComponent<StyleSelect>().allAddons.Length > 0);
            headAddon.GetComponent<StyleSelect>().StyleIncrement(0);

            headAddonColor.GetComponent<ColorSelect>().SelectColor(headAddonColor.GetChild(2).GetComponent<Button>());

            yield return new WaitUntil(() => faceStyle.GetComponent<StyleSelect>().allAddons.Length > 0);
            faceStyle.GetComponent<StyleSelect>().StyleIncrement(0);
        }

        foreach (Transform child in editCanavas)
        {
            child.GetChild(1).gameObject.SetActive(false);
        }

        editCanavas.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);

        Time.timeScale = 1;

        loadingScreen.gameObject.SetActive(false);

        Debug.Log("DONE");

        yield return null;
    }

    IEnumerator Loading()
    {
        if (racerList.racers.Count > 0)
        {

            Debug.Log("LOADING");

            yield return new WaitForSeconds(1);

            Time.timeScale = 0;

            foreach (Transform child in editCanavas)
            {
                child.GetChild(1).gameObject.SetActive(true);
            }


            /*START*/

            string racerName = racerList.racers[0].name;

            string readFromFilePath = Application.streamingAssetsPath + "/Racers/" + racerName + ".txt";

            List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

            GameObject racerObject = GameObject.Find("Racer");

            nameText.gameObject.SetActive(true);
            nameText.text = fileLines[1];


            Color currSkinColor = new Color(float.Parse(fileLines[13]), float.Parse(fileLines[14]), float.Parse(fileLines[15]));

            for (int i = 0; i < skinColors.childCount; i++)
            {
                if (currSkinColor == skinColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    skinColors.GetComponent<ColorSelect>().SelectColor(skinColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currEyeColor = new Color(float.Parse(fileLines[17]), float.Parse(fileLines[18]), float.Parse(fileLines[19]));

            for (int i = 0; i < eyeColors.childCount; i++)
            {
                if (currEyeColor == eyeColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    eyeColors.GetComponent<ColorSelect>().SelectColor(eyeColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShirtColor = new Color(float.Parse(fileLines[21]), float.Parse(fileLines[22]), float.Parse(fileLines[23]));

            for (int i = 0; i < skinColors.childCount; i++)
            {
                if (currShirtColor == shirtColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    shirtColors.GetComponent<ColorSelect>().SelectColor(shirtColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currPantsColor = new Color(float.Parse(fileLines[25]), float.Parse(fileLines[26]), float.Parse(fileLines[27]));

            for (int i = 0; i < pantsColors.childCount; i++)
            {
                if (currPantsColor == pantsColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    pantsColors.GetComponent<ColorSelect>().SelectColor(pantsColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShoeColor = new Color(float.Parse(fileLines[29]), float.Parse(fileLines[30]), float.Parse(fileLines[31]));

            for (int i = 0; i < shoeColors.childCount; i++)
            {
                if (currShoeColor == shoeColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    shoeColors.GetComponent<ColorSelect>().SelectColor(shoeColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            yield return new WaitUntil(() => headAddon.GetComponent<StyleSelect>().allAddons.Length > 0);
            headAddon.GetComponent<StyleSelect>().StyleIncrement(int.Parse(fileLines[33]));

            yield return new WaitUntil(() => faceStyle.GetComponent<StyleSelect>().allAddons.Length > 0);
            faceStyle.GetComponent<StyleSelect>().StyleIncrement(int.Parse(fileLines[35]));

            /*END*/

            Debug.Log("DONE");
        }
        else
        {
            inputName.SetActive(true);
        }

        foreach (Transform child in editCanavas)
        {
            child.GetChild(1).gameObject.SetActive(false);
        }

        editCanavas.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);

        Time.timeScale = 1;

        loadingScreen.gameObject.SetActive(false);

        yield return null;
    }
}
