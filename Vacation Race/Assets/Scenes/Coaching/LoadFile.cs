using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class LoadFile : MonoBehaviour
{
    private RacerList racerList;
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

        if (racerList.racers.Count > 0)
        {

            /*START*/

            GameObject racerModel = GameObject.Find("Racer");

            racerModel.name = racerList.racers[0];

            racerModel.GetComponent<LoadFromFile>().Load();

            yield return new WaitUntil(() => racerModel.GetComponent<LoadFromFile>().doneLoading);

            racerModel.name = "Racer";

            nameText.gameObject.SetActive(true);
            nameText.text = racerList.racers[0];


            Color currSkinColor = racerModel.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_SkinColor");

            for (int i = 0; i < skinColors.childCount; i++)
            {
                if (currSkinColor == skinColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    skinColors.GetComponent<ColorSelect>().SelectColor(skinColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currEyeColor = racerModel.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_EyeColor"); ;

            for (int i = 0; i < eyeColors.childCount; i++)
            {
                if (currEyeColor == eyeColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    eyeColors.GetComponent<ColorSelect>().SelectColor(eyeColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShirtColor = racerModel.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_ShirtColor");

            for (int i = 0; i < skinColors.childCount; i++)
            {
                if (currShirtColor == shirtColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    shirtColors.GetComponent<ColorSelect>().SelectColor(shirtColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currPantsColor = racerModel.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_PantsColor");

            for (int i = 0; i < pantsColors.childCount; i++)
            {
                if (currPantsColor == pantsColors.GetChild(i).GetComponent<Button>().colors.normalColor)
                {
                    pantsColors.GetComponent<ColorSelect>().SelectColor(pantsColors.GetChild(i).GetComponent<Button>());
                    break;
                }
            }

            Color currShoeColor = racerModel.transform.GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_ShoeColor");

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
                string a = racerModel.transform.GetChild(0).GetChild(0).name.Replace("Hair_", "");
                a = a.Replace("(Clone)", "");

                headAddon.GetComponent<StyleSelect>().addonInstance = racerModel.transform.GetChild(0).GetChild(0).gameObject;
                headAddon.GetComponent<StyleSelect>().addonNumText.text = a;
            }

            Color currHeadAddonColor = racerModel.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().material.GetColor("_PrimaryColor");

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
                string a = racerModel.transform.GetChild(0).GetChild(1).name.Replace("Face_", "");
                a = a.Replace("(Clone)", "");

                faceStyle.GetComponent<StyleSelect>().addonInstance = racerModel.transform.GetChild(0).GetChild(1).gameObject;
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

            racer.name = racerList.racers[0];

            racer.GetComponent<LoadFromFile>().Load();

            yield return new WaitUntil(() => racer.GetComponent<LoadFromFile>().doneLoading);

            racer.name = "Racer";

            nameText.gameObject.SetActive(true);
            nameText.text = racerList.racers[0];


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

            string racerName = racerList.racers[0];

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
