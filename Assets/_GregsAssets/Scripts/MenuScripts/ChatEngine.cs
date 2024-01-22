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

    bool bInChat = false;
    npcBase currentNPC;

    public void StartChat(npcBase myNPC, Transform npcTransform, string npcName, string startingDialouge, Color chatterColor)
    {
        if (bInChat)
            return;

        bInChat = true;
        currentNPC = myNPC;

        player.SetTargetNPC(npcTransform, true);
        chatAnimator.SetTrigger("startChat");

        dialogueText.text = startingDialouge;
        regularName.text = npcName;
        colorName.text = npcName;

        colorName.color = chatterColor;

        player.onInteract += GoNext;
    }

    private void GoNext()
    {
        EndChat();
    }

    private void EndChat()
    {
        bInChat = false;
        currentNPC.TalkState(null, false);
        player.SetTargetNPC(null, false);
        chatAnimator.SetTrigger("endChat");

        player.onInteract -= GoNext;
    }
}
