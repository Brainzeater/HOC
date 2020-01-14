using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A tiny script for a smooth camera follow effect.
public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 7f;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}