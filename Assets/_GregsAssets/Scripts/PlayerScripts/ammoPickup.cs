using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    [SerializeField] Animator myAnimator;
    [SerializeField] int pickUpAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.GetComponent<Player>().PickUpAmmo(pickUpAmount);
            myAnimator.SetTrigger("pickup");
            Destroy(this.gameObject, 0.25f);
        }
    }

}
