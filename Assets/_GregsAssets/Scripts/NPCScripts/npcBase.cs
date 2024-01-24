using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class npcBase : MonoBehaviour
{
    public Player player;

    [Header("Genearl Info")]
    [SerializeField] string npcName;
    [SerializeField] Color npcColor;
    public NavMeshAgent agent;
    public Animator npcAnimator;

    [Header("Senses")]
    [SerializeField] float sightRange = 20f;
    [SerializeField] float eyeHeight = 0.65f;
    [SerializeField] float halfPeripheralAngle = 80f;
    [SerializeField] float attentionSpanInSeconds = 5;

    [Header("Speed")]
    public float rotateSpeed = 7;

    private bool bTalking;
    public bool bFoundPlayer;
    public bool bDead;
    public bool bPlayerIsDead;
    private Transform playerLocation;

    public delegate void OnHeardThat();
    public event OnHeardThat onHeardThat;

    public delegate void OnSeesGun();
    public event OnSeesGun onSeesGun;

    public delegate void OnDeath(Vector3 shotDirection, Rigidbody shotRigidbody);
    public event OnDeath onDeath;

    public void GetHit(Vector3 shotDirection, Rigidbody shotRigidbody, bool headShot)
    {
        if(headShot)
        {
            bDead = true;
            npcAnimator.enabled = false;
            agent.isStopped = true;
            GetComponent<ChatInteraction>().Disable();
            onDeath?.Invoke(shotDirection, shotRigidbody);
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<Player>();
            if (player != null)
            {
                player.onTakeDamage += PlayerHit;
                player.onBlasting += HeardThat;
                player.onHESGOTAGUN += SeesGun;
            }

        }

        Talking();
    }

    public bool playerHasGun;

    private void SeesGun(bool hasIt)
    {
        playerHasGun = hasIt;
    }

    private void PlayerHit(Vector3 shotDirection, Rigidbody shotRigidbody, bool wouldKill)
    {
        if(wouldKill)
        {
            bPlayerIsDead = true;
        }
    }

    private void HeardThat()
    {
        bFoundPlayer = true;
        onHeardThat?.Invoke();
    }

    public Player GetPlayer()
    {
        return player;
    }

    public string GetName()
    {
        return npcName;
    }

    public float GetAttentionSpan()
    {
        return attentionSpanInSeconds;
    }

    public Color GetColor()
    {
        return npcColor;
    }

    public void TalkState(Transform playerLocation, bool state)
    {
        bTalking = state;
        this.playerLocation = playerLocation;
        npcAnimator.SetBool("talking", state);
    }

    public bool Talking()
    {
        if (bTalking)
        {
            agent.isStopped = true;

            Vector3 lookDirection = playerLocation.position - transform.position;
            lookDirection.y = 0;

            Quaternion rotate = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 3 * Time.deltaTime);
            return true;
        }

        return false;
    }

    public bool SeesPlayer()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > sightRange)
        {
            return false;
        }

        Vector3 stimuliDir = (player.transform.position - transform.position).normalized;
        Vector3 ownerForward = transform.forward;

        if (Vector3.Angle(stimuliDir, ownerForward) > halfPeripheralAngle)
        {
            return false;
        }

        if (Physics.Raycast(transform.position + Vector3.up * eyeHeight, stimuliDir, out RaycastHit hitInfo, sightRange))
        {
            if (hitInfo.collider.gameObject != player.gameObject)
            {
                return false;
            }
        }

        if (playerHasGun && seenGun == false)
        {
            seenGun = true;
            onSeesGun?.Invoke();
        }
        return true;
    }

    public bool seenGun;

    private void OnDrawGizmos()
    {
        Vector3 drawCenter = transform.position + Vector3.up * eyeHeight;
        Gizmos.DrawWireSphere(drawCenter, sightRange);

        Vector3 leftDir = Quaternion.AngleAxis(halfPeripheralAngle, Vector3.up) * transform.forward;
        Vector3 rightDir = Quaternion.AngleAxis(-halfPeripheralAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(drawCenter, drawCenter + leftDir * sightRange);
        Gizmos.DrawLine(drawCenter, drawCenter + rightDir * sightRange);
    }
}
