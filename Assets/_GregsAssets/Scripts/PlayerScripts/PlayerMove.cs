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

    [SerializeField] Animator playerAnimator;

    [SerializeField] float gravity = -20f;
    [SerializeField] float currentSpeed = 3f;
    [SerializeField] float rotateSpeed = 5f;

    private bool bDead;
    private bool hadGun;

    private bool bAiming = false;
    Vector3 playerVelocity;

    private void Start()
    {
        myPlayer.onSelectItem += GetItem;
        myPlayer.onTakeDamage += TookDamage;
        myPlayer.onAim += Aiming;
        myPlayer.onMoveInput += MoveInput;
        myPlayer.onRotate += RotateToTarget;
    }

    private void GetItem(int itemID)
    {
        hadGun = (itemID == 1) ? true : false ;
    }

    private void TookDamage(Vector3 shotDirection, Rigidbody shotRigidbody, bool wouldKill)
    {
        if(wouldKill)
        {
            bDead = true;
        }
    }

    private void Aiming(bool state)
    {
        bAiming = state;
    }

    private void Update()
    {
        if (bDead)
            return;

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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (rotateSpeed * 1.5f) * Time.deltaTime);
    }

    private void UpdateAnimator(Vector3 rawInputs)
    {
        //float rightSpeed = Vector3.Dot(moveDir, transform.right);
        //float forwardSpeed = Vector3.Dot(moveDir, transform.forward);

        float forwardSpeed = rawInputs.x;
        float rightSpeed = rawInputs.y;

        if (bAiming == false)
        {
            playerAnimator.SetLayerWeight(1, 0);
            forwardSpeed = 0;
            rightSpeed = 1;
        }

        float lerpRight = Mathf.Lerp(playerAnimator.GetFloat("leftSpeed"), forwardSpeed, 7 * Time.deltaTime);
        float lerpFoward = Mathf.Lerp(playerAnimator.GetFloat("fowardSpeed"), rightSpeed, 7 * Time.deltaTime);

        playerAnimator.SetFloat("leftSpeed", lerpRight);
        playerAnimator.SetFloat("fowardSpeed", lerpFoward);
    }

    private void MoveInput(Vector2 inputVector, Vector3 cameraFoward, bool bRotateThatDir)
    {
        if(bRotateThatDir)
            RotateToDir(inputVector, cameraFoward);

        UpdateAnimator(inputVector);

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
