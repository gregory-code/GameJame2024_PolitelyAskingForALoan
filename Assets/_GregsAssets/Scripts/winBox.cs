using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winBox : MonoBehaviour
{

    public GameObject winEffect;
    bool canWin;

    public void CanWin()
    {
        canWin = true;
        winEffect.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canWin)
        {
            Debug.Log("You win");
        }
    }
}
