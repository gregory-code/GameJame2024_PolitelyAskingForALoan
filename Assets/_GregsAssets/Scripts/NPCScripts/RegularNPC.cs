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

    void Start()
    {
        escape = GameObject.Find("escapePoint").GetComponent<Transform>();
        myInteraction.onInteract += StartTalking;
        onHeardThat += SeesGun;
        onSeesGun += SeesGun;
        currentTalk = firstTalk;
    }

    private void SeesGun()
    {
        if (alearted == true)
            return;

        if (neeko == true)
        {
            agent.destination = escape.position;
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
        TalkState(true);
        GameObject.FindFirstObjectByType<ChatEngine>().StartChat(this, this.transform, GetName(), currentTalk, GetColor());
    }

    public void Update()
    {
        if(alearted)
            LookAtPlayer(rotateSpeed);

        if (neeko == true)
        {
            agent.isStopped = false;
            agent.destination = escape.position;
            float distance = Vector3.Distance(transform.position, escape.position);
            if (distance <= 2)
                Destroy(this.gameObject);
        }
    }
}
