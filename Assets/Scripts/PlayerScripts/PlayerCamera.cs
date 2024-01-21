using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform followTransform;
    [SerializeField] Transform lookAtTransform;

    [SerializeField] Transform cameraYaw;
    [SerializeField] Transform cameraPitch;

    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform cameraArm;

    [SerializeField] float armLength;
    [SerializeField][Range(0, 1)] float followDamping;

    [SerializeField] float horizontalRotSpeed;
    [SerializeField] float verticalRotSpeed;

    float pitch;
    float yaw;

    private bool bIsActive = true;

    public Vector3 GetCameraFoward()
    {
        return cameraTrans.forward;
    }

    public void HandleLook(Vector2 look)
    {
        RotateYaw(look.x);
        RotatePitch(look.y);
        CameraFollow();

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
        pitch = Mathf.Clamp(pitch, -10, 50);
        cameraPitch.x = pitch;
        this.cameraPitch.localEulerAngles = cameraPitch;
    }

    private void CameraFollow()
    {
        cameraTrans.position = cameraArm.position - cameraTrans.forward * armLength;

        transform.position = Vector3.Lerp(transform.position, followTransform.position, (1 - followDamping) * Time.deltaTime * 20f);
    }
}
