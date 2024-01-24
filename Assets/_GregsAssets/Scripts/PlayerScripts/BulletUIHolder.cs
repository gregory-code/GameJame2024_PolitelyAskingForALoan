using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletUIHolder : MonoBehaviour
{
    [SerializeField] BulletUI[] bullets;
    [SerializeField] Player player;

    [SerializeField] GameObject UIHolder;

    private bool bShowAmmo;

    [SerializeField] TextMeshProUGUI ammoReservesText;
    int ammoReserves = 5;

    private void Start()
    {
        player.onSelectItem += SelectItem;
        player.onAddAmmo += AddAmmo;
        player.onBlasting += UseBullet;
        player.onReload += ReloadBullets;
    }

    private void AddAmmo(int amount)
    {
        ammoReserves += amount;
        ammoReservesText.text = "x" + ammoReserves + "";
    }

    private void SelectItem(int itemID)
    {
        if(itemID == 1)
        {
            bShowAmmo = true;
        }
        else
        {
            bShowAmmo = false;
        }
    }

    private void ReloadBullets(int amountReloading)
    {
        StartCoroutine(ReloadWait(amountReloading));
    }

    private IEnumerator ReloadWait(int AmountReloading)
    {
        BulletUI[] newSet = bullets;
        System.Array.Reverse(newSet);

        foreach (BulletUI bullet in newSet)
        {
            if (bullet.DoesExist() == false && AmountReloading > 0)
            {
                bullet.Reload();
                ammoReserves--;
                ammoReservesText.text = "x" + ammoReserves + "";
                AmountReloading--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        System.Array.Reverse(newSet);
    }

    private void UseBullet()
    {
        foreach(BulletUI bullet in bullets)
        {
            if(bullet.DoesExist())
            {
                bullet.Use();
                return;
            }
        }
    }

    private void Update()
    {
        Vector3 lerpPos = UIHolder.transform.localPosition;
        lerpPos.x = (bShowAmmo) ? 600 : 1350 ;

        UIHolder.transform.localPosition = Vector3.Lerp(UIHolder.transform.localPosition, lerpPos, 7 * Time.deltaTime);
    }
}
