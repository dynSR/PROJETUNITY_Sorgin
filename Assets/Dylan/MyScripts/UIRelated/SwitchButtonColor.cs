using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchButtonColor : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public enum ButtonType { CannotBeSetAsLastSelected, CanBeSetAsLastSelected}
    public ButtonType buttonType;
    private TextMeshProUGUI buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.faceColor = new Color32(255, 255, 255, 150);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        buttonText.faceColor = new Color32(255, 255, 255, 150);

        //if (buttonType == ButtonType.CanBeSetAsLastSelected)
        //    DefaultUIManager.lastSelectedButton = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonText.faceColor = new Color32(255, 255, 255, 255);

        if (buttonType == ButtonType.CanBeSetAsLastSelected)
            DefaultUIManager.lastSelectedButton = this.gameObject;

        //Debug.Log(DefaultUIManager.lastSelectedButton.name);
    }
}
