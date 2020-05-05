using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [TextArea(1, 10)]
    [SerializeField] private string buttonTooltipText;
    [SerializeField] private TextMeshProUGUI buttonsTooltipText;

    public void OnDeselect(BaseEventData eventData)
    {
        buttonsTooltipText.text = null;
        buttonsTooltipText.transform.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonsTooltipText.text = buttonTooltipText;
        buttonsTooltipText.transform.gameObject.SetActive(true);
    }

}
