using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Door : MonoBehaviour
{
    [SerializeField] GameObject popupText;
    [SerializeField] Transform attachPoint;

    [SerializeField] BoxCollider box;

    public delegate void OnInteract(Player interactingPlayer);
    public event OnInteract onInteract;

    private Player player;

    [SerializeField] bool lockedDoor;
    [SerializeField] Animator doorAnimator;

    bool opened;

    private void Start()
    {
        popupText.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    private void Update()
    {
        popupText.transform.position = Camera.main.WorldToScreenPoint(attachPoint.position);
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

        if (other.tag == "Player")
        {
            popupText.SetActive(shouldEnable);
            GetPlayer(other);

            if (shouldEnable)
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
        if(player.GetCurrentItemID() == 7)
        {
            opened = !opened;
            doorAnimator.SetBool("Open", opened);
            popupText.SetActive(false);
            box.enabled = false;

            onInteract?.Invoke(player);
        }
        else
        {
            GameObject.FindFirstObjectByType<Notification>().CreateNotification("Missing Vault Key!", Color.red, key);
        }
    }

    [SerializeField] Sprite key;
}
