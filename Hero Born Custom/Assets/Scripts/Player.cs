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
    public float moveSpeed = 5f; // Speed for player movement
    public float rotationSmoothSpeed = 10f; // Smoothing factor for player rotation
    public float jumpForce = 5f; // Force applied for jumping

    private float yaw = 0f; // Horizontal rotation angle
    private float pitch = 0f; // Vertical rotation angle
    private Rigidbody rb; // Reference to the Rigidbody
    private bool isGrounded = true; // Tracks whether the player is grounded

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned.");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned to the player object.");
        }
    }

    void Update()
    {
        HandleCamera();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down arrow

        // Calculate movement direction
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection = moveDirection.normalized * moveSpeed;

        // Apply velocity to Rigidbody
        Vector3 newVelocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        rb.velocity = newVelocity;

        // Jump when spacebar is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Set grounded to false until collision detection confirms landing
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

        // Rotate the player smoothly based on yaw
        Quaternion targetRotation = Quaternion.Euler(0, yaw, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);

        // Calculate the camera's position and rotation
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = transform.position + cameraRotation * cameraOffset;

        // Smoothly move the camera
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed);
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f); // Look slightly above the player
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Player is grounded
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Player is in the air
        }
    }
}
