using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FungusButtonHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Sprite selectedImage;
    [SerializeField] private Sprite notSelectedImage;

    void Start()
    {
        if(gameObject != EventSystem.current.currentSelectedGameObject)
            GetComponent<Button>().image.sprite = notSelectedImage;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (notSelectedImage != null)
            GetComponent<Button>().image.sprite = notSelectedImage;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (selectedImage != null)
            GetComponent<Button>().image.sprite = selectedImage;
    }
}
