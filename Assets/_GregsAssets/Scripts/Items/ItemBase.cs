using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Item")]
public class ItemBase : ScriptableObject
{
    [SerializeField] Sprite itemImage;
    [SerializeField] string itemName;
    [SerializeField] int itemID;

    public Sprite GetImage()
    {
        return itemImage;
    }

    public string GetName()
    {
        return itemName;
    }

    public int GetID()
    {
        return itemID;
    }
}
