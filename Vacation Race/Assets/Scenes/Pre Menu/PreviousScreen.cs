using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousScreen : MonoBehaviour
{
    public string previousScene;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        previousScene = SceneManager.GetActiveScene().name;
    }
}
