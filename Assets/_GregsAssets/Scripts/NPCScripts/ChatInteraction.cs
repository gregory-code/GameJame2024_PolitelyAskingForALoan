using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChatInteraction : MonoBehaviour
{
    [SerializeField] GameObject popupText;
    [SerializeField] GameObject canvasToAttach;
    [SerializeField] Transform attachPoint;

    public delegate void OnInteract(Player interactingPlayer);
    public event OnInteract onInteract;

    private bool bDisabled;

    private Player player;

    private void Start()
    {
        popupText.transform.SetParent(canvasToAttach.transform);
    }

    private void Update()
    {
        popupText.transform.position = Camera.main.WorldToScreenPoint(attachPoint.position);
    }

    public void Disable()
    {
        popupText.SetActive(false);
        bDisabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeInteractState(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        ChangeInteractState(other, false);
    }

    private void ChangeInteractState(Collider other, bool shouldEnable)
    {
        if (bDisabled)
            return;

        if (other.tag == "Player")
        {
            popupText.SetActive(shouldEnable);
            GetPlayer(other);

            if(shouldEnable)
            {
                player.onInteract += Interact;
            }
            else
            {
                player.onInteract -= Interact;
            }
        }
    }

    private void GetPlayer(Collider other)
    {
        if (player == null)
        {
            player = other.transform.root.transform.Find("player").GetComponent<Player>();
        }
    }

    private void Interact()
    {
        if (bDisabled)
            return;

        onInteract?.Invoke(player);
    }
}
