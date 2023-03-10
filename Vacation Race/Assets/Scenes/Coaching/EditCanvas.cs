using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditCanvas : MonoBehaviour
{
    public Transform currentTab;

    private void Start()
    {
        currentTab = transform.GetChild(0).transform;
    }

    public void OpenTab(Transform tab)
    {
        currentTab.transform.Find("Panel").gameObject.SetActive(false);
        currentTab.transform.Find("Button").GetComponent<Button>().interactable = true;

        currentTab = tab;
        currentTab.transform.Find("Panel").gameObject.SetActive(true);
        currentTab.transform.Find("Button").GetComponent<Button>().interactable = false;
    }
}
