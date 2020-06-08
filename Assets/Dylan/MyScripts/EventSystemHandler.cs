using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] private Sprite newButtonImage;
    [SerializeField] private GameObject buttonLayout;
    [SerializeField] private GameObject menuDialog;

    public void ForceToDeactivate()
    {
        menuDialog.SetActive(false);
        buttonLayout.transform.GetChild(0).GetComponent<ButtonSoundEffects>().isSelected = true;
    }

    public void SetFirstObjectOfEventSystem()
    {
        EventSystem.current.SetSelectedGameObject(buttonLayout.transform.GetChild(0).gameObject);

        buttonLayout.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = newButtonImage;
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
