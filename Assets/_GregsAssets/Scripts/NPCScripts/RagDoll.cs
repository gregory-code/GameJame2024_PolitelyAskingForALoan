using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RagDoll : MonoBehaviour
{
    private Rigidbody[] _rigidBodies;
    [SerializeField] float ragdollForce;

    [SerializeField] npcBase npcbase;
    [SerializeField] Player player;

    [SerializeField] bool bIsNPC;

    private void Start()
    {
        if (!bIsNPC)
        {
            npcbase.onDeath += TargetHit;

        }
        else
        {
            player.onTakeDamage += Damaged;

        }

        DisableRagdoll();
    }

    private void Damaged(Vector3 shotDirection, Rigidbody shotRigidbody, bool wouldKill)
    {
        if(wouldKill)
        {
            EnableRagdoll();
            shotRigidbody.AddForce(shotDirection.normalized * ragdollForce, ForceMode.Impulse);
        }
    }

    public void TargetHit(Vector3 shotDirection, Rigidbody shotRigidbody)
    {
        EnableRagdoll();
        shotRigidbody.AddForce(shotDirection.normalized * ragdollForce, ForceMode.Impulse);
    }

    private Rigidbody[] rigidbodies
    {
        get
        {
            if(_rigidBodies == null)
            {
                _rigidBodies = GetComponentsInChildren<Rigidbody>();
            }

            return _rigidBodies;
        }
    }

    public void EnableRagdoll()
    {
        foreach(var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    public void DisableRagdoll()
    {
        foreach(var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = true;
        }
    }
}
