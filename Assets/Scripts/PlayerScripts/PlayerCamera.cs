using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] Transform followTransform;
    [SerializeField] Transform playerFollow;

    [SerializeField] Transform cameraYaw;
    [SerializeField] Transform cameraPitch;

    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform cameraArm;

    [SerializeField] float closeArmLength;
    [SerializeField] float farArmLength;
    float currentArmLength;

    [SerializeField][Range(0, 1)] float followDamping;

    [SerializeField] float horizontalRotSpeed;
    [SerializeField] float verticalRotSpeed;

    float pitch;
    float yaw;

    bool bAiming = false;

    public void Start()
    {
        player.onAim += AimInput;
        currentArmLength = closeArmLength;
    }

    public void Update()
    {
        CameraFollow();
        LerpCameraLength();
    }

    private void AimInput(bool state)
    {
        bAiming = state;
    }

    public Vector3 GetCameraFoward()
    {
        return cameraTrans.forward;
    }

    public void HandleLook(Vector2 look)
    {
        RotateYaw(look.x);
        RotatePitch(look.y);

    }

    private void RotateYaw(float lookInputX)
    {
        Vector3 cameraYaw = this.cameraYaw.localEulerAngles;
        yaw += lookInputX * Time.deltaTime * horizontalRotSpeed;
        cameraYaw.y = yaw;
        this.cameraYaw.localEulerAngles = cameraYaw;
    }

    private void RotatePitch(float lookInputY)
    {
        Vector3 cameraPitch = this.cameraPitch.localEulerAngles;
        pitch -= lookInputY * Time.deltaTime * verticalRotSpeed;
        pitch = Mathf.Clamp(pitch, -5, 60);
        cameraPitch.x = pitch;
        this.cameraPitch.localEulerAngles = cameraPitch;
    }

    private void CameraFollow()
    {
        cameraTrans.position = cameraArm.position - cameraTrans.forward * currentArmLength;

        transform.position = Vector3.Lerp(transform.position, followTransform.position, (1 - followDamping) * Time.deltaTime * 20f);
    }

    private void LerpCameraLength()
    {
        float speed = (player.IsInChat()) ? 20 : 5;
        float desiredLength = 0;

        if (player.IsInChat() == false && bAiming == false)
        {
            float dotProduct = Vector3.Dot(player.transform.forward, cameraTrans.forward);
            desiredLength = (dotProduct <= 0.1f) ? farArmLength : closeArmLength;
            followTransform = playerFollow;
        }
        else if (player.IsInChat() == true)
        {
            desiredLength = closeArmLength / 1.25f;
            followTransform = player.GetTargetNPC().Find("talkTransform");
        }
        else if (bAiming)
        {
            desiredLength = closeArmLength / 2.5f;
        }

        currentArmLength = Mathf.Lerp(currentArmLength, desiredLength, speed * Time.deltaTime);
    }
}
