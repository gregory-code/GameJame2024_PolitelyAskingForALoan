using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    
    PlayerControls playerControls;

    public delegate void OnMoveInput(Vector2 inputVector, Vector3 cameraFoward);
    public event OnMoveInput onMoveInput;

    public delegate void OnMouseRaycast(Vector3 hitVector);
    public event OnMouseRaycast onMouseRaycast;

    public delegate void OnInteract();
    public event OnInteract onInteract;

    private bool bCanAct = true;
    private Transform targetNPC = null;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    void Update()
    {
        if (targetNPC != null)
            onMouseRaycast?.Invoke(targetNPC.position);

        if (!bCanAct)
            return;

        MoveInput();
        LookInput();
        //MouseHoverInput();
    }

    private void MoveInput()
    {
        Vector2 inputVector = playerControls.Player.Movement.ReadValue<Vector2>();
        onMoveInput?.Invoke(inputVector, playerCamera.GetCameraFoward());
    }

    private void LookInput()
    {
        Vector2 lookVector = playerControls.Player.Look.ReadValue<Vector2>();
        playerCamera.HandleLook(lookVector);
    }

    private void MouseHoverInput()
    {
        RaycastHit hit;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(raycast, out hit))
        {
            onMouseRaycast?.Invoke(hit.point);
        }
    }

    public void SetCanAct(bool state)
    {
        bCanAct = state;
    }

    public void SetTargetNPC(Transform newTarget)
    {
        targetNPC = newTarget;
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onInteract?.Invoke();
        }
    }
}
