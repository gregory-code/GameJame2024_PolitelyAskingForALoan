using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telle : npcBase
{
    [SerializeField] ChatInteraction myInteraction;
    [SerializeField] TalkBox firstTalk;
    [SerializeField] TalkBox gaveKey;
    [SerializeField] TalkBox gunTalk;

    TalkBox currentTalk;

    void Start()
    {
        myInteraction.onInteract += StartTalking;
        onHeardThat += SeesGun;
        onSeesGun += SeesGun;
        onSpecialEvent += SpecialEvent;
        currentTalk = firstTalk;
    }

    private void SpecialEvent(string eventname)
    {
        switch(eventname)
        {
            case "StartRun":
                gunTalk = gaveKey;
                currentTalk = gaveKey;
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
