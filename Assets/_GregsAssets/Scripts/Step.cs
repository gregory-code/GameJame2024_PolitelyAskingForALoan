using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    [SerializeField] GameObject smokeStep;
    [SerializeField] Transform baseTransform;

    private void SmokeStep()
    {
        GameObject step = Instantiate(smokeStep, baseTransform.position, baseTransform.rotation);
        Destroy(step, 1);
    }
}
