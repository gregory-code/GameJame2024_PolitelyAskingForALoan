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
            other.transform.root.GetComponent<npcBase>().GetHit(transform.forward, other.GetComponent<Rigidbody>(), true);
            GameObject sparkle = Instantiate(headShotSparklePrefab, transform.position, transform.rotation);
            Destroy(sparkle, 3);

            Destroy(this.gameObject);
        }

        if (other.tag == "Body")
        {
            other.transform.root.GetComponent<npcBase>().GetHit(transform.forward, other.GetComponent<Rigidbody>(), false);
            GameObject body = Instantiate(bodyShotSparklePrefab, transform.position, transform.rotation);
            Destroy(body, 3);

            Destroy(this.gameObject);
        }

        if (other.tag == "Environment")
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, transform.position, transform.rotation);
            Destroy(bulletHole, 12);

            Destroy(this.gameObject);
        }
    }
}
