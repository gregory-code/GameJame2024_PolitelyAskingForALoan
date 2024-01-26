using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject exlposion;
    public float delay;
    public float radius;

    public void Start()
    {
        StartCoroutine(explodeDelay());
    }

    private IEnumerator explodeDelay()
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    public void Explode()
    {
        Instantiate(exlposion, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach(Collider objects in colliders)
        {

            if(objects.transform.root.tag == "EnemyRoot")
            {
                npcBase enemy = objects.transform.root.GetComponent<npcBase>();
                enemy.GetHit(transform.forward, enemy.leg, true);
            }
        }

        Destroy(this.gameObject);
    }
}
