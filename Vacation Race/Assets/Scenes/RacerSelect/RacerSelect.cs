using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacerSelect : MonoBehaviour
{
    private Button button;
    public bool selected = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Click()
    {
        if (transform.parent.GetComponent<SelectionManager>().UpdateList(transform.GetChild(0).GetComponent<Text>().text))
        {
            selected = !selected;

            UpdateColor(selected);

        }
    }

    public void UpdateColor(bool check)
    {
        var col = button.colors;

        if (check)
        {
            col.normalColor = Color.green;
            col.selectedColor = Color.green;
        }
        else
        {
            col.normalColor = Color.white;
            col.selectedColor = Color.white;
        }

        button.colors = col;
    }
}
