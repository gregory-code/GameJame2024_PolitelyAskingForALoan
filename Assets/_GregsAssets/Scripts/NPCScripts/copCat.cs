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

    [SerializeField] ChatInteraction myInteraction;
    [SerializeField] TalkBox firstTalk;

    public Transform[] guardPoints;

    public void GoToGuard()
    {
        int randomPoint = Random.Range(0, guardPoints.Length);
        agent.destination = guardPoints[randomPoint].position;
        bHighAlert = true;
    }

    private void StartTalking(Player interactingPlayer)
    {
        if (interactingPlayer.IsInChat())
            return;

        TalkState(true);
        GameObject.FindFirstObjectByType<ChatEngine>().StartChat(this, this.transform, GetName(), firstTalk, GetColor());
    }

    float animSpeed;
    float desiredSpeed;

    bool bHighAlert;

    public void Start()
    {
        myInteraction.onInteract += StartTalking;
        desiredSpeed = 0;
        onHeardThat += CopCatAlert;
        onSeesGun += CopCatAlert;
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

        desiredSpeed = agent.velocity.magnitude;
        animSpeed = Mathf.Lerp(animSpeed, desiredSpeed, 5 * Time.deltaTime);
        npcAnimator.SetFloat("speed", animSpeed);

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
            return;
        }

        if (player == null)
        {
            FetchPlayer();
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

        bool seesPlayer = SeesPlayer();

        if (distance < (rangeToShoot + 2f) && seesPlayer)
            ShootAtPlayer();

        LookAtPlayer(rotateSpeed);

        if (distance < rangeToShoot && distance > tooClose && seesPlayer)
        {
            agent.isStopped = true;
        }
        else if(distance > rangeToShoot || seesPlayer == false)
        {
            agent.SetDestination(player.transform.position);
            agent.isStopped = false;
        }
        else if(seesPlayer)
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
            npcAnimator.SetTrigger("shoot");

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
