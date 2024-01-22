using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Player myPlayer;

    [SerializeField] float gravity = -20f;
    [SerializeField] float currentSpeed = 3f;
    [SerializeField] float rotateSpeed = 5f;


    private bool bAiming = false;
    Vector3 playerVelocity;

    private void Start()
    {
        myPlayer.onAim += Aiming;
        myPlayer.onMoveInput += MoveInput;
        myPlayer.onRotate += RotateToTarget;
    }

    private void Aiming(bool state)
    {
        bAiming = state;
    }

    private void Update()
    {
        MoveGravity();
        AimRotate();
    }

    private void AimRotate()
    {
        if (bAiming)
        {
            Vector3 cameraFoward = Camera.main.transform.forward;

            cameraFoward.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(cameraFoward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (rotateSpeed * 2) * Time.deltaTime);
        }
    }

    private void RotateToTarget(Vector3 targetVector)
    {
        Vector3 lookDirection = targetVector - transform.position;
        lookDirection.y = 0;

        Quaternion rotate = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);
    }

    private void RotateToDir(Vector3 inputVector, Vector3 cameraFoward)
    {
        if (bAiming)
            return;

        Vector3 movementDir = new Vector3(inputVector.x, 0, inputVector.y);

        cameraFoward.y = 0;
        cameraFoward = cameraFoward.normalized;
        Vector3 relativeDirection = Quaternion.LookRotation(cameraFoward) * movementDir;
        Quaternion targetRotation = Quaternion.LookRotation(relativeDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void MoveInput(Vector2 inputVector, Vector3 cameraFoward, bool bRotateThatDir)
    {
        if(bRotateThatDir)
            RotateToDir(inputVector, cameraFoward);

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
