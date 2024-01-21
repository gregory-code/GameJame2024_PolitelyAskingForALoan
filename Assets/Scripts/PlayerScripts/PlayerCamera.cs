using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform followTransform;
    [SerializeField] float armLength;
    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform cameraArm;
    [SerializeField] float turnSpeed;

    [SerializeField][Range(0, 1)] float followDamping;

    public Vector3 GetCameraFoward()
    {
        return cameraTrans.forward;
    }

    private void LateUpdate()
    {
        cameraTrans.position = cameraArm.position - cameraTrans.forward * armLength;

        transform.position = Vector3.Lerp(transform.position, followTransform.position, (1 - followDamping) * Time.deltaTime * 20f);
    }
}
