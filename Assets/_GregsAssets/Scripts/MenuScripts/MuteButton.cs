using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    [SerializeField] GameObject openMic;
    [SerializeField] GameObject mutedMic;

    [SerializeField] MusicOption[] musicOptions;

    bool isMuted;

    public void ClickMute()
    {
        foreach (MusicOption slider in musicOptions)
        {
            if (isMuted)
            {
                slider.SetMusicLevel(1);
                isMuted = true;
            }
            else
            {
                slider.SetMusicLevel(0.0001f);
                isMuted = false;
            }
        }

        CheckMute();
    }

    public void CheckMute()
    {
        foreach (MusicOption slider in musicOptions)
        {
            if (slider.GetSliderLevel() > 0.0001f)
            {
                SetMute(false);
                return;
            }
        }

        SetMute(true);
    }

    private void SetMute(bool state)
    {
        isMuted = state;
        openMic.SetActive(!state);
        mutedMic.SetActive(state);
    }
}
