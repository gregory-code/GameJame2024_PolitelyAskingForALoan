using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Player player;

    private Transform currentTransform;

    [SerializeField] Transform followTransform;
    [SerializeField] Transform regularTransform;
    [SerializeField] Transform adsTransform;

    [SerializeField] CanvasGroup crosshairGroup;
    [SerializeField] Crosshair[] crosshairs;

    bool bADS;
    RaycastHit hit;

    private void Start()
    {
        player.onAim += SetOffset;
        player.onBlasting += MoveCrosshairs;
        currentTransform = regularTransform;
    }

    private void MoveCrosshairs()
    {
        foreach(Crosshair cross in crosshairs)
        {
            cross.CrosshairRecoil();
        }
    }

    public void SetOffset(bool ADS)
    {
        bADS = ADS;
        currentTransform = (ADS) ? adsTransform : regularTransform;
    }

    void Update()
    {
        followTransform.transform.position = Vector3.Lerp(followTransform.transform.position, currentTransform.position, 5 * Time.deltaTime);

        float alphaLerp = (bADS) ? 1 : 0;
        crosshairGroup.alpha = Mathf.Lerp(crosshairGroup.alpha, alphaLerp, 10 * Time.deltaTime);

        /*Vector3 fowardTransform = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fowardTransform * 50, Color.red);

        if (Physics.Raycast(transform.position, fowardTransform, out hit, 50))
        {

        }*/
    }
}
