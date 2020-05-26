using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FungusButtonHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Sprite selectedImage;
    [SerializeField] private Sprite deselectedImage;

    void Start()
    {
        if(gameObject != EventSystem.current.currentSelectedGameObject)
            GetComponent<Button>().image.sprite = deselectedImage;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (deselectedImage != null)
            GetComponent<Button>().image.sprite = deselectedImage;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (selectedImage != null)
            GetComponent<Button>().image.sprite = selectedImage;
    }
}
