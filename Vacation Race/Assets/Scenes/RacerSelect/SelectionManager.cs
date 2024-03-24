using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionManager : MonoBehaviour
{
    readonly string _FILEPATH = "Racer Profiles/";

    public GameObject racerListPrefab;
    public GameObject racerButtonPrefab;

    public int MAX_RACERS = 8;
    public List<RacerProfile> selectedList = new List<RacerProfile>();

    public Transform listUIContainer;

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
            print("RacerList Found!");

            racerList = GameObject.Find("RacerList").GetComponent<RacerList>();

            if (racerList.maxRacers != MAX_RACERS)
                racerList.racers.Clear();
        }
        else
        {
            racerList = Instantiate(racerListPrefab, null).GetComponent<RacerList>();
            racerList.gameObject.name = "RacerList";
            racerList.maxRacers = MAX_RACERS;
        }

        Object[] profiles = Resources.LoadAll(_FILEPATH);

        foreach (Object racerAsset in profiles)
        {
            if(((RacerProfile)Resources.Load(_FILEPATH + racerAsset.name, typeof(RacerProfile)))._name == "")
            {
                continue;
            }

            Button button = Instantiate(racerButtonPrefab, transform).GetComponent<Button>();       // create button 

            RacerProfile racer_profile =  button.GetComponent<RacerSelect>().racer_profile = (RacerProfile)Resources.Load(_FILEPATH + racerAsset.name, typeof(RacerProfile));

            button.transform.GetChild(0).GetComponent<Text>().text = racer_profile._name;                  // set button text to racer name

            if (racerList.racers.Contains(racer_profile))                                                // if racer already in selected list
                button.GetComponent<RacerSelect>().Select();
        }


        racerList.racers = selectedList;
    }

    public bool UpdateList(RacerProfile racer)
    {

        if (!selectedList.Contains(racer))
        {
            if (selectedList.Count < MAX_RACERS)
            {
                selectedList.Add(racer);


                if (selectedList.Count > 0)
                    continueButton.interactable = true;

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
            selectedList.Remove(racer);

            Debug.Log("Removed");


            if (selectedList.Count == 0)
                continueButton.interactable = false;

            return false;
        }
    }
}
