using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] GameObject hitPlayerPrefab;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.root.transform.Find("player").GetComponent<Player>().TakeDamage(transform.forward, other.GetComponent<Rigidbody>());
            GameObject body = Instantiate(hitPlayerPrefab, transform.position, transform.rotation);
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
