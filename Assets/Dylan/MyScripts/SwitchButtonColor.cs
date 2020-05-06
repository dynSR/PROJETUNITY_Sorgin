using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchButtonColor : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    private TextMeshProUGUI buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.faceColor = new Color32(255, 255, 255, 150);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        buttonText.faceColor = new Color32(255, 255, 255, 150);
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonText.faceColor = new Color32(255, 255, 255, 255);
    }
}
