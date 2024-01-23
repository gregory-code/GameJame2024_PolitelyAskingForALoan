using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class npcBase : MonoBehaviour
{
    Player player;

    [Header("Genearl Info")]
    [SerializeField] string npcName;
    [SerializeField] Color npcColor;
    [SerializeField] NavMeshAgent agent;

    [Header("Senses")]
    [SerializeField] float sightRange = 20f;
    [SerializeField] float eyeHeight = 0.65f;
    [SerializeField] float halfPeripheralAngle = 75f;
    [SerializeField] float attentionSpanInSeconds;

    private bool bTalking;
    private bool bFoundPlayer;
    private bool bDead;
    private Transform playerLocation;

    public delegate void OnDeath(Vector3 shotDirection, Rigidbody shotRigidbody);
    public event OnDeath onDeath;

    public void GetHit(Vector3 shotDirection, Rigidbody shotRigidbody, bool headShot)
    {
        if(headShot)
        {
            bDead = true;
            agent.isStopped = true;
            GetComponent<ChatInteraction>().Disable();
            onDeath?.Invoke(shotDirection, shotRigidbody);
        }
    }

    public string GetName()
    {
        return npcName;
    }

    public Color GetColor()
    {
        return npcColor;
    }

    public void TalkState(Transform playerLocation, bool state)
    {
        bTalking = state;
        this.playerLocation = playerLocation;
    }

    private void Update()
    {
        if (bDead)
            return;

        if (bFoundPlayer && Talking() == false)
        {
            FollowPlayer();
        }
    }

    private void LateUpdate()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (bDead)
            return;

        if (Talking())
        {
            bFoundPlayer = true;
            return;
        }

        if (SeesPlayer())
        {
            bFoundPlayer = true;
        }
        else
        {
            StartCoroutine(LostPlayer());
        }
    }

    private void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);
        agent.isStopped = false;
    }

    private IEnumerator LostPlayer()
    {
        yield return new WaitForSeconds(attentionSpanInSeconds);
        bFoundPlayer = false;
        agent.isStopped = true;
    }

    private bool Talking()
    {
        if (bTalking)
        {
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

        return true;
    }

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
