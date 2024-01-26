using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPoints : MonoBehaviour
{
    [SerializeField] Transform[] guardPoints;
    public Transform[] GetGuardPoints()
    {
        return guardPoints;
    }
}
