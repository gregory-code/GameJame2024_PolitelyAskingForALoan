using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class copCat : npcBase
{
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
            StopAllCoroutines();
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
        yield return new WaitForSeconds(GetAttentionSpan());
        bFoundPlayer = false;
        agent.isStopped = true;
    }

}
