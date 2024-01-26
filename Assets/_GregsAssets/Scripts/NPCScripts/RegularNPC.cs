using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularNPC : npcBase
{
    [SerializeField] ChatInteraction myInteraction;
    [SerializeField] TalkBox firstTalk;
    [SerializeField] TalkBox gunTalk;

    Transform escape;

    TalkBox currentTalk;

    [SerializeField] bool neeko;
    [SerializeField] bool alearted;

    [SerializeField] bool waitingInLine;
    bool served;
    int positionInLine = -1;

    [SerializeField] Transform[] spotsInLine;

    void Start()
    {
        escape = GameObject.Find("escapePoint").GetComponent<Transform>();
        myInteraction.onInteract += StartTalking;
        onHeardThat += SeesGun;
        onSeesGun += SeesGun;
        currentTalk = firstTalk;

        if(waitingInLine)
        {
            StartCoroutine(WaitingInLine());
        }
    }

    private IEnumerator WaitingInLine()
    {
        while(served == false)
        {
            yield return new WaitForSeconds(15);
            positionInLine++;

            if(agent != null)
                agent.destination = spotsInLine[positionInLine].position;

            if (positionInLine == spotsInLine.Length - 1)
            {
                served = true;
            }

        }
    }

    private void SeesGun()
    {
        if (alearted == true)
            return;

        if (neeko == true)
        {
            escape = GameObject.Find("escapePoint").GetComponent<Transform>();
            if(agent != null)
            {
                agent.destination = escape.position;
            }
            return;
        }

        alearted = true;

        currentTalk = gunTalk;
        StartCoroutine(waitRun());
    }

    private IEnumerator waitRun()
    {
        alearted = true;
        yield return new WaitForSeconds(1f);
        alearted = false;
        npcAnimator.SetTrigger("panic");
        agent.destination = escape.position;
        neeko = true;
    }

    private void StartTalking(Player interactingPlayer)
    {
        if (interactingPlayer.IsInChat())
            return;

        TalkState(true);
        GameObject.FindFirstObjectByType<ChatEngine>().StartChat(this, this.transform, GetName(), currentTalk, GetColor());
    }

    public void Update()
    {
        if(alearted)
            LookAtPlayer(rotateSpeed);

        if(bDead)
        {

            agent.isStopped = true;
            return;
        }

        if (neeko == true && bDead == false)
        {
            agent.isStopped = false;
            agent.destination = escape.position;
            float distance = Vector3.Distance(transform.position, escape.position);
            if (distance <= 2)
                Destroy(this.gameObject);
        }
    }
}
