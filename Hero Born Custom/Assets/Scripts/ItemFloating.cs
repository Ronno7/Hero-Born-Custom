using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloating : MonoBehaviour
{
    public float FloatSpeed = 1f;
    public float FloatAmplitude = 0.5f;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        float newY = _startPosition.y + Mathf.Sin(Time.time * FloatSpeed) * FloatAmplitude;
        transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
    }
}
