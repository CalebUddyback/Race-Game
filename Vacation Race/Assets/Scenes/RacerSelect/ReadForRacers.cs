using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ReadForRacers : MonoBehaviour
{
    public GameObject racerButtonPrefab;

    public int MAX_RACERS = 8;
    private List<string> selectedList = new List<string>();

    public Transform listUI;
    public GameObject listElement;

    public Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        if (!File.Exists(Application.streamingAssetsPath + "/RacerList.txt"))
        {
            File.Create(Application.streamingAssetsPath + "/RacerList.txt");
            //delay needs to be added here
        }
        else
        {
            string readFromFile = Application.streamingAssetsPath + "/RacerList.txt";

            selectedList = File.ReadAllLines(readFromFile).ToList();

            UpdateUI();
        }

        string[] filePaths = Directory.GetFiles(Application.streamingAssetsPath + "/Racers/", "*.txt");

        for (int i = 0; i < selectedList.Count(); i++)
        {
            if (!File.Exists(Application.streamingAssetsPath + "/Racers/" + selectedList[i] + ".txt"))
            {
                UpdateList(selectedList[i]);
                i--;
            }
        }

        foreach(string file in filePaths)
        {           
            Button button = Instantiate(racerButtonPrefab, transform).GetComponent<Button>();

            List<string> fileLines = File.ReadAllLines(file).ToList();

            button.transform.GetChild(0).GetComponent<Text>().text = fileLines[1];

            if (selectedList.Contains(fileLines[1]))
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

                UpdateFile();
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

            UpdateFile();
            UpdateUI();

            Debug.Log("Removed");
            return true;
        }
    }

    private void UpdateFile()
    {
        string txtDocumentName = Application.streamingAssetsPath + "/RacerList.txt";

        if (selectedList.Count > 0)
            File.WriteAllText(txtDocumentName, selectedList[0] + "\n");
        else
            File.WriteAllText(txtDocumentName, "");


        for(int i = 1; i < selectedList.Count; i++)
        {
            File.AppendAllText(txtDocumentName, selectedList[i] + "\n");
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
