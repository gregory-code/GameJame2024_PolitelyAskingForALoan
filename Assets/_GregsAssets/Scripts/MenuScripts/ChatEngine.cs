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
    TalkBox currentTalkBox;
    int currentTalk;

    public void StartChat(npcBase myNPC, Transform npcTransform, string npcName, TalkBox talk, Color chatterColor)
    {
        if (bInChat)
            return;

        bInChat = true;
        currentNPC = myNPC;
        currentTalkBox = talk;

        player.SetTargetNPC(npcTransform, true);
        chatAnimator.SetTrigger("startChat");

        StartCoroutine(TypeText(talk.dialogues[0]));
        currentTalk = 0;
        regularName.text = npcName;
        colorName.text = npcName;

        colorName.color = chatterColor;

        player.onInteract += GoNext;
    }

    private void GoNext()
    {
        if(currentTalk < currentTalkBox.dialogues.Length - 1)
        {
            StopAllCoroutines();
            currentTalk++;
            StartCoroutine(TypeText(currentTalkBox.dialogues[currentTalk]));
        }
        else
        {
            EndChat();
        }
    }

    private IEnumerator TypeText(string dialogue)
    {
        dialogueText.text = "";
        foreach (char c in dialogue)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
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
