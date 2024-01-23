using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] GameObject headShotSparklePrefab;
    [SerializeField] GameObject bodyShotSparklePrefab;

    private RaycastHit raycastHitPoint;

    public void GetRayCastHit(RaycastHit hit)
    {
        raycastHitPoint = hit;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Head")
        {
            other.transform.root.GetComponent<RagDoll>().TargetHit(transform.forward, other.GetComponent<Rigidbody>());
            GameObject sparkle = Instantiate(headShotSparklePrefab, raycastHitPoint.point, transform.rotation);
            Destroy(sparkle, 3);

            Destroy(this.gameObject);
        }

        if (other.tag == "Body")
        {
            GameObject body = Instantiate(bodyShotSparklePrefab, raycastHitPoint.point, transform.rotation);
            Destroy(body, 3);

            Destroy(this.gameObject);
        }

        if (other.tag == "Environment")
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, raycastHitPoint.point, transform.rotation);
            Destroy(bulletHole, 12);

            Destroy(this.gameObject);
        }
    }
}
