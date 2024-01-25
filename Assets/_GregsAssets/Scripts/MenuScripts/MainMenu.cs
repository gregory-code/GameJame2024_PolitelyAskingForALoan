using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : UIRenderer
{
    [SerializeField] private SettingsMenu settinigsMenu;

    private void Start()
    {
        SetStartingActive(true);
    }

    public void StartGame()
    {
        GameObject.FindFirstObjectByType<loadingScreen>().StartLoadScene("testScene");
    }

    public void ChangeSettings()
    {
        SetUIStatus(!isActive());
        settinigsMenu.SetUIStatus(!settinigsMenu.isActive());
    }
}
