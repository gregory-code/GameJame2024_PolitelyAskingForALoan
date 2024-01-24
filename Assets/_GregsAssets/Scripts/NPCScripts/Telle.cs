using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telle : npcBase
{
    [SerializeField] ChatInteraction myInteraction;
    [SerializeField] TalkBox firstTalk;
    [SerializeField] TalkBox gunTalk;

    TalkBox currentTalk;

    void Start()
    {
        myInteraction.onInteract += StartTalking;
        onHeardThat += SeesGun;
        onSeesGun += SeesGun;
        currentTalk = firstTalk;
    }

    private void SeesGun()
    {
        currentTalk = gunTalk;
    }

    private void StartTalking(Player interactingPlayer)
    {
        TalkState(interactingPlayer.transform, true);
        GameObject.FindFirstObjectByType<ChatEngine>().StartChat(this, this.transform, GetName(), currentTalk, GetColor());
    }


}
