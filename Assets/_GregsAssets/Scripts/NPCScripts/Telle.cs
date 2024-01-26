using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Telle : npcBase
{
    [SerializeField] ChatInteraction myInteraction;
    [SerializeField] TalkBox firstTalk;
    [SerializeField] TalkBox gaveKey;
    [SerializeField] TalkBox gunTalk;
    [SerializeField] TalkBox AskForLoan;

    TalkBox currentTalk;

    void Start()
    {
        myInteraction.onInteract += StartTalking;
        onHeardThat += SeesGun;
        onSeesGun += SeesGun;
        onSpecialEvent += SpecialEvent;
        currentTalk = firstTalk;
    }

    private IEnumerator AskLoan()
    {
        yield return new WaitForSeconds(90);
        currentTalk = AskForLoan;
    }

    private void SpecialEvent(string eventname)
    {
        switch(eventname)
        {
            case "StartRun":
                gunTalk = gaveKey;
                AskForLoan = gaveKey;
                currentTalk = gaveKey;
                break;

            case "Credits":
                SceneManager.LoadScene("creditsScene");
                break;
        }
    }

    private void SeesGun()
    {
        currentTalk = gunTalk;
    }

    private void StartTalking(Player interactingPlayer)
    {
        if (interactingPlayer.IsInChat())
            return;

        TalkState(true);
        GameObject.FindFirstObjectByType<ChatEngine>().StartChat(this, this.transform, GetName(), currentTalk, GetColor());
    }


}
