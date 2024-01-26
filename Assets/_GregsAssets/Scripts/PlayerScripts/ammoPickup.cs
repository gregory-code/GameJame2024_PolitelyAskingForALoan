using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    [SerializeField] Animator myAnimator;
    [SerializeField] int pickUpAmount;

    private bool alreadyPicked = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && alreadyPicked == false)
        {
            alreadyPicked = true;
            other.transform.root.transform.Find("player").GetComponent<Player>().PickUpAmmo(pickUpAmount);
            myAnimator.SetTrigger("pickup");
            Destroy(this.gameObject, 0.25f);
        }
    }

}
