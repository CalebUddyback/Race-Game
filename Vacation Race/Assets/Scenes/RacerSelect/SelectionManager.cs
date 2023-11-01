using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class SelectionManager : MonoBehaviour
{
    readonly string _FILEPATH = "Assets/Racer Profiles/";

    public GameObject racerListPrefab;
    public GameObject racerButtonPrefab;

    public int MAX_RACERS = 8;
    private List<RacerProfile> selectedList = new List<RacerProfile>();

    public Transform listUI;
    public GameObject listElement;

    public Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        RacerList racerList;

        if (GameObject.Find("RacerList"))
        {
            racerList = GameObject.Find("RacerList").GetComponent<RacerList>();

            if (racerList.maxRacers != MAX_RACERS)
            {
                racerList.racers.Clear();
            }
        }
        else
        {
            racerList = Instantiate(racerListPrefab, null).GetComponent<RacerList>();
            racerList.gameObject.name = "RacerList";
            racerList.maxRacers = MAX_RACERS;
        }

        selectedList = racerList.racers;

        UpdateUI();

        string[] filePaths = Directory.GetFiles(_FILEPATH, "*.asset");         // Put all racers into string array

        for (int i = 0; i < selectedList.Count(); i++)
        {
            if (!File.Exists(_FILEPATH + selectedList[i] + ".asset"))          // Make sure Racer file exists
            {
                UpdateList(selectedList[i]);
                i--;
            }
        }

        foreach (string racerAsset in filePaths)
        {
            Button button = Instantiate(racerButtonPrefab, transform).GetComponent<Button>();       // create button 

            RacerProfile racer_profile =  button.GetComponent<RacerSelect>().racer_profile = (RacerProfile)AssetDatabase.LoadAssetAtPath(racerAsset, typeof(RacerProfile));

            button.transform.GetChild(0).GetComponent<Text>().text = racer_profile.name;                  // set button text to racer name

            if (selectedList.Contains(racer_profile))                                                // if racer already in selected list
            {
                button.GetComponent<RacerSelect>().UpdateColor(true);
                button.GetComponent<RacerSelect>().selected = true;
            }
        }
    }

    public bool UpdateList(RacerProfile racerName)
    {

        if (!selectedList.Contains(racerName))
        {
            if (selectedList.Count < MAX_RACERS)
            {
                selectedList.Add(racerName);

                UpdateUI();

                Debug.Log("Added");
                return true;
            }
            else
            {
                Debug.Log("Full");
                return false;
            }
        }
        else
        {
            selectedList.Remove(racerName);

            UpdateUI();

            Debug.Log("Removed");
            return true;
        }
    }

    private void UpdateUI()
    {

        foreach (Transform child in listUI)
        {
            Destroy(child.gameObject);
        }

        if (selectedList.Count > 0)
        {
            for (int i = 0; i < selectedList.Count; i++)
            {
                Transform element = Instantiate(listElement, listUI).transform;
                element.GetChild(0).GetComponent<Text>().text = selectedList[i].name;
            }

            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }
}
