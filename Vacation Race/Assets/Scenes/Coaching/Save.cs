using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class Save : MonoBehaviour
{

    readonly string _FILEPATH = "Assets/Racer Profiles/";

    public InputField newRacerName;
    public Text racerName;

    public Text points;

    public Text startReact;
    public Text startSpeed;
    public Text acceleration;
    public Text power;
    public Text stamina;
    public Text composure;

    public ColorSelect skinColor;
    public ColorSelect eyeColor;
    public ColorSelect shirtColor;
    public ColorSelect pantsColor;
    public ColorSelect shoeColor;
    public Text headAddon;
    public ColorSelect headAddonColor;
    public Text faceAddon;
    public ColorSelect faceAddonColor;


    // Start is called before the first frame update
    void Start()
    {
        if(!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        if(!Directory.Exists(Application.streamingAssetsPath + "/Racers/"))
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Racers/");   
    }

    public void CreateProfile2()
    {
        string _name = "";

        if (newRacerName.gameObject.activeSelf)
        {
            if (newRacerName.text == "")
            {
                return;
            }

            _name = newRacerName.text;
        }
        else
        {
            _name = racerName.text;
        }

        RacerProfile racer = ScriptableObject.CreateInstance<RacerProfile>();

        /* STATS */

        racer.points = int.Parse(points.text);

        //racer.start_Reaction = int.Parse(startReact.text);
        racer.start_Speed = int.Parse(startSpeed.text);
        racer.acceleration = int.Parse(acceleration.text);
        racer.power = int.Parse(acceleration.text);
        racer.stamina = int.Parse(stamina.text);
        racer.composure = int.Parse(power.text);


        /* Cosmetics */

        racer.skin_Color = skinColor.currentButton.GetComponent<Button>().colors.normalColor;
        racer.eye_Color = eyeColor.currentButton.GetComponent<Button>().colors.normalColor;
        racer.shirt_Color = shirtColor.currentButton.GetComponent<Button>().colors.normalColor;
        racer.pant_Color = pantsColor.currentButton.GetComponent<Button>().colors.normalColor;
        racer.shoe_Color = shoeColor.currentButton.GetComponent<Button>().colors.normalColor;

        racer.head_Addon = int.Parse(headAddon.text);
        racer.head_Addon_Color = headAddonColor.currentButton.GetComponent<Button>().colors.normalColor;

        racer.face_Addon = int.Parse(faceAddon.text);



        string path = _FILEPATH + _name + ".asset";

        AssetDatabase.CreateAsset(racer, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = racer;
    }

    public void CreateProfile()
    {

        string _name = "";

        if (newRacerName.gameObject.activeSelf)
        {
            if (newRacerName.text == "")
            {
                return;
            }

            _name = newRacerName.text;
        }
        else
        {
            _name = racerName.text;
        }

        string txtDocumentName = Application.streamingAssetsPath + "/Racers/" + _name + ".txt";

        File.WriteAllText(txtDocumentName, "Name:\n");
        File.AppendAllText(txtDocumentName, _name + "\n");
        File.AppendAllText(txtDocumentName, "AvailablePoints:\n");
        File.AppendAllText(txtDocumentName, "1" + "\n");
        File.AppendAllText(txtDocumentName, "Start:\n");
        File.AppendAllText(txtDocumentName, startReact.text + "\n");
        File.AppendAllText(txtDocumentName, "Acceleration:\n");
        File.AppendAllText(txtDocumentName, acceleration.text + "\n");
        File.AppendAllText(txtDocumentName, "Top Speed:\n");
        File.AppendAllText(txtDocumentName, power.text + "\n");
        File.AppendAllText(txtDocumentName, "Stamina:\n");
        File.AppendAllText(txtDocumentName, stamina.text + "\n");
        File.AppendAllText(txtDocumentName, "Skin Color:\n");
        File.AppendAllText(txtDocumentName, skinColor.currentButton.GetComponent<Button>().colors.normalColor.r + "\n");
        File.AppendAllText(txtDocumentName, skinColor.currentButton.GetComponent<Button>().colors.normalColor.g + "\n");
        File.AppendAllText(txtDocumentName, skinColor.currentButton.GetComponent<Button>().colors.normalColor.b + "\n");
        File.AppendAllText(txtDocumentName, "Eye Color:\n");
        File.AppendAllText(txtDocumentName, eyeColor.currentButton.GetComponent<Button>().colors.normalColor.r + "\n");
        File.AppendAllText(txtDocumentName, eyeColor.currentButton.GetComponent<Button>().colors.normalColor.g + "\n");
        File.AppendAllText(txtDocumentName, eyeColor.currentButton.GetComponent<Button>().colors.normalColor.b + "\n");
        File.AppendAllText(txtDocumentName, "Shirt Color:\n");
        File.AppendAllText(txtDocumentName, shirtColor.currentButton.GetComponent<Button>().colors.normalColor.r + "\n");
        File.AppendAllText(txtDocumentName, shirtColor.currentButton.GetComponent<Button>().colors.normalColor.g + "\n");
        File.AppendAllText(txtDocumentName, shirtColor.currentButton.GetComponent<Button>().colors.normalColor.b + "\n");
        File.AppendAllText(txtDocumentName, "Pants Color:\n");
        File.AppendAllText(txtDocumentName, pantsColor.currentButton.GetComponent<Button>().colors.normalColor.r + "\n");
        File.AppendAllText(txtDocumentName, pantsColor.currentButton.GetComponent<Button>().colors.normalColor.g + "\n");
        File.AppendAllText(txtDocumentName, pantsColor.currentButton.GetComponent<Button>().colors.normalColor.b + "\n");
        File.AppendAllText(txtDocumentName, "Shoe Color:\n");
        File.AppendAllText(txtDocumentName, shoeColor.currentButton.GetComponent<Button>().colors.normalColor.r + "\n");
        File.AppendAllText(txtDocumentName, shoeColor.currentButton.GetComponent<Button>().colors.normalColor.g + "\n");
        File.AppendAllText(txtDocumentName, shoeColor.currentButton.GetComponent<Button>().colors.normalColor.b + "\n");
        File.AppendAllText(txtDocumentName, "Head Addon:\n");
        File.AppendAllText(txtDocumentName, headAddon.text + "\n");
        File.AppendAllText(txtDocumentName, "Head Addon Color:\n");
        File.AppendAllText(txtDocumentName, headAddonColor.currentButton.GetComponent<Button>().colors.normalColor.r + "\n");
        File.AppendAllText(txtDocumentName, headAddonColor.currentButton.GetComponent<Button>().colors.normalColor.g + "\n");
        File.AppendAllText(txtDocumentName, headAddonColor.currentButton.GetComponent<Button>().colors.normalColor.b + "\n");
        File.AppendAllText(txtDocumentName, "Face Addon:\n");
        File.AppendAllText(txtDocumentName, faceAddon.text + "\n");
    }
}
