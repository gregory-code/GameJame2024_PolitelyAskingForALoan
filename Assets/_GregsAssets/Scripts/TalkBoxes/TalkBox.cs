using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TalkBox")]
public class TalkBox : ScriptableObject
{
    public string[] dialogues;

    public bool hasOption;

    [SerializeField][Range(2, 3)] int options;
    public int GetOptions()
    {
        return options;
    }

    public string[] specialEvent = new string[3];

    public Sprite[] optionSprites = new Sprite[3];

    public string[] optionText = new string[3];

    public int[] optionItemID = new int[3];

    public TalkBox[] optionTalk = new TalkBox[3];
}
