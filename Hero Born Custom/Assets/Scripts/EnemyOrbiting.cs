using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySphericalOrbit : MonoBehaviour
{
    public bool reverseDirection = false;
    public float orbitRadius = 5f;
    public float orbitSpeedTheta = 1f; // Horizontal rotation speed
    public float orbitSpeedPhi = 0.5f;   // Vertical rotation speed

    private Vector3 _startLocalPosition;
    private float initialTheta;
    private float initialPhi;

    void Start()
    {
        _startLocalPosition = transform.localPosition;
        // Randomize initial offsets for variety among objects
        initialTheta = Random.Range(0f, Mathf.PI * 2f);
        initialPhi = Random.Range(0f, Mathf.PI);
    }

    void Update()
    {
        float direction = reverseDirection ? -1f : 1f;
        float theta = initialTheta + Time.time * orbitSpeedTheta * direction;
        float phi = initialPhi + Time.time * orbitSpeedPhi * direction;

        float x = orbitRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = orbitRadius * Mathf.Cos(phi);
        float z = orbitRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

        transform.localPosition = _startLocalPosition + new Vector3(x, y, z);
    }
}
