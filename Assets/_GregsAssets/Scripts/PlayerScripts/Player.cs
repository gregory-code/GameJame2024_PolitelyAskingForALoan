using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] CharacterController characterController;
    
    PlayerControls playerControls;

    public delegate void OnMoveInput(Vector2 inputVector, Vector3 cameraFoward, bool bRotateThatDir);
    public event OnMoveInput onMoveInput;

    public delegate void OnRotate(Vector3 rotateVector);
    public event OnRotate onRotate;

    public delegate void OnInteract();
    public event OnInteract onInteract;

    public delegate void OnBlasting();
    public event OnBlasting onBlasting;

    public delegate void OnHESGOTAGUN(bool bHasIt);
    public event OnHESGOTAGUN onHESGOTAGUN;

    public delegate void OnTakeDamage(Vector3 shotDirection, Rigidbody shotRigidbody, bool wouldKill);
    public event OnTakeDamage onTakeDamage;

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
    private bool bDead;
    private bool bInvincible = false;
    private Transform targetNPC = null;
    [SerializeField] Transform followTransform;

    [SerializeField] Animator racoonAnimator;
    
    [SerializeField] Transform headTransform;
    public Transform GetPlayerHead()
    {
        return headTransform;
    }

    private int currentAmmo;
    int ammoReserves = 5;
    int health = 3;
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
        if(item.GetID() == 1)
        {
            onHESGOTAGUN?.Invoke(true);
        }
        else
        {
            onHESGOTAGUN?.Invoke(false);
        }
        currentItem = item;
        onSelectItem?.Invoke(item.GetID());
        itemGameObjects[item.GetID()].SetActive(true);
    }

    void Update()
    {
        if (bDead)
            return;

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

    public void TakeDamage(Vector3 shotDirection, Rigidbody shotRigidbody)
    {
        if (bInvincible)
            return;

        health--;

        StartCoroutine(IFrames());

        bool kills = false;
        if (health < 0)
        {
            followTransform.SetParent(headTransform);
            bDead = true;
            characterController.enabled = false;
            kills = true;
        }

        onTakeDamage?.Invoke(shotDirection, shotRigidbody, kills);
    }

    private IEnumerator IFrames()
    {
        bInvincible = true;
        yield return new WaitForSeconds(0.2f);
        bInvincible = false;
    }

    private void LookInput()
    {
        Vector2 lookVector = playerControls.Player.Look.ReadValue<Vector2>();
        playerCamera.HandleLook(lookVector);
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (bDead)
            return;

        if (bInventory || bAiming)
            return;

        if (context.performed)
        {
            onInteract?.Invoke();
        }
    }

    public void AimInput(InputAction.CallbackContext context)
    {
        if (bDead)
            return;

        if (bInventory)
            return;

        if (context.performed || context.canceled)
        {
            bAiming = !bAiming;
            racoonAnimator.SetBool("aiming", bAiming);
            onAim?.Invoke(bAiming);
        }
    }

    public void InventoryInput(InputAction.CallbackContext context)
    {
        if (bDead)
            return;

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
        if (bDead)
            return;

        if (currentItem == null)
            return;

        if (bInventory || !bAiming || currentItem.GetID() != 1)
            return;

        if (context.performed && currentAmmo > 0)
        {
            racoonAnimator.SetTrigger("shoot");

            currentAmmo -= 1;

            onBlasting?.Invoke();
        }
    }

    public void PickUpAmmo(int amount)
    {
        if (bDead)
            return;

        ammoReserves += amount;
        GameObject.FindFirstObjectByType<Notification>().CreateNotification("- Picked Up -   x" + amount + " Bullets", Color.white, null);
        onAddAmmo(amount);
    }

    public void ReloadInput(InputAction.CallbackContext context)
    {
        if (bDead)
            return;

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
