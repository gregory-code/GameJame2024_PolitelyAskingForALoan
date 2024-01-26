using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public delegate void OnHealHealth();
    public event OnHealHealth onHealHealth;

    public delegate void OnBlasting();
    public event OnBlasting onBlasting;

    public delegate void OnSpecialEvent(string eventName);
    public event OnSpecialEvent onSpecialEvent;

    public delegate void OnHESGOTAGUN(bool bHasIt);
    public event OnHESGOTAGUN onHESGOTAGUN;

    public delegate void OnHEHASPOOPONHISHAND(bool hasPoop);
    public event OnHEHASPOOPONHISHAND onHEHASPOOPONHISHAND;

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

    [SerializeField] AudioMixerSnapshot regularSnapshot;
    [SerializeField] AudioMixerSnapshot mutedSnapshot;

    [SerializeField] TextMeshProUGUI theyrehere;

    public delegate void OnSettings(bool state);
    public event OnSettings onSettings;

    [SerializeField] bool bInChat = false;
    private bool bAiming = false;
    private bool bInventory = false;
    private bool bSettings = false;
    private bool bDead;
    private bool bInvincible = false;
    private Transform targetNPC = null;
    [SerializeField] Transform followTransform;

    [SerializeField] ChatEngine chat;

    [SerializeField] Animator racoonAnimator;
    
    [SerializeField] Transform headTransform;
    public Transform GetPlayerHead()
    {
        return headTransform;
    }

    public bool reloading;

    private int currentAmmo;
    int ammoReserves = 5;
    int health = 3;
    [SerializeField] int maxAmmo;

    [SerializeField] ItemBase nothingItem;
    [SerializeField] ItemBase gunItem;
    [SerializeField] ItemBase nuke;

    private ItemBase currentItem;

    [SerializeField] GameObject[] itemGameObjects;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentAmmo = maxAmmo;

        playerInventory.AddItem(nothingItem);
        playerInventory.AddItem(gunItem);

        chat.onSpecialOption += SpecialDialogue;

        ItemSlot[] slots = playerInventory.GetSlots();
        foreach(ItemSlot slot in slots)
        {
            slot.onSelect += SelectItem;
        }

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    [SerializeField] ItemBase key;
    private void SpecialDialogue(string option)
    {
        switch(option)
        {
            case "StartRun":
                AddItem(key);
                onSpecialEvent?.Invoke(option);
                if (bEpicMusic == false)
                {
                    bEpicMusic = true;
                }
                break;
        }
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
        itemGameObjects[item.GetID()].SetActive(true);
    }

    private void FinishSelect()
    {
        Debug.Log("Selected Item");
        if (currentItem == null)
            return;

        if (currentItem.GetID() == 1)
        {
            onHESGOTAGUN?.Invoke(true);
            racoonAnimator.SetBool("gun", true);
        }
        else
        {
            onHESGOTAGUN?.Invoke(false);
            racoonAnimator.SetBool("gun", false);
        }

        if(currentItem.GetID() == 5)
        {
            onHEHASPOOPONHISHAND?.Invoke(true);
        }
        else
        {
            onHEHASPOOPONHISHAND?.Invoke(false);
        }
        onSelectItem?.Invoke(currentItem.GetID());
    }



    public int GetCurrentItemID()
    {
        if (currentItem == null)
            return 0;

        return currentItem.GetID();
    }

    void Update()
    {
        float lerp = (bEpicMusic) ? 1 : 0;
        float reverseLerp = (bEpicMusic) ? 0 : 1;
        float volume = GameObject.Find("robed_a_bank").GetComponent<AudioSource>().volume;
        GameObject.Find("robed_a_bank").GetComponent<AudioSource>().volume = Mathf.Lerp(volume, lerp, 10 * Time.deltaTime);
        GameObject.Find("bankTheme").GetComponent<AudioSource>().volume = Mathf.Lerp(volume, reverseLerp, 10 * Time.deltaTime);

        if (bDead)
            return;

        if (bInChat)
        {
            GoToIdle();
            onRotate?.Invoke(targetNPC.position);
            return;
        }

        LookInput();
        MoveInput(true);
    }

    private void GoToIdle()
    {
        float lerpRight = Mathf.Lerp(racoonAnimator.GetFloat("leftSpeed"), 0, 7 * Time.deltaTime);
        float lerpFoward = Mathf.Lerp(racoonAnimator.GetFloat("fowardSpeed"), 0, 7 * Time.deltaTime);

        racoonAnimator.SetFloat("leftSpeed", lerpRight);
        racoonAnimator.SetFloat("fowardSpeed", lerpFoward);
    }

    private void MoveInput(bool bRotateThatDir)
    {
        Vector2 inputVector = playerControls.Player.Movement.ReadValue<Vector2>();

        if (inputVector == Vector2.zero)
        {
            GoToIdle();
            return;
        }

        onMoveInput?.Invoke(inputVector, playerCamera.GetCameraFoward(), bRotateThatDir);
    }

    public void TakeDamage(Vector3 shotDirection, Rigidbody shotRigidbody)
    {
        if (bInvincible)
            return;

        health--;

        racoonAnimator.SetTrigger("damaged");
        racoonAnimator.SetLayerWeight(1, 0);

        StartCoroutine(IFrames());

        bool kills = false;
        if (health < 0)
        {
            followTransform.SetParent(headTransform);
            bDead = true;
            characterController.enabled = false;
            kills = true;
            StartCoroutine(RestartDeath());
        }

        onTakeDamage?.Invoke(shotDirection, shotRigidbody, kills);
    }

    private IEnumerator RestartDeath()
    {
        yield return new WaitForSeconds(3f);
        GameObject.FindFirstObjectByType<loadingScreen>().StartLoadScene("testScene");
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

        if (bInventory || bAiming || bSettings)
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

        if (bInventory || bSettings || reloading)
            return;

        if (context.performed)
        {
            bAiming = true;
            ProcessAimInput();
        }

        if(context.canceled)
        {
            bAiming = false;
            ProcessAimInput();
        }
    }

    private void ProcessAimInput()
    {
        float weight = (bAiming) ? 1 : 0;
        racoonAnimator.SetLayerWeight(1, weight);
        racoonAnimator.SetBool("aiming", bAiming);
        onAim?.Invoke(bAiming);
    }

    public void InventoryInput(InputAction.CallbackContext context)
    {
        if (bDead)
            return;

        if (bAiming || bInChat || bSettings)
            return;

        if (context.performed)
        {
            bInventory = true;
            mutedSnapshot.TransitionTo(0.5f);
            ManageInventory();
        }

        if(context.canceled)
        {
            bInventory = false;
            FinishSelect();
            regularSnapshot.TransitionTo(0.5f);
            ManageInventory();
        }
    }

    private void ManageInventory()
    {
        if (bAiming == true)
        {
            bAiming = false;
            racoonAnimator.SetBool("aiming", bAiming);
            onAim?.Invoke(bAiming);
        }
        onInventory?.Invoke(bInventory);
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (bDead)
            return;

        if (bAiming || bInventory)
            return;

        if (context.performed)
        {
            bSettings = !bSettings;
            if(bSettings)
            {
                mutedSnapshot.TransitionTo(0.5f);
            }
            else
            {
                regularSnapshot.TransitionTo(0.5f);
            }
            onSettings?.Invoke(bSettings);
        }
    }

    private bool bEpicMusic;

    public void BlastingInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (bDead || reloading)
                return;

            if (bInChat)
            {
                onInteract?.Invoke();
                return;
            }

            if (currentItem == null || bSettings || bInventory)
                return;

            if (currentItem.GetID() == 2 || currentItem.GetID() == 3)
            {
                if (health >= 3)
                {
                    GameObject.FindFirstObjectByType<Notification>().CreateNotification("Health is already full!", Color.green, currentItem.GetImage());
                    return;
                }
                racoonAnimator.SetTrigger("Eat");
                racoonAnimator.SetLayerWeight(1, 1);
                reloading = true;
                StartCoroutine(Eatting());
                return;
            }

            if (!bAiming)
                return;

            if(currentItem.GetID() == 4)
            {
                racoonAnimator.SetTrigger("GrenadeThrow");
                racoonAnimator.SetLayerWeight(1, 1);
                reloading = true;
                StartCoroutine(ThrowNade());
                return;
            }

            if (currentItem.GetID() != 1)
                return;

            if (currentAmmo > 0)
            {
                racoonAnimator.SetTrigger("shoot");

                if(bEpicMusic == false)
                {
                    bEpicMusic = true;
                }

                currentAmmo -= 1;

                onBlasting?.Invoke();
            }
        }
    }

    [SerializeField] GameObject nade;
    [SerializeField] Transform handTransform;

    private IEnumerator ThrowNade()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject nadeClone = Instantiate(nade, handTransform.position, handTransform.rotation);
        Rigidbody rbody = nadeClone.GetComponent<Rigidbody>();
        rbody.AddForce(Camera.main.transform.forward * 15, ForceMode.VelocityChange);
        reloading = false;
        playerInventory.RemoveItem(currentItem);
        SelectItem(nothingItem);
    }

    private IEnumerator Eatting()
    {
        yield return new WaitForSeconds(1f);
        reloading = false;
        health += 1;
        onHealHealth?.Invoke();
        playerInventory.RemoveItem(currentItem);
        SelectItem(nothingItem);

    }

    [SerializeField] Sprite bulletImage;

    public void PickUpAmmo(int amount)
    {
        if (bDead)
            return;

        ammoReserves += amount;
        GameObject.FindFirstObjectByType<Notification>().CreateNotification("- Picked Up -   x" + amount + " Bullets", Color.white, bulletImage);
        onAddAmmo(amount);
    }

    public void ReloadInput(InputAction.CallbackContext context)
    {
        if (bDead)
            return;

        if (bInventory || bInChat || bSettings || reloading)
            return;

        if (context.performed && ammoReserves > 0 && currentAmmo < maxAmmo)
        {
            int amountReloading = maxAmmo - currentAmmo;
            ammoReserves -= amountReloading;

            racoonAnimator.SetTrigger("Reload");
            racoonAnimator.SetLayerWeight(1, 1);
            reloading = true;
            bAiming = false;
            racoonAnimator.SetBool("aiming", bAiming);
            onAim?.Invoke(bAiming);
            StartCoroutine(reloadingWait(amountReloading));
        }
    }

    private IEnumerator reloadingWait(int amountReloading)
    {
        yield return new WaitForSeconds(1.4f);
        reloading = false;
        while (ammoReserves < 0)
        {
            amountReloading--;
            ammoReserves++;
        }

        

        currentAmmo += amountReloading;
        onReload?.Invoke(amountReloading);
    }

    public void AddItem(ItemBase itemToAdd)
    {
        if(itemToAdd.GetID() == 6)
        {
            theyrehere.text = "ESCAPE TO YOUR CAR";
            GameObject.FindFirstObjectByType<winBox>().CanWin();
        }
        GameObject.FindFirstObjectByType<Notification>().CreateNotification("- Picked Up -    " + itemToAdd.GetName(), Color.white, itemToAdd.GetImage());
        playerInventory.AddItem(itemToAdd);
    }

    public void SetTargetNPC(Transform newTarget, bool state)
    {
        targetNPC = newTarget;
        bInChat = state;

        if(state == false)
            StartCoroutine(STOPCHATTING());
    }

    private IEnumerator STOPCHATTING()
    {
        yield return new WaitForEndOfFrame();
        GameObject.FindObjectOfType<ChatEngine>().bInChat = false;
        bInChat = false;
        targetNPC = null;
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
