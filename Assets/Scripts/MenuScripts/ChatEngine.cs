using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatEngine : MonoBehaviour
{
    [SerializeField] Animator chatAnimator;
    [SerializeField] Player player;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI regularName;
    [SerializeField] TextMeshProUGUI colorName;

    private bool bCanAct = true;

    private void SetStates(bool state)
    {
        bCanAct = state;
        player.SetCanAct(state);
    }

    public void StartChat(string chatterName, string startingDialouge, Color chatterColor)
    {
        if (bCanAct == false)
            return;

        SetStates(false);
        chatAnimator.SetTrigger("startChat");

        dialogueText.text = startingDialouge;
        regularName.text = chatterName;
        colorName.text = chatterName;

        colorName.color = chatterColor;

        player.onInteract += GoNext;
    }

    private void GoNext()
    {
        EndChat();
    }

    private void EndChat()
    {
        SetStates(true);
        chatAnimator.SetTrigger("endChat");

        player.SetTargetNPC(null);

        player.onInteract -= GoNext;
    }
}
