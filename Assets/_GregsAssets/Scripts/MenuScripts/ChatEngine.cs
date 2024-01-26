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

    [SerializeField] Image[] options;
    [SerializeField] Image[] optionImages;
    [SerializeField] TextMeshProUGUI[] optionTexts;

    [SerializeField] Color fullyVisible;
    [SerializeField] Color transparent;

    [SerializeField] Sprite redOption;
    [SerializeField] Sprite blueOption;

    public delegate void OnSpecialOption(string option);
    public event OnSpecialOption onSpecialOption;

    public bool bInChat = false;
    npcBase currentNPC;
    TalkBox currentTalkBox;
    int currentTalk;

    public void StartChat(npcBase myNPC, Transform npcTransform, string npcName, TalkBox talk, Color chatterColor)
    {
        if (bInChat)
            return;

        Debug.Log("Start Chat");

        chatAnimator.SetTrigger("start");
        chatAnimator.ResetTrigger("hide2");
        chatAnimator.ResetTrigger("show2");
        chatAnimator.ResetTrigger("hide3");
        chatAnimator.ResetTrigger("show3");

        dialogueText.text = "";

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
            if (currentTalkBox.hasOption)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                for (int i = 0; i < currentTalkBox.GetOptions(); i++)
                {
                    if (currentTalkBox.optionItemID[i] == player.GetCurrentItemID() || currentTalkBox.optionItemID[i] == 0)
                    {
                        optionTexts[i].text = currentTalkBox.optionText[i];
                        options[i].sprite = blueOption;
                    }
                    else
                    {
                        optionTexts[i].text = "Holding wrong item!" ;
                        options[i].sprite = redOption;

                    }

                    optionImages[i].sprite = currentTalkBox.optionSprites[i];
                    optionImages[i].color = (currentTalkBox.optionSprites[i] == null) ? transparent : fullyVisible ;
                }
                chatAnimator.SetTrigger("show" + currentTalkBox.GetOptions());
            }
            else
            {

                EndChat();
            }
        }
    }

    public void SelectOption(int which)
    {
        if (options[which].sprite == redOption)
            return;

        dialogueText.text = "";

        chatAnimator.SetTrigger("hide" + currentTalkBox.GetOptions());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        onSpecialOption?.Invoke(currentTalkBox.specialEvent[which]);

        currentTalkBox = currentTalkBox.optionTalk[which];
        currentTalk = 0;
        StartCoroutine(TypeText(currentTalkBox.dialogues[0]));
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
        player.onInteract -= GoNext;
        dialogueText.text = "";
        currentNPC.TalkState(false);
        player.SetTargetNPC(null, false);
        chatAnimator.SetTrigger("endChat");
        StartCoroutine(EndingChat());
    }

    private IEnumerator EndingChat()
    {
        yield return new WaitForEndOfFrame();
        bInChat = false;
    }
}
