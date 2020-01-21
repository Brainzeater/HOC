using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A tiny script for a smooth camera follow effect.
public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 7f;
    public Vector3 offset;

    public float xAxisMin;
    public float xAxisMax;
    public float yAxisMin;
    public float yAxisMax;

    void LateUpdate()
    {
        Vector3 boundedPosition = new Vector3(
            Mathf.Clamp(target.position.x, xAxisMin, xAxisMax),
            Mathf.Clamp(target.position.y, yAxisMin, yAxisMax),
            0);
        Vector3 desiredPosition = boundedPosition + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}