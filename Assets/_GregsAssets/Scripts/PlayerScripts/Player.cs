using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] PlayerInventory playerInventory;
    
    PlayerControls playerControls;

    public delegate void OnMoveInput(Vector2 inputVector, Vector3 cameraFoward, bool bRotateThatDir);
    public event OnMoveInput onMoveInput;

    public delegate void OnRotate(Vector3 rotateVector);
    public event OnRotate onRotate;

    public delegate void OnInteract();
    public event OnInteract onInteract;

    public delegate void OnBlasting();
    public event OnBlasting onBlasting;

    public delegate void OnAddAmmo(int amount);
    public event OnAddAmmo onAddAmmo;

    public delegate void OnAim(bool state);
    public event OnAim onAim;

    public delegate void OnSelectItem(int itemID);
    public event OnSelectItem onSelectItem;

    public delegate void OnReload(int countReloading);
    public event OnReload onReload;

    public delegate void OnInventory(bool state);
    public event OnInventory onInventory;

    private bool bInChat = false;
    private bool bAiming = false;
    private bool bInventory = false;
    private Transform targetNPC = null;

    private int currentAmmo;
    int ammoReserves = 5;
    [SerializeField] int maxAmmo;

    [SerializeField] ItemBase nothingItem;
    [SerializeField] ItemBase gunItem;

    private ItemBase currentItem;

    [SerializeField] GameObject[] itemGameObjects;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentAmmo = maxAmmo;

        playerInventory.AddItem(nothingItem);
        playerInventory.AddItem(gunItem);

        ItemSlot[] slots = playerInventory.GetSlots();
        foreach(ItemSlot slot in slots)
        {
            slot.onSelect += SelectItem;
        }

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    private void SelectItem(ItemBase item)
    {
        if (item == null)
            return;

        foreach(GameObject itemGameObject in itemGameObjects)
        {
            itemGameObject.SetActive(false);
        }
        currentItem = item;
        onSelectItem?.Invoke(item.GetID());
        itemGameObjects[item.GetID()].SetActive(true);
    }

    void Update()
    {

        if (bInChat)
        {
            onRotate?.Invoke(targetNPC.position);
            return;
        }

        LookInput();
        MoveInput(true);
    }

    private void MoveInput(bool bRotateThatDir)
    {
        Vector2 inputVector = playerControls.Player.Movement.ReadValue<Vector2>();

        if (inputVector == Vector2.zero)
            return;

        onMoveInput?.Invoke(inputVector, playerCamera.GetCameraFoward(), bRotateThatDir);
    }

    private void LookInput()
    {
        Vector2 lookVector = playerControls.Player.Look.ReadValue<Vector2>();
        playerCamera.HandleLook(lookVector);
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (bInventory || bAiming)
            return;

        if (context.performed)
        {
            onInteract?.Invoke();
        }
    }

    public void AimInput(InputAction.CallbackContext context)
    {
        if (bInventory)
            return;

        if (context.performed || context.canceled)
        {
            bAiming = !bAiming;
            onAim?.Invoke(bAiming);
        }
    }

    public void InventoryInput(InputAction.CallbackContext context)
    {
        if (bAiming || bInChat)
            return;

        if (context.performed || context.canceled)
        {
            bInventory = !bInventory;
            onInventory?.Invoke(bInventory);
        }
    }

    public void BlastingInput(InputAction.CallbackContext context)
    {
        if (currentItem == null)
            return;

        if (bInventory || !bAiming || currentItem.GetID() != 1)
            return;

        if (context.performed && currentAmmo > 0)
        {
            currentAmmo -= 1;

            onBlasting?.Invoke();
        }
    }

    public void PickUpAmmo(int amount)
    {
        ammoReserves += amount;
        GameObject.FindFirstObjectByType<Notification>().CreateNotification("- Picked Up -   x" + amount + " Bullets", Color.white, null);
        onAddAmmo(amount);
    }

    public void ReloadInput(InputAction.CallbackContext context)
    {
        if (bInventory || bInChat)
            return;

        if (context.performed && ammoReserves > 0 && currentAmmo < maxAmmo)
        {
            int amountReloading = maxAmmo - currentAmmo;
            ammoReserves -= amountReloading;

            while(ammoReserves < 0)
            {
                amountReloading--;
                ammoReserves++;
            }

            currentAmmo += amountReloading;
            onReload?.Invoke(amountReloading);
        }
    }

    private void AddItem(ItemBase itemToAdd)
    {
        playerInventory.AddItem(itemToAdd);
    }

    public void SetTargetNPC(Transform newTarget, bool state)
    {
        targetNPC = newTarget;
        bInChat = state;
    }

    public bool IsInChat()
    {
        return bInChat;
    }

    public Transform GetTargetNPC()
    {
        return targetNPC;
    }
}
