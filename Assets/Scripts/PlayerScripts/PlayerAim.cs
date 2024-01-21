using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    RaycastHit hit;

    void Start()
    {
        
    }


    void Update()
    {
        Vector3 fowardTransform = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fowardTransform * 50, Color.red);

        if (Physics.Raycast(transform.position, fowardTransform, out hit, 50))
        {

        }
    }
}
