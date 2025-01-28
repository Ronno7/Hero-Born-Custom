using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera transform
    public Vector3 cameraOffset = new Vector3(0, 5, -10); // Offset for the camera
    public float cameraSmoothSpeed = 0.125f; // Smoothing factor for camera movement
    public float rotationSpeed = 5f; // Speed of rotation for the player and camera
    public float pitchMin = -30f; // Minimum vertical angle
    public float pitchMax = 60f; // Maximum vertical angle

    private float yaw = 0f; // Horizontal rotation angle
    private float pitch = 0f; // Vertical rotation angle

    void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned.");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();
    }

    private void HandleMovement()
    {
        // Movement input
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * 0.04f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * 0.04f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * 0.02f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * 0.02f;
        }
    }

    private void HandleCamera()
    {
        // Mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY; // Inverted pitch control
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax); // Clamp vertical rotation

        // Rotate the player based on the yaw
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        // Calculate the camera's position and rotation
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = transform.position + cameraRotation * cameraOffset;

        // Smoothly move the camera
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed);
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f); // Look slightly above the player
    }
}
