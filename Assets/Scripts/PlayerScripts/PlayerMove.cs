using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] CharacterController characterController;
    [SerializeField] float gravity = -20f;
    [SerializeField] float currentSpeed = 3f;

    Vector3 playerVelocity;
    PlayerControls playerControls;

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    private void Update()
    {
        MoveInput();
        MoveGravity();
    }

    public void MoveInput()
    {
        Vector2 inputVector = playerControls.Player.Movement.ReadValue<Vector2>();
        Vector3 movementDir = new Vector3(inputVector.x, 0, inputVector.y);
        MoveCharacter(inputVector, movementDir, playerCamera.GetCameraFoward());
    }

    public void MoveCharacter(Vector3 velocity, Vector3 inputDirection, Vector3 movementDir)
    {
        Debug.Log(velocity);
        Vector3 relativeDirection = GetRelativeLookDirection(inputDirection);

        characterController.Move(transform.TransformDirection(relativeDirection) * currentSpeed * Time.deltaTime);
    }

    public void MoveGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;

        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public Vector3 GetRelativeLookDirection(Vector3 inputDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
        targetRotation.y = 0;
        targetRotation = targetRotation.normalized;
        Vector3 relativeDirection = targetRotation * inputDirection;

        return relativeDirection;
    }
}
