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

    bool neeko;
    bool alearted;

    void Start()
    {
        myInteraction.onInteract += StartTalking;
        onHeardThat += SeesGun;
        onSeesGun += SeesGun;
        currentTalk = firstTalk;
    }

    private void SeesGun()
    {
        currentTalk = gunTalk;
        StartCoroutine(waitRun());
    }

    private IEnumerator waitRun()
    {
        alearted = true;
        yield return new WaitForSeconds(1f);
        alearted = false;
        neeko = true;
        npcAnimator.SetTrigger("panic");
        escape = GameObject.Find("escapePoint").GetComponent<Transform>();
        agent.destination = escape.position;
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

        if (neeko == false)
            return;

        agent.destination = escape.position;
        float distance = Vector3.Distance(player.transform.position, escape.position);
        if (distance <= 1)
            Destroy(this.gameObject);
    }
}
