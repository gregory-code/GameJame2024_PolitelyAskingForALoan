using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicOption : MonoBehaviour
{
    [SerializeField] AudioMixer MasterMixer;
    [SerializeField] string mixerName;
    [SerializeField] string playerPrefName;

    [SerializeField] Slider ownerSlider;

    [SerializeField] MuteButton muteButton;

    public void Start()
    {
        SetMusicLevel(PlayerPrefs.GetFloat(playerPrefName));
    }

    public void SetMusicLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat(playerPrefName, sliderValue);
        MasterMixer.SetFloat(mixerName, Mathf.Log10(sliderValue) * 20);
        ownerSlider.value = sliderValue;

        muteButton.CheckMute();
    }

    public float GetSliderLevel()
    {
        return ownerSlider.value;
    }
}
