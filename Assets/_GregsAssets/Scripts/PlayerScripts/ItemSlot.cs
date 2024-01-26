using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Player player;
    [SerializeField] PlayerInventory inventory;

    public delegate void OnSelect(ItemBase item);
    public event OnSelect onSelect;

    private Vector3 originalPos;
    private Vector3 originalScale;

    [SerializeField] float scaleAmount;
    [SerializeField] float speed = 0.5f;

    [SerializeField] Image slotImage;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText;

    bool bHasItem;

    private ItemBase myItem;

    [SerializeField] Color selectedColor;
    [SerializeField] Color unselectedColor;

    [SerializeField] Color Transparent;
    [SerializeField] Color FullColor;
    [SerializeField] Color fullyTransparent;

    private bool bHover;
    private bool selected;

    private void Start()
    {
        player.onInventory += Openinventory;

        foreach(ItemSlot slot in inventory.GetSlots())
        {
            slot.onSelect += Unselect;
        }

        slotImage.color = unselectedColor;

        originalPos = transform.localPosition;
        originalScale = transform.localScale;
    }

    public int CheckItemID()
    {
        return myItem.GetID();
    }

    private void Openinventory(bool state)
    {
        if(state)
        {
            transform.localPosition = Vector3.zero;
        }
    }

    public bool HasItem()
    {
        return bHasItem;
    }

    public void GetItem(ItemBase item)
    {
        myItem = item;
        itemImage.sprite = item.GetImage();
        itemImage.color = Transparent;
        itemNameText.text = item.GetName();
        bHasItem = true;
    }

    public void RemoveItem()
    {
        myItem = null;
        itemImage.sprite = null;
        itemImage.color = fullyTransparent;
        itemNameText.text = "";
        bHasItem = false;
    }

    private void Update()
    {
        Vector3 scaleLerp = originalScale;

        if (bHover)
            scaleLerp *= scaleAmount;

        if(selected)
        {
            itemNameText.color = Color.Lerp(itemNameText.color, FullColor, 0.1f);
        }
        else
        {
            itemNameText.color = Color.Lerp(itemNameText.color, fullyTransparent, 0.1f);
        }
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, (speed - 0.1f));
        transform.localScale = Vector3.Lerp(transform.localScale, scaleLerp, speed);
    }

    private void Unselect(ItemBase item)
    {
        if (item == myItem)
            return;

        selected = false;
        slotImage.color = unselectedColor;
        itemImage.color = (bHasItem) ? Transparent : fullyTransparent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myItem == null)
            return;

        slotImage.color = selectedColor;
        itemImage.color = (bHasItem) ? FullColor : fullyTransparent;
        bHover = true;

        selected = true;
        onSelect?.Invoke(myItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myItem == null)
            return;

        bHover = false;
    }
}
