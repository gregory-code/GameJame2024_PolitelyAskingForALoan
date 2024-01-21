using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    
    PlayerControls playerControls;

    public delegate void OnMoveInput(Vector2 inputVector, Vector3 cameraFoward);
    public event OnMoveInput onMoveInput;

    public delegate void OnMouseHoverInput(Vector3 hitVector);
    public event OnMouseHoverInput onMouseHoverInput;


    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    void Update()
    {
        MoveInput();
        MouseHoverInput();
    }

    private void MoveInput()
    {
        Vector2 inputVector = playerControls.Player.Movement.ReadValue<Vector2>();
        onMoveInput?.Invoke(inputVector, playerCamera.GetCameraFoward());
    }

    private void MouseHoverInput()
    {
        RaycastHit hit;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(raycast, out hit))
        {
            onMouseHoverInput?.Invoke(hit.point);
        }
    }
}
