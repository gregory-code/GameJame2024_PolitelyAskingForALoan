using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Player player;

    private Transform currentTransform;

    [SerializeField] Transform followTransform;
    [SerializeField] Transform regularTransform;
    [SerializeField] Transform adsTransform;

    RaycastHit hit;

    private void Start()
    {
        player.onAim += SetOffset;
        currentTransform = regularTransform;
    }

    public void SetOffset(bool ADS)
    {
        currentTransform = (ADS) ? adsTransform : regularTransform ;
    }

    void Update()
    {
        followTransform.transform.position = Vector3.Lerp(followTransform.transform.position, currentTransform.position, 5 * Time.deltaTime);

        Vector3 fowardTransform = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fowardTransform * 50, Color.red);

        if (Physics.Raycast(transform.position, fowardTransform, out hit, 50))
        {

        }
    }
}
