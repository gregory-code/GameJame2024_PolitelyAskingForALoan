using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] Animator HPAnimator;

    private bool exists;

    private void Start()
    {
        exists = true;
    }

    public void Reload()
    {
        exists = true;
        HPAnimator.SetTrigger("Reload");
        HPAnimator.ResetTrigger("Use");
    }

    public void Use()
    {
        exists = false;
        HPAnimator.SetTrigger("Use");
        HPAnimator.ResetTrigger("Reload");
    }

    public bool DoesExist()
    {
        return exists;
    }
}
