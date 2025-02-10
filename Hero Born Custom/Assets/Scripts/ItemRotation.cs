using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    public float RotationSpeed = 100f;
    public Vector3 RotationAxis = Vector3.up;  // Default to Y-axis rotation
    public bool ReverseRotation = false;  // Option to reverse rotation direction

    void Update()
    {
        float direction = ReverseRotation ? -1f : 1f; // Reverse if enabled
        transform.Rotate(RotationAxis * RotationSpeed * direction * Time.deltaTime);
    }
}
