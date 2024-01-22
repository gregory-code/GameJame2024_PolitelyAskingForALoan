using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 origionalLocation;

    private void Start()
    {
        origionalLocation = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, origionalLocation, speed * Time.deltaTime);
    }

    public void CrosshairRecoil()
    {
        transform.localPosition *= 1.2f;
    }
}
