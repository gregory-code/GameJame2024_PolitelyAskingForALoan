using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class winBox : MonoBehaviour
{

    public GameObject winEffect;
    bool canWin;

    bool hasWon;

    [SerializeField] CanvasGroup winScreenGroup;

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
            Time.timeScale = 0;
            winScreenGroup.interactable = true;
            winScreenGroup.blocksRaycasts = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            hasWon = true;
        }
    }

    private void Update()
    {
        float lerp = (hasWon) ? 1 : 0 ;
        winScreenGroup.alpha = Mathf.Lerp(winScreenGroup.alpha, lerp, 0.2f);
    }
}
