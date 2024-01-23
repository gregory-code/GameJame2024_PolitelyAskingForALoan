using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    private Rigidbody[] _rigidBodies;
    [SerializeField] float ragdollForce;

    [SerializeField] npcBase npcbase;

    private void Start()
    {
        npcbase.onDeath += TargetHit;
        DisableRagdoll();
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
