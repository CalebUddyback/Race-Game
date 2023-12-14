using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Racer_UI : MonoBehaviour
{
    public Text racer_name;

    public RectTransform upgradeRect;

    public Text upgradeText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Upgrade(RacerProfile.Stat stat)
    {
        string txt = stat.ToString();

        upgradeText.text = txt + "+1";
        upgradeRect.GetComponent<Animation>().Play();

        yield return new WaitUntil(() => !upgradeRect.GetComponent<Animation>().isPlaying);
    }
}
