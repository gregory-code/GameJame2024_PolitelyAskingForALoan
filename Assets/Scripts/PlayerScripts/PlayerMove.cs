using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Player myPlayer;

    [SerializeField] float gravity = -20f;
    [SerializeField] float currentSpeed = 3f;
    [SerializeField] float rotateSpeed = 5f;

    Vector3 playerVelocity;

    private void Start()
    {
        myPlayer.onMoveInput += MoveInput;
        myPlayer.onMouseHoverInput += RotateInput;
    }

    private void Update()
    {
        MoveGravity();
    }

    private void RotateInput(Vector3 hitVector)
    {
        Vector3 lookDirection = hitVector - transform.position;
        lookDirection.y = 0;

        Quaternion rotate = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);
    }

    private void MoveInput(Vector2 inputVector, Vector3 cameraFoward)
    {
        Debug.Log(inputVector);
        characterController.Move(GetMoveDir(inputVector) * currentSpeed * Time.deltaTime);
    }

    private Vector3 GetMoveDir(Vector2 inputDirection)
    {
        Vector3 rightDir = Camera.main.transform.right;
        Vector3 upDir = Vector3.Cross(rightDir, Vector3.up);

        return (rightDir * inputDirection.x + upDir * inputDirection.y).normalized;
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
}
