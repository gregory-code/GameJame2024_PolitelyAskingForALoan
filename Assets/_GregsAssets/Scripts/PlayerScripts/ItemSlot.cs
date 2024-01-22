using System;
using System.Collections;
using System.Collections.Generic;
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

    bool bHasItem;

    private ItemBase myItem;

    [SerializeField] Color selectedColor;
    [SerializeField] Color unselectedColor;

    [SerializeField] Color Transparent;
    [SerializeField] Color FullColor;
    [SerializeField] Color fullyTransparent;

    private bool bHover;

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
        bHasItem = true;
    }

    private void Update()
    {
        Vector3 scaleLerp = originalScale;

        if (bHover)
            scaleLerp *= scaleAmount;

        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, (speed - 0.1f));
        transform.localScale = Vector3.Lerp(transform.localScale, scaleLerp, speed);
    }

    private void Unselect(ItemBase item)
    {
        if (item == myItem)
            return;

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

        onSelect?.Invoke(myItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myItem == null)
            return;

        bHover = false;
    }
}