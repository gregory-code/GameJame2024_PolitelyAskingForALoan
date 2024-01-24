using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telle : npcBase
{
    [SerializeField] ChatInteraction myInteraction;

    void Start()
    {
        myInteraction.onInteract += StartTalking;
    }

    private void StartTalking(Player interactingPlayer)
    {
        TalkState(interactingPlayer.transform, true);
        GameObject.FindFirstObjectByType<ChatEngine>().StartChat(this, this.transform, GetName(), "Hi", GetColor());
    }
}
