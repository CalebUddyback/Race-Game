using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class GameManager1 : MonoBehaviour
{
    public Camera cam;

    public GameObject racerPrefab;

    private string[] racers_List;
    public GameObject[] racers_Instances;

    public GameObject miniLeaderboard;

    public Text roundEndCountdown;

    public Transform leaderboard;
    public GameObject racerRowPrefab;

    private float timer = 0;

    public List<GameObject> finalPlacements = new List<GameObject>();

    public bool showBars = false;

    private event System.Action Event_OnYourMark;
    private event System.Action Event_GetSet;

    private Vector3 velocity;

    public class Racer
    {
        string name;
        GameObject instance;
        int points;
    }

    void Start()
    {
        if (File.Exists(Application.streamingAssetsPath + "/RacerList.txt"))
        {
            string readFromFile = Application.streamingAssetsPath + "/RacerList.txt";

            racers_List = File.ReadAllLines(readFromFile).ToArray();

            racers_Instances = new GameObject[racers_List.Length];

            for (int i = 0; i < racers_Instances.Length; i++)
            {
                racers_Instances[i] = Instantiate(racerPrefab, transform.GetChild(0).GetChild(i).position, Quaternion.identity);
                racers_Instances[i].name = racers_List[i];

                if (showBars)
                {
                    racers_Instances[i].transform.Find("Hud").GetComponent<Racer_UI>().stamina_bar.gameObject.SetActive(true);
                    racers_Instances[i].transform.Find("Hud").GetComponent<Racer_UI>().speed_bar.gameObject.SetActive(true);
                }

                LoadRacer(racers_Instances[i]);
            }

            cam.transform.position = new Vector3(0, 0, -10);

            miniLeaderboard.transform.Find("Names").GetComponent<Text>().text = "";
            miniLeaderboard.transform.Find("Times").GetComponent<Text>().text = "";

            StartCoroutine(RacersSet());
        }
    }

    public void SetUpRace(string[] newList)
    {
        racers_Instances = new GameObject[newList.Length];

        for (int i = 0; i < newList.Count(); i++)
        {
            racers_Instances[i] = Instantiate(racerPrefab, transform.GetChild(0).GetChild(i).position, Quaternion.identity);
            racers_Instances[i].name = newList[i];

            if (showBars)
            {
                racers_Instances[i].transform.Find("Hud").GetComponent<Racer_UI>().stamina_bar.gameObject.SetActive(true);
                racers_Instances[i].transform.Find("Hud").GetComponent<Racer_UI>().speed_bar.gameObject.SetActive(true);
            }

            LoadRacer(racers_Instances[i]);
        }

        cam.transform.position = new Vector3(0, 0, -10);

        miniLeaderboard.transform.Find("Names").GetComponent<Text>().text = "";
        miniLeaderboard.transform.Find("Times").GetComponent<Text>().text = "";

        timer = 0;
        roundEndCountdown.gameObject.SetActive(false);

        StartCoroutine(RacersSet());

    }

    private void LoadRacer(GameObject loadingRacer)
    {
        //Load Racer Profile

        string readFromFilePath = Application.streamingAssetsPath + "/Racers/" + loadingRacer.name + ".txt";

        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        Event_OnYourMark += loadingRacer.GetComponent<Racer_Script>().GO;
        Event_GetSet += loadingRacer.GetComponent<Racer_Script>().GetSet;

        //Stats

        loadingRacer.transform.Find("Hud").GetComponent<Racer_UI>().racer_name.text = fileLines[1];
        loadingRacer.gameObject.name = fileLines[1];

        loadingRacer.GetComponent<Stats_Script>().start_reaction += int.Parse(fileLines[5]);
        loadingRacer.GetComponent<Stats_Script>().acceleration += int.Parse(fileLines[7]);
        loadingRacer.GetComponent<Stats_Script>().top_speed += int.Parse(fileLines[9]);
        loadingRacer.GetComponent<Stats_Script>().stamina += int.Parse(fileLines[11]);

        //Cosmetics

        //Skin
        loadingRacer.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_SkinColor", new Color(float.Parse(fileLines[13]), float.Parse(fileLines[14]), float.Parse(fileLines[15])));

        if (int.Parse(fileLines[17]) != 0)
        {
            Object[] allStyles = Resources.LoadAll("Head/");
            Instantiate(allStyles[int.Parse(fileLines[17]) - 1] as GameObject, loadingRacer.transform.Find("Sprite"));
        }

        if (int.Parse(fileLines[19]) != 0)
        {
            Object[] allStyles = Resources.LoadAll("Face/");
            Instantiate(allStyles[int.Parse(fileLines[19]) - 1] as GameObject, loadingRacer.transform.Find("Sprite"));
        }

        // Leaderboard
        GameObject leaderboardRow = Instantiate(racerRowPrefab, leaderboard.GetChild(0).transform);

        leaderboardRow.transform.Find("Name").GetChild(0).GetComponent<Text>().text = loadingRacer.name;

    }

    public IEnumerator RacersSet()
    {
        Debug.Log("Racers On Your Mark...");

        yield return new WaitForSeconds(1);

        Debug.Log("Get Set...");

        Event_GetSet?.Invoke();

        yield return new WaitForSeconds(1);

        Debug.Log("GO!");

        Event_OnYourMark?.Invoke();

        StartCoroutine(RacerPositions());

        miniLeaderboard.SetActive(true);
    }

    IEnumerator RacerPositions()
    {
        int i = 0;
        float countDownTimer = 10;

        while (i < racers_Instances.Length)
        {
            System.Array.Sort(racers_Instances, YPositionComparison);
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(0,racers_Instances[i].transform.position.y, -10), Time.deltaTime * 5f);

            MiniLeaderboard();

            if (finalPlacements.Count > 0)
            { 
                roundEndCountdown.gameObject.SetActive(true);
                countDownTimer -= Time.deltaTime;
                roundEndCountdown.text = countDownTimer.ToString("F0");

                if(countDownTimer <= 0)
                {
                    FinishRound();
                    break;
                }
            }

            if (racers_Instances[i].transform.position.y <= -100)
            {
                finalPlacements.Add(racers_Instances[i]);
                racers_Instances[i].GetComponent<Racer_Script>().FinishedPhase();
                Event_OnYourMark -= racers_Instances[i].GetComponent<Racer_Script>().GO;
                Event_GetSet -= racers_Instances[i].GetComponent<Racer_Script>().GetSet;

                miniLeaderboard.transform.Find("Times").GetComponent<Text>().text += timer.ToString("F2") + "\n";
            
                i++;
            }

            yield return null;
        }

        StartCoroutine(FinishRound());
    }

    public IEnumerator FinishRound()
    {
        roundEndCountdown.text = "FINISH";

        string[] newList;

        if (finalPlacements.Count == racers_Instances.Length)    //Every Racer finished
        {
            newList = new string[racers_Instances.Length - 1];
        }
        else
        {
            newList = new string[finalPlacements.Count];
        }

        for (int i = newList.Length; i < racers_Instances.Length; i++)
        {
            while (cam.transform.position.y < racers_Instances[i].transform.position.y)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(0, racers_Instances[i].transform.position.y, -10), Time.deltaTime * 5f);
                yield return null;
            }

            Event_OnYourMark -= racers_Instances[i].GetComponent<Racer_Script>().GO;
            Event_GetSet -= racers_Instances[i].GetComponent<Racer_Script>().GetSet;

            yield return new WaitForSeconds(1.5f);

            racers_Instances[i].GetComponent<Racer_Script>().Eliminate();
        }


        yield return new WaitForSeconds(3);

        miniLeaderboard.SetActive(false);

        leaderboard.gameObject.SetActive(true);

        yield return new WaitForSeconds(10);

        leaderboard.gameObject.SetActive(false);

        if (newList.Length > 1)
        {
            for (int i = 0; i < newList.Length; i++)
            {
                newList[i] = racers_Instances[i].name;
            }

            for (int i = 0; i< newList.Length; i++)
            {
                Event_OnYourMark -= racers_Instances[i].GetComponent<Racer_Script>().GO;
                Event_GetSet -= racers_Instances[i].GetComponent<Racer_Script>().GetSet;

                Destroy(racers_Instances[i]);
            }

            finalPlacements.Clear();

            SetUpRace(newList);
        }
        else
            FinishRace();
    }  

    public void FinishRace()
    {
        //Finale Screen
    }

    private int YPositionComparison(GameObject a, GameObject b)
    {
        if (a == null) return (b == null) ? 0 : 1;
        if (b == null) return -1;

        var ya = a.transform.position.y;
        var yb = b.transform.position.y;
        return ya.CompareTo(yb);
    }

    public void MiniLeaderboard()
    {
        timer += Time.deltaTime;

        string txt = "";

        for (int i = 0; i < finalPlacements.Count; i++)
        {
            txt += finalPlacements[i].name + "\n";
        }

        for (int i = finalPlacements.Count; i < racers_Instances.Length; i++)
        {
                txt += racers_Instances[i].name + "\n";
        }

        miniLeaderboard.transform.Find("Names").GetComponent<Text>().text = txt;
    }
}
