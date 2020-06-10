using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeLevel : MonoBehaviour
{
    public Slider thisSlider;
    public int musicVolume = 100;
    public int musicEffectsVolume = 100;


    // Start is called before the first frame update
    void Start()
    {
        thisSlider = GetComponent<Slider>();
        musicVolume = PlayerPrefs.GetInt("MusicVolume");
        musicEffectsVolume = PlayerPrefs.GetInt("MusicEffects");

    }

    public void SetSpecificVolume(string whatValueToSet)
    {
        //float sliderValue = thisScrollBar.value;

        if (whatValueToSet == "MusicVolume")
        {
            musicVolume = (int)thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
            PlayerPrefs.SetInt("MusicVolume", musicVolume);
        }
        if (whatValueToSet == "MusicEffects")
        {
            musicEffectsVolume = (int)thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicEffects", musicEffectsVolume);
            PlayerPrefs.SetInt("MusicEffects", musicEffectsVolume);
        }
    }
}
