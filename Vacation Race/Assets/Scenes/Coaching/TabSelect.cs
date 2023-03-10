using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSelect : MonoBehaviour
{
    public void OpenTab()
    {
        transform.parent.parent.GetComponent<EditCanvas>().OpenTab(transform.parent);
    }
}
