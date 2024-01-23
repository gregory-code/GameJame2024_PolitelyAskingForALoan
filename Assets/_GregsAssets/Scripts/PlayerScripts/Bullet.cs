using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bulletHolePrefab;

    private RaycastHit raycastHitPoint;

    public void GetRayCastHit(RaycastHit hit)
    {
        raycastHitPoint = hit;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment")
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, raycastHitPoint.point, transform.rotation);
            Destroy(bulletHole, 12);

            Destroy(this.gameObject);
        }
    }
}
