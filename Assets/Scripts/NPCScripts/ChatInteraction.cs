using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ChatInteraction : MonoBehaviour
{
    [SerializeField] GameObject popupText;
    [SerializeField] GameObject canvasToAttach;
    [SerializeField] Transform attachPoint;
    bool bInRange = false;

    private void Start()
    {
        popupText.transform.SetParent(canvasToAttach.transform);
    }

    private void Update()
    {
        popupText.transform.position = Camera.main.WorldToScreenPoint(attachPoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        popupText.SetActive(true);
        bInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        popupText.SetActive(false);
        bInRange = false;
    }
}
