using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayWith : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(target.position - offset, transform.rotation);
    }
}
