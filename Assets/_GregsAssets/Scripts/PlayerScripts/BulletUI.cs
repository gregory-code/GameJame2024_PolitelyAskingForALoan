using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUI : MonoBehaviour
{
    [SerializeField] Animator bulletAnimator;

    private bool exists;

    private void Start()
    {
        exists = true;
    }

    public void Reload()
    {
        exists = true;
        bulletAnimator.SetTrigger("Reload");
        bulletAnimator.ResetTrigger("Use");
    }

    public void Use()
    {
        exists = false;
        bulletAnimator.SetTrigger("Use");
        bulletAnimator.ResetTrigger("Reload");
    }

    public bool DoesExist()
    {
        return exists;
    }    
}
