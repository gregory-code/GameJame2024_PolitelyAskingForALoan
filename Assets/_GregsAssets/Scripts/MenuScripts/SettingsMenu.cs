using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : UIRenderer
{
    void Start()
    {
        SetStartingActive(false);
    }

    public void Credits()
    {
        SceneManager.LoadScene("creditsScene");
    }
}
