using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Camera cam;

    public GameObject racerPrefab;

    public GameObject pauseCanvas;

    public class RacerPerformance
    {
        public string name;
        public int lane;
        public RacerProfile profile;
        public GameObject instance;
        public float finishTime = 0;
        public string placement = "";
        public int wins = 0;
        public int points = 0;
    }

    private RacerPerformance[] allRacers;

    public int numCurrentRacers = 0;

    public int numFinisedRacers = 0;

    private event System.Action Event_Go;
    private event System.Action Event_GetSet;

    private float timer = 0;

    public Text roundEndCountdown;
    private float roundEndCountDownTime = 10;

    private int roundNum = 0;

    public GameObject miniLeaderboard;

    public Transform allLeaderboards;
    public GameObject leaderboardPrefab;
    public GameObject leaderboardRowPrefab;
    public GameObject currentLeaderboard;
    private readonly float leaderboardViewTime = 10;

    public GameObject ghostPrefab;

    public bool showBars = false;

    void Start()
    {
        StartCoroutine(Phases());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    IEnumerator Phases()
    {
        LoadRacers();       

        while (numCurrentRacers > 1)
        {
            SetUpRound();

            yield return StartCoroutine(RoundStart());

            miniLeaderboard.SetActive(true);

            yield return StartCoroutine(RoundInProgress());

            yield return StartCoroutine(Eliminations());

            yield return new WaitForSeconds(1);

            Awards();

            BigLeaderboard();  // Create new Leaderboard

            yield return new WaitForSeconds(3);

            miniLeaderboard.SetActive(false);

            allLeaderboards.gameObject.SetActive(true);

            yield return currentLeaderboard.GetComponent<Leaderboard>().Timer(leaderboardViewTime); ;

            if (numCurrentRacers == 2)
                break;

            allLeaderboards.gameObject.SetActive(false);

            CleanUpRound();
        }

        FinishRace();

        yield return null;
    }

    private void LoadRacers()
    {
        if (GameObject.Find("RacerList"))
        {
            RacerProfile[] racersList = GameObject.Find("RacerList").GetComponent<RacerList>().racers.ToArray();

            allRacers = new RacerPerformance[racersList.Length];

            for (int i = 0; i < allRacers.Length; i++)
            {
                RacerPerformance racer = new RacerPerformance
                {
                    name = racersList[i].name,
                    lane = i,
                    profile = racersList[i],
                };

                allRacers[i] = racer;
            }
        }

        numCurrentRacers = allRacers.Length;
    }

    void SetUpRound()
    {

        for (int i = 0; i < numCurrentRacers; i++)
        {
            allRacers[i].instance = Instantiate(racerPrefab, transform.GetChild(0).GetChild(allRacers[i].lane).position, Quaternion.identity);

            allRacers[i].instance.AddComponent<GhostMaker>().ghostPrefab = ghostPrefab;

            StartCoroutine(allRacers[i].instance.GetComponent<LoadFromProfile>().Loading(allRacers[i].profile));

            if (showBars)
            {
                allRacers[i].instance.transform.Find("Hud").GetComponent<Racer_UI>().stamina_bar.gameObject.SetActive(true);
                allRacers[i].instance.transform.Find("Hud").GetComponent<Racer_UI>().speed_bar.gameObject.SetActive(true);
            }

            Event_Go += allRacers[i].instance.GetComponent<Racer_Script>().GO;
            Event_GetSet += allRacers[i].instance.GetComponent<Racer_Script>().GetSet;
        }

        if(allRacers[0].placement == "1st")
            allRacers[0].instance.GetComponent<Racer_Script>().crown.SetActive(true);

        cam.transform.position = new Vector3(0, 0, -10);

        miniLeaderboard.transform.Find("Names").GetComponent<Text>().text = "";
        miniLeaderboard.transform.Find("Times").GetComponent<Text>().text = "";

        timer = 0;
        roundEndCountDownTime = 10;
        roundEndCountdown.gameObject.SetActive(false);

        roundNum++;
    }

    public void PauseGame()
    {
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);

        Time.timeScale = !pauseCanvas.activeSelf ? 1 : 0;
    }

    IEnumerator RoundStart()
    {
        Debug.Log("Racers On Your Mark...");

        yield return new WaitForSeconds(1);

        Debug.Log("Get Set...");

        Event_GetSet?.Invoke();

        yield return new WaitForSeconds(1);

        Debug.Log("GO!");

        Event_Go?.Invoke();
    }

    IEnumerator RoundInProgress()
    {
        numFinisedRacers = 0;

        while (numFinisedRacers < numCurrentRacers)
        {
            timer += Time.deltaTime;

            miniLeaderboard.transform.Find("Timer").GetComponent<Text>().text = timer.ToString("F2");

            System.Array.Sort(allRacers, numFinisedRacers, numCurrentRacers - numFinisedRacers, new Comparer());
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(0, allRacers[numFinisedRacers].instance.transform.position.y, -10), Time.deltaTime * 5f);

            MiniLeaderboard();

            if (allRacers[numFinisedRacers].instance.transform.position.y <= -100)
            {
                allRacers[numFinisedRacers].instance.GetComponent<Racer_Script>().FinishedPhase();

                miniLeaderboard.transform.Find("Times").GetComponent<Text>().text += timer.ToString("F2") + "\n";
                allRacers[numFinisedRacers].finishTime = Mathf.Round(timer * 100f) / 100f;

                numFinisedRacers++;
            }

            if (numFinisedRacers > 0)
            {
                roundEndCountdown.gameObject.SetActive(true);
                roundEndCountDownTime -= Time.deltaTime;
                roundEndCountdown.text = roundEndCountDownTime.ToString("F0");

                if (roundEndCountDownTime <= 0)
                {
                    break;
                }
            }

            yield return null;
        }

        roundEndCountdown.text = "FINISH";
    }

    IEnumerator Eliminations()
    {
        if (numFinisedRacers == numCurrentRacers)     // All racers finished
        {
            float tieTime = allRacers[numCurrentRacers-1].finishTime;

            if (allRacers[0].finishTime != tieTime)                // Check if all racers tied
            {
                for (int i = 1; i < numFinisedRacers; i++)      // remove all racers tied for last
                {
                    if (allRacers[i].finishTime == tieTime)
                    {
                        numFinisedRacers--;
                    }
                }
            }
        }

        for (int i = numFinisedRacers; i < numCurrentRacers; i++)
        {
            while (cam.transform.position.y < allRacers[i].instance.transform.position.y)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(0, allRacers[i].instance.transform.position.y, -10), Time.deltaTime * 5f);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            Event_Go -= allRacers[i].instance.GetComponent<Racer_Script>().GO;
            Event_GetSet -= allRacers[i].instance.GetComponent<Racer_Script>().GetSet;

            allRacers[i].instance.GetComponent<Racer_Script>().Eliminate();
        }
    }

    void Awards()
    {
        int[] awardedPoints = { 30, 20, 10, 5 };
        string[] suffix = { "st", "nd", "rd", "th" };

        int p = 0;
        int r = 1;

        bool tie = false;


        if (numCurrentRacers > 1)
        {
            if (allRacers[0].finishTime != allRacers[1].finishTime)    // if no one tied for 1st
            {
                allRacers[0].wins++;
            }
        }
        else
        {
            allRacers[0].wins++;
        }


        for (int i = 0; i < numFinisedRacers; i++)
        {
            if (i == 0)
                allRacers[i].instance.GetComponent<Racer_Script>().crown.SetActive(true);
            else
                allRacers[i].instance.GetComponent<Racer_Script>().crown.SetActive(false);
        }


        for (int i = 0; i < numCurrentRacers; i++)
        { 

            if (i < numCurrentRacers)
            {
                allRacers[i].points += awardedPoints[p];
            }

            allRacers[i].placement = r.ToString() + suffix[p];                

            if (i + 1 < numCurrentRacers)
            {
                if (allRacers[i].finishTime != allRacers[i + 1].finishTime || i >= numCurrentRacers)
                {
                    r++;

                    if (tie)
                    {
                        r++;
                        tie = false;
                    }

                    if (p + 1 < awardedPoints.Length)
                    {
                        p++;
                    }
                }
                else
                {
                    tie = true;
                }
            }
        }
    }

    void CleanUpRound()
    {
        numCurrentRacers = numFinisedRacers;

        foreach (Transform ghost in transform.Find("Ghosts"))
            Destroy(ghost.gameObject);

        for (int i = 0; i < numCurrentRacers; i++)
        {
            Event_Go -= allRacers[i].instance.GetComponent<Racer_Script>().GO;
            Event_GetSet -= allRacers[i].instance.GetComponent<Racer_Script>().GetSet;

            Destroy(allRacers[i].instance);
        }
    }

    void FinishRace()
    {
        allLeaderboards.transform.Find("Nav").gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        gameObject.AddComponent<SceneSwitcher>().SwitchScene("RacerSelect_8");
    }

    void MiniLeaderboard()
    {
        string txt = "";
        string etxt = "";

        for (int i = 0; i < numCurrentRacers; i++)
        {
            txt += allRacers[i].name + "\n";
            etxt += "\n";
        }

        for(int i = numCurrentRacers; i < allRacers.Length; i++)
        {
            etxt += allRacers[i].name + "\n";
        }

        miniLeaderboard.transform.Find("Names").GetComponent<Text>().text = txt;
        miniLeaderboard.transform.Find("Eliminated Names").GetComponent<Text>().text = etxt;
    }

    public void BigLeaderboard()
    {
        Transform content = allLeaderboards.transform.Find("Viewport/Content").transform;

        currentLeaderboard = Instantiate(leaderboardPrefab, content);

        currentLeaderboard.transform.Find("Round").GetChild(0).GetComponent<Text>().text = "ROUND " + roundNum.ToString();

        for (int i = 0; i < allRacers.Length; i++)
        {
            GameObject leaderboardRow = Instantiate(leaderboardRowPrefab, currentLeaderboard.transform.Find("Board").transform);

            leaderboardRow.transform.Find("Position").GetChild(0).GetComponent<Text>().text = allRacers[i].placement;
            leaderboardRow.transform.Find("Name").GetChild(0).GetComponent<Text>().text = allRacers[i].name;


            if (i < numFinisedRacers)
            {
                leaderboardRow.transform.Find("Time").GetChild(0).GetComponent<Text>().text = allRacers[i].finishTime.ToString("F2");
            }
            else if (i < numCurrentRacers)
            {
                if(allRacers[i].finishTime == 0)
                    leaderboardRow.transform.Find("Time").GetChild(0).GetComponent<Text>().text = "DNF";
                else
                    leaderboardRow.transform.Find("Time").GetChild(0).GetComponent<Text>().text = allRacers[i].finishTime.ToString("F2");

                foreach (Transform child in leaderboardRow.transform)
                {
                    child.GetComponent<Image>().color = Color.red;
                }
            }
            else
            {
                leaderboardRow.transform.Find("Time").GetChild(0).GetComponent<Text>().text = "-";

                foreach (Transform child in leaderboardRow.transform)
                {
                    child.GetComponent<Image>().color = Color.grey;
                }
            }

            allRacers[i].finishTime = 0;
            leaderboardRow.transform.Find("Wins").GetChild(0).GetComponent<Text>().text = allRacers[i].wins.ToString();
            leaderboardRow.transform.Find("Points").GetChild(0).GetComponent<Text>().text = allRacers[i].points.ToString();
        }
    }

    public void BigLeaderboard(int input)
    {
        Transform content = allLeaderboards.transform.Find("Viewport/Content").transform;

        if (Mathf.Abs(input) == 1)
        {
            if (input == 1)
            {
                if (content.position.x > 834 * 2)
                    content.position -= new Vector3(834, 0, 0);
            }

            if (input == -1)
            {
                if (content.position.x < 834 * (roundNum))
                    content.position += new Vector3(834, 0, 0);
            }
        }
    }


    public class Comparer : IComparer
    {
        public int Compare(object x, object y)
        {
            RacerPerformance a = x as RacerPerformance;
            RacerPerformance b = y as RacerPerformance;

            float ay = a.instance.transform.position.y;
            float by = b.instance.transform.position.y;

            return ay.CompareTo(by);
        }
    }
}
