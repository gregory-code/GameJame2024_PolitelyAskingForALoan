using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcBase : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] Color npcColor;

    public string GetName()
    {
        return npcName;
    }

    public Color GetColor()
    {
        return npcColor;
    }
}
