using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class copCat : npcBase
{
    [SerializeField] float rangeToShoot = 9;
    [SerializeField] float tooClose = 6;

    [SerializeField] float shootingSpeed;
    private float currentshoot;

    [SerializeField] Transform shootSpawn;
    [SerializeField] Transform casingSpawn;

    [SerializeField] GameObject BulletPrefab;
    [SerializeField] GameObject CasingPrefab;

    [SerializeField] GameObject muzzleFlashPrefab;

    [SerializeField] float bulletSpeed;

    [SerializeField] float casingUpSpeed;
    [SerializeField] float casingRightSpeed;
    [SerializeField] float casingSpinSpeed;

    bool bHighAlert;

    public void Start()
    {
        onHeardThat += CopCatAlert;
        agent.stoppingDistance = 1;
    }

    private void CopCatAlert()
    {
        StopAllCoroutines();
        CheckHighAlert();
    }

    private void Update()
    {
        if (bDead || bPlayerIsDead)
            return;

        if (bFoundPlayer && Talking() == false)
        {
            FollowPlayer();
        }
    }

    private void CheckHighAlert()
    {
        if(bHighAlert == false)
        {
            bHighAlert = true;
            npcAnimator.SetBool("highAlert", true);
        }
    }

    private void LateUpdate()
    {
        if (bDead)
            return;

        if (bPlayerIsDead)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            return;
        }

        if (Talking())
        {
            bFoundPlayer = true;
            return;
        }

        if (SeesPlayer() && bHighAlert)
        {
            bFoundPlayer = true;
            StopAllCoroutines();
        }
        else if(bHighAlert)
        {
            StartCoroutine(LostPlayer());
        }
    }

    private void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < (rangeToShoot + 0.5f) && SeesPlayer())
            ShootAtPlayer();

        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0;

        Quaternion rotate = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);

        if (distance < rangeToShoot && distance > tooClose)
        {
            agent.isStopped = true;
        }
        else if(distance > rangeToShoot)
        {
            agent.SetDestination(player.transform.position);
            agent.isStopped = false;
        }
        else
        {
            Vector3 behindPos = transform.position - (transform.forward * 10);
            agent.SetDestination(behindPos);
            agent.isStopped = false;
        }
    }

    private void ShootAtPlayer()
    {
        currentshoot = Mathf.Lerp(currentshoot, 0, shootingSpeed * Time.deltaTime);
        if(currentshoot <= 0.1f)
        {
            currentshoot = 100;
            MuzzleFlash();
            ExjectCasing();

            GameObject bullet = Instantiate(BulletPrefab, shootSpawn.position, shootSpawn.rotation);

            Vector3 directionToTarget = (player.GetPlayerHead().position - shootSpawn.position).normalized;

            bullet.GetComponent<Rigidbody>().velocity = directionToTarget * bulletSpeed;

            bullet.transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    private void MuzzleFlash()
    {
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, shootSpawn);
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


    private IEnumerator LostPlayer()
    {
        yield return new WaitForSeconds(GetAttentionSpan());
        agent.isStopped = true;
        bFoundPlayer = false;
    }

}
