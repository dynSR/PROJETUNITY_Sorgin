using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchButtonColor : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public enum ButtonType { CannotBeSetAsLastSelected, CanBeSetAsLastSelected, MainMenuButton }
    public ButtonType buttonType;
    private TextMeshProUGUI buttonTextMeshProUGUI;
    private Text _buttonText;

    private void Start()
    {
        buttonTextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        _buttonText = GetComponentInChildren<Text>();

        if (buttonTextMeshProUGUI != null && buttonType != ButtonType.MainMenuButton )
            buttonTextMeshProUGUI.faceColor = new Color32(255, 255, 255, 50);

        if (_buttonText != null && buttonType != ButtonType.MainMenuButton )
            _buttonText.color = new Color(255, 255, 255, 50);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (buttonTextMeshProUGUI != null)
            buttonTextMeshProUGUI.faceColor = new Color32(255, 255, 255, 50);

        if (_buttonText != null)
            _buttonText.color = new Color(255, 255, 255, 50);

        if (_buttonText != null)
            _buttonText.color = new Color(255, 255, 255, 255);

        //if (buttonType == ButtonType.CanBeSetAsLastSelected)
        //    DefaultUIManager.lastSelectedButton = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(buttonTextMeshProUGUI != null)
            buttonTextMeshProUGUI.faceColor = new Color32(255, 255, 255, 255);

        if (_buttonText != null)
            _buttonText.color = new Color(255, 255, 255, 255);

        if (buttonType == ButtonType.CanBeSetAsLastSelected)
            DefaultUIManager.lastSelectedButton = this.gameObject;

        //Debug.Log(DefaultUIManager.lastSelectedButton.name);
    }
}
