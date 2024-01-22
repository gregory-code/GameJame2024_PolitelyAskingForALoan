using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    
    PlayerControls playerControls;

    public delegate void OnMoveInput(Vector2 inputVector, Vector3 cameraFoward, bool bRotateThatDir);
    public event OnMoveInput onMoveInput;

    public delegate void OnRotate(Vector3 rotateVector);
    public event OnRotate onRotate;

    public delegate void OnInteract();
    public event OnInteract onInteract;

    public delegate void OnAim(bool state);
    public event OnAim onAim;

    public delegate void OnInventory(bool state);
    public event OnInventory onInventory;

    private bool bInChat = false;
    private bool bAiming = false;
    private bool bInventory = false;
    private Transform targetNPC = null;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
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
        //MouseRayCastInput();
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
        if (bInventory)
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

    private void MouseRayCastInput()
    {
        RaycastHit hit;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(raycast, out hit))
        {
            onRotate?.Invoke(hit.point);
        }
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
