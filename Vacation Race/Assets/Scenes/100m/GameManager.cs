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
    private RacerPerformance[] currentRacers;
    private int numFinishedRacers = 0;

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
        LoadRacers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
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

            currentRacers = allRacers;

            SetUpRound();
        }
    }

    void SetUpRound()
    {
        foreach (RacerPerformance racer in currentRacers)
        {
            racer.instance = Instantiate(racerPrefab, transform.GetChild(0).GetChild(racer.lane).position, Quaternion.identity);

            racer.instance.AddComponent<GhostMaker>().ghostPrefab = ghostPrefab;

            StartCoroutine(racer.instance.GetComponent<LoadFromProfile>().Loading(racer.profile));

            if (showBars)
            {
                racer.instance.transform.Find("Hud").GetComponent<Racer_UI>().stamina_bar.gameObject.SetActive(true);
                racer.instance.transform.Find("Hud").GetComponent<Racer_UI>().speed_bar.gameObject.SetActive(true);
            }

            Event_Go += racer.instance.GetComponent<Racer_Script>().GO;
            Event_GetSet += racer.instance.GetComponent<Racer_Script>().GetSet;
        }

        cam.transform.position = new Vector3(0, 0, -10);

        miniLeaderboard.transform.Find("Names").GetComponent<Text>().text = "";
        miniLeaderboard.transform.Find("Times").GetComponent<Text>().text = "";

        timer = 0;
        roundEndCountDownTime = 10;
        roundEndCountdown.gameObject.SetActive(false);

        roundNum++;

        StartCoroutine(RacePhases());
    }

    public void PauseGame()
    {
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);

        Time.timeScale = !pauseCanvas.activeSelf ? 1 : 0;
    }

    IEnumerator RacePhases()
    {
        yield return StartCoroutine(RoundStart());

        yield return StartCoroutine(RoundInProgress());

        yield return StartCoroutine(RoundFinish());

        if (currentRacers.Length > 1)
        {
            CleanUpRound();
            SetUpRound();
        }
        else
            FinishRace();
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

        miniLeaderboard.SetActive(true);
    }

    IEnumerator RoundInProgress()
    {
        while (numFinishedRacers < currentRacers.Length)
        {
            timer += Time.deltaTime;

            System.Array.Sort(currentRacers, PositionComparison);
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(0, currentRacers[numFinishedRacers].instance.transform.position.y, -10), Time.deltaTime * 5f);

            MiniLeaderboard();

            if (numFinishedRacers > 0)
            {
                roundEndCountdown.gameObject.SetActive(true);
                roundEndCountDownTime -= Time.deltaTime;
                roundEndCountdown.text = roundEndCountDownTime.ToString("F0");

                if (roundEndCountDownTime <= 0)
                {
                    break;
                }
            }

            if (currentRacers[numFinishedRacers].instance.transform.position.y <= -100)
            {
                currentRacers[numFinishedRacers].instance.GetComponent<Racer_Script>().FinishedPhase();

                miniLeaderboard.transform.Find("Times").GetComponent<Text>().text += timer.ToString("F2") + "\n";
                currentRacers[numFinishedRacers].finishTime = Mathf.Round(timer * 100f) / 100f;

                numFinishedRacers++;
            }

            yield return null;
        }
    }

    IEnumerator RoundFinish()
    {
        roundEndCountdown.text = "FINISH";

        Awards();

        if (numFinishedRacers == currentRacers.Length)     // All racers finished
        {
            float tieTime = currentRacers[currentRacers.Length -1].finishTime;

            if (currentRacers[0].finishTime != tieTime)                // Check if all racers tied
            {
                for (int i = 1; i < currentRacers.Length; i++)      // eliminate all racers tied with last
                {
                    if (currentRacers[i].finishTime == tieTime)
                    {
                        numFinishedRacers--;
                    }
                }
            }
        }

        for (int i = numFinishedRacers; i < currentRacers.Length; i++)
        {
            while (cam.transform.position.y < currentRacers[i].instance.transform.position.y)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(0, currentRacers[i].instance.transform.position.y, -10), Time.deltaTime * 5f);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            Event_Go -= currentRacers[i].instance.GetComponent<Racer_Script>().GO;
            Event_GetSet -= currentRacers[i].instance.GetComponent<Racer_Script>().GetSet;

            currentRacers[i].instance.GetComponent<Racer_Script>().Eliminate();
        }

        BigLeaderboard(0);

        RacerPerformance[] tempArray = currentRacers;

        currentRacers = new RacerPerformance[numFinishedRacers];

        for(int i = 0; i< currentRacers.Length; i++)
        {
            currentRacers[i] = tempArray[i];
        }

        yield return new WaitForSeconds(3);

        miniLeaderboard.SetActive(false);

        allLeaderboards.gameObject.SetActive(true);

        currentLeaderboard.GetComponent<Leaderboard>().StartTimer(leaderboardViewTime);

        yield return new WaitForSeconds(leaderboardViewTime);
    }

    void Awards()
    {
        int[] awardedPoints = { 30, 20, 10, 5 };
        string[] rank = { "st", "nd", "rd", "th" };

        int p = 0;
        int r = 1;

        bool tie = false;


        if (numFinishedRacers > 1)
        {
            if (currentRacers[0].finishTime != currentRacers[1].finishTime)    // if no one tied for 1st
            {
                currentRacers[0].wins++;
            }
        }
        else
        {
            currentRacers[0].wins++;
        }



        for (int i = 0; i < currentRacers.Length; i++)
        {
            if (i < numFinishedRacers)
            {
                currentRacers[i].points += awardedPoints[p];
            }

            currentRacers[i].placement = r.ToString() + rank[p];

            if (i + 1 < currentRacers.Length)
            {
                if (currentRacers[i].finishTime != currentRacers[i + 1].finishTime || i >= numFinishedRacers)
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
        allLeaderboards.gameObject.SetActive(false);

        foreach (Transform ghost in transform.Find("Ghosts"))
            Destroy(ghost.gameObject);

        for (int i = 0; i < currentRacers.Length; i++)
        {
            Event_Go -= currentRacers[i].instance.GetComponent<Racer_Script>().GO;
            Event_GetSet -= currentRacers[i].instance.GetComponent<Racer_Script>().GetSet;

            Destroy(currentRacers[i].instance);
        }

        numFinishedRacers = 0;
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

        for (int i = 0; i < currentRacers.Length; i++)
        {
            txt += currentRacers[i].name + "\n";
            etxt += "\n";
        }

        for(int i = currentRacers.Length; i < allRacers.Length; i++)
        {
            etxt += allRacers[i].name + "\n";
        }

        miniLeaderboard.transform.Find("Names").GetComponent<Text>().text = txt;
        miniLeaderboard.transform.Find("Eliminated Names").GetComponent<Text>().text = etxt;
    }

    public void BigLeaderboard(int input)
    {
        Transform content = allLeaderboards.transform.Find("Viewport/Content").transform;

        for(int i = 0; i < currentRacers.Length; i++)
        {
            allRacers[i] = currentRacers[i];
        }

        if (input == 0)
        {
            currentLeaderboard = Instantiate(leaderboardPrefab, content);

            currentLeaderboard.transform.Find("Round").GetChild(0).GetComponent<Text>().text = "ROUND " + roundNum.ToString();

            for (int i = 0; i < allRacers.Length; i++)
            {
                GameObject leaderboardRow = Instantiate(leaderboardRowPrefab, currentLeaderboard.transform.Find("Board").transform);

                if(i >= numFinishedRacers)
                {
                    foreach(Transform child in leaderboardRow.transform)
                    {
                        child.GetComponent<Image>().color = Color.red;
                    }
                }

                leaderboardRow.transform.Find("Position").GetChild(0).GetComponent<Text>().text = allRacers[i].placement;
                leaderboardRow.transform.Find("Name").GetChild(0).GetComponent<Text>().text = allRacers[i].name;

                if (allRacers[i].finishTime == 0)
                {
                    if (currentRacers.Contains(allRacers[i]))
                    {
                        leaderboardRow.transform.Find("Time").GetChild(0).GetComponent<Text>().text = "DNF";
                    }
                    else
                    {
                        leaderboardRow.transform.Find("Time").GetChild(0).GetComponent<Text>().text = "-";

                        foreach (Transform child in leaderboardRow.transform)
                        {
                            child.GetComponent<Image>().color = Color.grey;
                        }
                    }
                }
                else
                {
                    leaderboardRow.transform.Find("Time").GetChild(0).GetComponent<Text>().text = allRacers[i].finishTime.ToString("F2");
                }
                allRacers[i].finishTime = 0;
                leaderboardRow.transform.Find("Wins").GetChild(0).GetComponent<Text>().text = allRacers[i].wins.ToString();
                leaderboardRow.transform.Find("Points").GetChild(0).GetComponent<Text>().text = allRacers[i].points.ToString();
            }
        }

        if(Mathf.Abs(input) == 1)
        {
            if(input == 1)
            {
                if (content.position.x > 834 * 2)
                    content.position -= new Vector3(834, 0, 0);
            }
            
            if(input == -1)
            {
                if(content.position.x < 834 * (roundNum))
                    content.position += new Vector3(834, 0, 0);
            }
        }
    }

    int PositionComparison(RacerPerformance a, RacerPerformance b)
    {
        if (a.finishTime == 0)
        {
            if (b.finishTime == 0)
            {
                float ay = a.instance.transform.position.y;
                float by = b.instance.transform.position.y;

                return ay.CompareTo(by);
            }
            else
            {
                return 1;
            }
        }

        if (b.finishTime == 0)
            return -1;

        float at = a.finishTime;
        float bt = b.finishTime;

        return at.CompareTo(bt);

        /*  OLD CODE
        if (a.instance == null) return (b.instance == null) ? 0 : 1;
        if (b.instance == null) return -1;

        float ay = a.instance.transform.position.y;
        float by = b.instance.transform.position.y;

        return ay.CompareTo(by);
        */
    }

}
