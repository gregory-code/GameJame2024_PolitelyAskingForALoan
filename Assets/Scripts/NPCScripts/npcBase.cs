using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcBase : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] Color npcColor;

    private bool bTalking;
    private Transform playerLocation;

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

    private void LateUpdate()
    {
        if(bTalking)
        {
            Vector3 lookDirection = playerLocation.position - transform.position;
            lookDirection.y = 0;

            Quaternion rotate = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 3 * Time.deltaTime);
        }
    }
}
