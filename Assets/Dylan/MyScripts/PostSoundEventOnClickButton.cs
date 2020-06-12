using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostSoundEventOnClickButton : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event wwiseSoundEvent;

    public void PostAnEvent()
    {
        wwiseSoundEvent.Post(gameObject);
    }
}
