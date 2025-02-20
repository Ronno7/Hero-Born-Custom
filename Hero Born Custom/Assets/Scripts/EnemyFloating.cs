using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloating : MonoBehaviour
{
    public enum FloatAxis { X, Y, Z }
    public FloatAxis floatAxis = FloatAxis.Y;
    public bool reverseDirection = false;
    public float FloatSpeed = 1f;
    public float FloatAmplitude = 0.5f;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * FloatSpeed) * FloatAmplitude;
        if (reverseDirection)
            offset = -offset;

        Vector3 newPos = _startPosition;
        switch (floatAxis)
        {
            case FloatAxis.X:
                newPos.x += offset;
                break;
            case FloatAxis.Y:
                newPos.y += offset;
                break;
            case FloatAxis.Z:
                newPos.z += offset;
                break;
        }
        transform.localPosition = newPos;
    }
}
