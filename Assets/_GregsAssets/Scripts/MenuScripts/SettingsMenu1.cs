using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu1 : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] CanvasGroup settingsGroup;

    private bool bInSettings;

    public void Start()
    {
        player.onSettings += Settings;
    }

    private void Settings(bool state)
    {
        Time.timeScale = (state) ? 0f : 1f;
        bInSettings = state;
        settingsGroup.blocksRaycasts = state;
        settingsGroup.interactable = state;
        Cursor.lockState = (state) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;

    }

    private void LateUpdate()
    {
        float alpha = (bInSettings) ? 1 : 0;
        settingsGroup.alpha = Mathf.Lerp(settingsGroup.alpha, alpha, 0.2f);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        GameObject.FindFirstObjectByType<loadingScreen>().StartLoadScene("testScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
