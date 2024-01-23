using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Player player;

    private Transform currentTransform;

    [SerializeField] Transform followTransform;
    [SerializeField] Transform regularTransform;
    [SerializeField] Transform adsTransform;

    [SerializeField] GameObject BulletPrefab;
    [SerializeField] GameObject CasingPrefab;

    [SerializeField] Transform shootingSpawn;
    [SerializeField] Transform casingSpawn;

    [SerializeField] GameObject muzzleFlashPrefab;

    [SerializeField] float bulletSpeed;
    [SerializeField] float casingUpSpeed;
    [SerializeField] float casingRightSpeed;
    [SerializeField] float casingSpinSpeed;

    [SerializeField] float accuracy = 0;

    [SerializeField] CanvasGroup crosshairGroup;
    [SerializeField] Crosshair[] crosshairs;

    bool bADS;
    RaycastHit hit;

    private void Start()
    {
        player.onAim += SetOffset;
        player.onBlasting += Fire;

        currentTransform = regularTransform;
    }

    private void Fire()
    {
        RaycastHit hit;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        MuzzleFlash();

        if (Physics.Raycast(raycast, out hit))
        {
            Vector3 inaccuracy = Random.insideUnitSphere * accuracy;
            hit.point += inaccuracy;

            ExjectCasing();

            GameObject bullet = Instantiate(BulletPrefab, shootingSpawn.position, shootingSpawn.rotation);
            bullet.GetComponent<Bullet>().GetRayCastHit(hit);

            Vector3 directionToTarget = (hit.point - shootingSpawn.position).normalized;

            bullet.GetComponent<Rigidbody>().velocity = directionToTarget * bulletSpeed;

            bullet.transform.rotation = Quaternion.LookRotation(directionToTarget);
        }

        foreach (Crosshair cross in crosshairs)
        {
            cross.CrosshairRecoil();
            accuracy += 0.2f;
        }
    }

    private void MuzzleFlash()
    {
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, shootingSpawn);
        Destroy(muzzleFlash, 1);
    }

    private void ExjectCasing()
    {
        GameObject bulletCasing = Instantiate(CasingPrefab, casingSpawn.position, casingSpawn.rotation);

        Rigidbody casingRigidbody = bulletCasing.GetComponent<Rigidbody>();
        casingRigidbody.AddForce(casingSpawn.up * casingUpSpeed, ForceMode.Force);
        casingRigidbody.AddForce(casingSpawn.right * casingRightSpeed, ForceMode.Impulse);

        Vector3 randomSpin = Random.onUnitSphere * casingSpinSpeed;
        casingRigidbody.AddTorque(randomSpin, ForceMode.Impulse);

        Destroy(bulletCasing, 10);
    }

    public void SetOffset(bool ADS)
    {
        bADS = ADS;
        currentTransform = (ADS) ? adsTransform : regularTransform;
    }

    void Update()
    {
        followTransform.transform.position = Vector3.Lerp(followTransform.transform.position, currentTransform.position, 5 * Time.deltaTime);

        accuracy = Mathf.Lerp(accuracy, 0, crosshairs[0].GetCrosshariSpeed() * Time.deltaTime);

        float alphaLerp = (bADS) ? 1 : 0;
        crosshairGroup.alpha = Mathf.Lerp(crosshairGroup.alpha, alphaLerp, 10 * Time.deltaTime);

        //Vector3 fowardTransform = transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(transform.position, fowardTransform * 50, Color.red);
    }
}
