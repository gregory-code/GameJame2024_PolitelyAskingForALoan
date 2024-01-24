using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField] BoxCollider box;

    private void Start()
    {
        StartCoroutine(EnableColliders());
    }

    private IEnumerator EnableColliders()
    {
        yield return new WaitForSeconds(0.2f);
        box.isTrigger = false;
    }
}
