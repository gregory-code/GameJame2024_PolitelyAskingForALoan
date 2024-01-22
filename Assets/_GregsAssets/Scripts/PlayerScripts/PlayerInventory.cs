using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] CanvasGroup inventoryGroup;

    bool bInventory;

    void Start()
    {
        player.onInventory += ToggleInventory;
    }

    private void LateUpdate()
    {
        float alpha = (bInventory) ? 1 : 0;
        inventoryGroup.alpha = Mathf.Lerp(inventoryGroup.alpha, alpha, 0.1f);
    }

    private void ToggleInventory(bool state)
    {
        Time.timeScale = (state) ? 0f : 1f;
        bInventory = state;
        inventoryGroup.blocksRaycasts = state;
        inventoryGroup.interactable = state;
        Cursor.visible = state;
        Cursor.lockState = (state) ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
