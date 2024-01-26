using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPHolder : MonoBehaviour
{
    [SerializeField] HP[] hearts;
    [SerializeField] Player player;

    int health = 3;

    private void Start()
    {
        player.onHealHealth += Heal;
        player.onTakeDamage += TookDamage;
    }

    private void Heal()
    {
        Heal(1);
    }

    private void TookDamage(Vector3 shotDirection, Rigidbody shotRigidbody, bool wouldKill)
    {
        if (wouldKill)
            return;

        foreach (HP heart in hearts)
        {
            if (heart.DoesExist())
            {
                health--;
                heart.Use();
                return;
            }
        }
    }

    public void Heal(int healAmount)
    {
        HP[] newSet = hearts;
        System.Array.Reverse(newSet);

        foreach (HP heart in newSet)
        {
            if (heart.DoesExist() == false && healAmount > 0)
            {
                heart.Reload();
                healAmount--;
            }
        }

        System.Array.Reverse(newSet);
    }
}
