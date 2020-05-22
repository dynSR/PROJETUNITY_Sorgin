﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffects : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler
{
    [Header("WWISE EVENT SOUND NAME")]
    [SerializeField] private string selectionWwiseEventSoundName;
    [SerializeField] private string submitWwiseEventSoundName;

    public bool isSelected = true;

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (isSelected) return;

        else if (!string.IsNullOrEmpty(selectionWwiseEventSoundName))
        {
            AkSoundEngine.PostEvent(selectionWwiseEventSoundName, this.gameObject);
            isSelected = true;
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        isSelected = true;

        if (!string.IsNullOrEmpty(submitWwiseEventSoundName))
        {
            AkSoundEngine.PostEvent(submitWwiseEventSoundName, this.gameObject);
        }
    }
}