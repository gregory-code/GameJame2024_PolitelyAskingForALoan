using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
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

    [SerializeField] float clampMinRegular;
    [SerializeField] float clampMaxRegular;

    [SerializeField] float clampMinADS;
    [SerializeField] float clampMaxADS;

    float clampMin;
    float clampMax;

    bool bAiming = false;

    public void Start()
    {
        clampMin = clampMinRegular;
        clampMax = clampMaxRegular;
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

        clampMin = state ? clampMinADS : clampMinRegular;
        clampMax = state ? clampMaxADS : clampMaxRegular;
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
        pitch = Mathf.Clamp(pitch, clampMin, clampMax);
        cameraPitch.x = pitch;
        this.cameraPitch.localEulerAngles = cameraPitch;
    }

    private void CameraFollow()
    {
        cameraTrans.position = cameraArm.position - cameraTrans.forward * currentArmLength;
        float speed = (bAiming) ? 5 : (1 - followDamping);
        transform.position = Vector3.Lerp(transform.position, followTransform.position, speed * Time.deltaTime * 20f);
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
            desiredLength = closeArmLength / 2f;
            speed *= 2;
        }

        currentArmLength = Mathf.Lerp(currentArmLength, desiredLength, speed * Time.deltaTime);
    }
}
