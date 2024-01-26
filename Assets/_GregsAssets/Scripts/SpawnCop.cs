using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnCop : MonoBehaviour
{
    [SerializeField] float timeDelay;
    [SerializeField] Player player;
    [SerializeField] GameObject copPrefab;

    [SerializeField] GameObject copCars;

    [SerializeField] float secondsToArrive;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject copsText;
    [SerializeField] GameObject copsText2;

    [SerializeField] bool handleTimer;

    bool gameIsLive = false;

    public void Start()
    {
        player.onSpecialEvent += SpecialEvent;
        player.onBlasting += StartGame;
    }

    private void StartGame()
    {
        if (gameIsLive == true)
            return;


        gameIsLive = true;
        StartCoroutine(Spawn());
        copCars.SetActive(true);
    }

    private void SpecialEvent(string eventName)
    {
        switch (eventName)
        {
            case "StartRun":
                if (gameIsLive == true)
                    return;

                copCars.SetActive(true);
                gameIsLive = true;
                StartCoroutine(Spawn());
                break;
        }
    }

    public IEnumerator Spawn()
    {
        if(handleTimer)
            copsText.SetActive(true);
        while(secondsToArrive > 0)
        {
            yield return new WaitForSeconds(1f);
            secondsToArrive--;
            timeText.text = "" + secondsToArrive;
        }

        if(handleTimer)
        {
            copsText.SetActive(false);
            copsText2.SetActive(true);
        }


        while (gameIsLive)
        {
            yield return new WaitForSeconds(timeDelay);
            if(gameIsLive)
            {
                GameObject cop = Instantiate(copPrefab, transform.position, transform.rotation);
                cop.GetComponent<copCat>().guardPoints = GameObject.FindFirstObjectByType<GuardPoints>().GetGuardPoints();
                cop.GetComponent<copCat>().GoToGuard();
            }
        }
    }
}
