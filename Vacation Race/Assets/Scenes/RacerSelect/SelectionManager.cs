using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class SelectionManager : MonoBehaviour
{
    public GameObject racerListPrefab;
    public GameObject racerButtonPrefab;

    public int MAX_RACERS = 8;
    private List<string> selectedList = new List<string>();

    public Transform listUI;
    public GameObject listElement;

    public Button continueButton;

    // Start is called before the first frame update
    void Start()
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

        string[] filePaths = Directory.GetFiles(Application.streamingAssetsPath + "/Racers/", "*.txt");         // Put all racers into string array

        for (int i = 0; i < selectedList.Count(); i++)
        {
            if (!File.Exists(Application.streamingAssetsPath + "/Racers/" + selectedList[i] + ".txt"))          // Make sure Racer file exists
            {
                UpdateList(selectedList[i]);
                i--;
            }
        }

        foreach (string file in filePaths)
        {
            Button button = Instantiate(racerButtonPrefab, transform).GetComponent<Button>();       // create button

            List<string> fileLines = File.ReadAllLines(file).ToList();                              // store race file lines in list    

            button.transform.GetChild(0).GetComponent<Text>().text = fileLines[1];                  // set button text to racer name

            if (selectedList.Contains(fileLines[1]))                                                // if racer already in selected list
            {
                button.GetComponent<RacerSelect>().UpdateColor(true);
                button.GetComponent<RacerSelect>().selected = true;
            }
        }
    }

    public bool UpdateList(string racerName)
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
                element.GetChild(0).GetComponent<Text>().text = selectedList[i];
            }

            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }
}
