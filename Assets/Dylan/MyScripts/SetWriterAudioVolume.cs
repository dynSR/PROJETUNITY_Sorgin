using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWriterAudioVolume : MonoBehaviour
{
    private AudioSource attachedAudioSource;

    private void Start()
    {
        attachedAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.Pause)
        {
            //SetAttachedWriterAudioVolume(0);
            attachedAudioSource.Pause();
        }
        else
        {
            //SetAttachedWriterAudioVolume(1);
            attachedAudioSource.UnPause();
        }
    }

    public void SetAttachedWriterAudioVolume(int volumeValue)
    {
        attachedAudioSource.volume = volumeValue;
    }
}
