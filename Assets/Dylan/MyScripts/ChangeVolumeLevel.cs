using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeLevel : MonoBehaviour
{
    private Slider thisSlider;

    private int globalVolume;
    private int musicVolume;
    private int musicEffectsVolume;

    [SerializeField] private TextMeshProUGUI volumeValueText;


    // Start is called before the first frame update
    void Start()
    {
        thisSlider = GetComponent<Slider>();

        LoadVolumeLevels();
    }

    public void SetSpecificVolume(string whatValueToSet)
    {
        if (whatValueToSet == "GlobalMusicVolume")
        {
            globalVolume = (int)thisSlider.value;
            AkSoundEngine.SetRTPCValue("GlobalMusicVolume", globalVolume);
            PlayerPrefs.SetInt("GlobalMusicVolume", globalVolume);
            SetSpecificVolumeValueText(volumeValueText, globalVolume);
        }
        if (whatValueToSet == "MusicVolume")
        {
            musicVolume = (int)thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
            PlayerPrefs.SetInt("MusicVolume", musicVolume);
            SetSpecificVolumeValueText(volumeValueText, musicVolume);
        }
        if (whatValueToSet == "MusicEffects")
        {
            musicEffectsVolume = (int)thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicEffects", musicEffectsVolume);
            PlayerPrefs.SetInt("MusicEffects", musicEffectsVolume);
            SetSpecificVolumeValueText(volumeValueText, musicEffectsVolume);
        }
    }
    
    void LoadVolumeLevels()
    {
        if (PlayerPrefs.HasKey("GlobalMusicVolume"))
            globalVolume = PlayerPrefs.GetInt("GlobalMusicVolume");
        else
            globalVolume = 100;
        //

        if (PlayerPrefs.HasKey("MusicVolume"))
            musicEffectsVolume = PlayerPrefs.GetInt("MusicVolume");
        else
            musicEffectsVolume = 100;

        //

        if (PlayerPrefs.HasKey("MusicEffects"))
            musicEffectsVolume = PlayerPrefs.GetInt("MusicEffects");
        else
            musicEffectsVolume = 100;
    }

    private void SetSpecificVolumeValueText(TextMeshProUGUI valueTextToSet, int value)
    {
        int actualValue = value * 10;
        valueTextToSet.text = actualValue.ToString();
    }

}
