using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBlock : npcBase
{
    [SerializeField] ChatInteraction myInteraction;

    void Start()
    {
        myInteraction.onInteract += StartTalking;
    }

    private void StartTalking(Player interactingPlayer)
    {
        interactingPlayer.SetTargetNPC(this.transform);
        GameObject.FindFirstObjectByType<ChatEngine>().StartChat(GetName(), "Hi", GetColor());
    }
}
