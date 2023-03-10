using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private GameObject previousSceneObj;

    private void Start()
    {
        previousSceneObj = GameObject.Find("PreviousScreen");
    }

    public void SwitchScene(string targetScene)
    {
        if (previousSceneObj)
            previousSceneObj.GetComponent<PreviousScreen>().previousScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(targetScene);
    }

    public void PreviousScene()
    {
        if (previousSceneObj)
            SceneManager.LoadScene(previousSceneObj.GetComponent<PreviousScreen>().previousScene);
        else
            SceneManager.LoadScene("Main Menu");
    }
}
