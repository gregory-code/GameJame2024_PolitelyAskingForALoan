using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] GameObject popupText;
    [SerializeField] Transform attachPoint;

    public delegate void OnInteract(Player interactingPlayer);
    public event OnInteract onInteract;

    public ItemBase item;

    bool alreadyPickedUp;

    private Player player;

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
        if (alreadyPickedUp == true)
            return;

        alreadyPickedUp = true;
        player.AddItem(item);
        onInteract?.Invoke(player);
        popupText.SetActive(false);
        Destroy(this.gameObject);
    }
}
