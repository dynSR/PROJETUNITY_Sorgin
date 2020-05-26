using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private Sprite newButtonImage;

    public void SetFirstObjectOfEventSystem()
    {
        if (buttonGroup.activeInHierarchy)
            EventSystem.current.SetSelectedGameObject(buttonGroup.transform.GetChild(0).gameObject);
        else
            EventSystem.current.SetSelectedGameObject(null);


        buttonGroup.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = newButtonImage;
    }
}
