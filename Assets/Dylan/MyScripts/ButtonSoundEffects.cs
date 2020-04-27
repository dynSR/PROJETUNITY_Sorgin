using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffects : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    [Header("WWISE EVENT SOUND NAME")]
    [SerializeField] private string selectionWwiseEventSoundName;
    [SerializeField] private string submitWwiseEventSoundName;

    public void OnSelect(BaseEventData eventData)
    {
        if (!string.IsNullOrEmpty(selectionWwiseEventSoundName))
        {
            AkSoundEngine.PostEvent(selectionWwiseEventSoundName, this.gameObject);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!string.IsNullOrEmpty(submitWwiseEventSoundName))
        {
            AkSoundEngine.PostEvent(submitWwiseEventSoundName, this.gameObject);
        }
            
    }
}
