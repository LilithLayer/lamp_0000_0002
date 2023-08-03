using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void FixedUpdate()
    {
        Vector3 desiredPosition = targetPos + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
