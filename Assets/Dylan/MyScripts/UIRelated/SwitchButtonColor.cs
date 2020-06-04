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
    private TextMeshProUGUI[] childrenButtonTextMeshProUGUI;
    private Text _childrenButtonText;

    void Awake()
    {
        childrenButtonTextMeshProUGUI = GetComponentsInChildren<TextMeshProUGUI>();
        _childrenButtonText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        if (childrenButtonTextMeshProUGUI != null && buttonType != ButtonType.MainMenuButton )
        {
            foreach (TextMeshProUGUI childrenText in childrenButtonTextMeshProUGUI)
            {
                childrenText.faceColor = new Color32(255, 255, 255, 50);
            }
        }

        if (_childrenButtonText != null && buttonType != ButtonType.MainMenuButton )
            _childrenButtonText.color = new Color(255, 255, 255, 50);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (childrenButtonTextMeshProUGUI != null)
        {
            foreach (TextMeshProUGUI childrenText in childrenButtonTextMeshProUGUI)
            {
                childrenText.faceColor = new Color32(255, 255, 255, 50);
            }
        }

        if (_childrenButtonText != null)
            _childrenButtonText.color = new Color(255, 255, 255, 50);

        if (_childrenButtonText != null)
            _childrenButtonText.color = new Color(255, 255, 255, 255);

        //if (buttonType == ButtonType.CanBeSetAsLastSelected)
        //    DefaultUIManager.lastSelectedButton = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(childrenButtonTextMeshProUGUI != null)
        {
            foreach (TextMeshProUGUI childrenText in childrenButtonTextMeshProUGUI)
            {
                Debug.Log("Has Been Selected and texts has been put in white");
                childrenText.faceColor = new Color32(255, 255, 255, 255);
            }
        }
        
        if (_childrenButtonText != null)
            _childrenButtonText.color = new Color(255, 255, 255, 255);

        if (buttonType == ButtonType.CanBeSetAsLastSelected)
            DefaultUIManager.lastSelectedButton = this.gameObject;

        //Debug.Log(DefaultUIManager.lastSelectedButton.name);
    }
}
