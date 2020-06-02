using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private Sprite newButtonImage;
    [SerializeField] private GameObject buttonLayout;

    public void SetFirstObjectOfEventSystem()
    {
        if (buttonGroup.activeInHierarchy)
            EventSystem.current.SetSelectedGameObject(buttonGroup.transform.GetChild(0).gameObject);
        else
            EventSystem.current.SetSelectedGameObject(null);


        buttonGroup.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = newButtonImage;
    }

    public void EnableButtonsJustBeforeTimerBegin()
    {
        foreach (Button _buttons in buttonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = true;
        }
    }

    public void DisableButtonsJustBeforeAQuestion()
    {
        foreach (Button _buttons in buttonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }
    }
}
