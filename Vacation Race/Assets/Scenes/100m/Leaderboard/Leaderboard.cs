using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public Slider bar;

    public IEnumerator Timer(float time)
    {
        float currentValue = 1;
        float t = 0;

        while(bar.value > 0)
        {
            bar.value = currentValue;

            t += Time.deltaTime / time;
            currentValue = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        yield return null;
    }
}
