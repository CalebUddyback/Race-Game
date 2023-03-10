using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour
{
    public Button currentButton;
    public string shaderProperty;

    public StyleSelect styleSelector;

    public void SelectColor(Button selectedColor)
    {
        if (currentButton)
            currentButton.interactable = true;

        currentButton = selectedColor;

        selectedColor.interactable = false;

        if(!styleSelector)
            GameObject.Find("Racer").transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor(shaderProperty, currentButton.colors.normalColor);
        else
            styleSelector.addonInstance.GetComponent<SpriteRenderer>().material.SetColor(shaderProperty, currentButton.colors.normalColor);
    }
}