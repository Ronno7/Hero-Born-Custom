using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("Target and Offset")]
    public Transform target; // Reference to the player.
    public Vector3 pivotOffset = new Vector3(0f, 1.2f, 0f);

    [Header("Zoom Settings")]
    public float distance = 5f;       // Initial zoom distance.
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float zoomSpeed = 5f;

    [Header("Collision Settings")]
    public LayerMask collisionLayers; // Layers that block the camera.

    [Header("Rotation Speeds")]
    public float rightMouseXSpeed = 200f;
    public float rightMouseYSpeed = 200f;
    public float leftMouseXSpeed = 100f;
    public float leftMouseYSpeed = 100f;

    [Header("Auto-Align Settings")]
    public float autoAlignSpeed = 5f;

    [Header("Pitch Limits")]
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private float orbitX; // Horizontal orbit angle.
    private float orbitY; // Vertical orbit (pitch) angle.

    private void Start()
    {
        // Auto-find the player if not assigned.
        if (target == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
        orbitX = (target != null) ? target.eulerAngles.y : 0f;
        orbitY = 20f; // Default pitch.
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        UpdateCursorState();
        UpdateOrbit();
        UpdateZoom();
        UpdateCameraPosition();
    }

    // Updates cursor lock and visibility based on right mouse button.
    private void UpdateCursorState()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Updates the camera orbit angles based on mouse input.
    private void UpdateOrbit()
    {
        bool rightMouse = Input.GetMouseButton(1);
        bool leftMouse = Input.GetMouseButton(0);

        if (rightMouse && !leftMouse)
        {
            orbitX += Input.GetAxis("Mouse X") * rightMouseXSpeed * Time.deltaTime;
            orbitY -= Input.GetAxis("Mouse Y") * rightMouseYSpeed * Time.deltaTime;
            orbitY = ClampAngle(orbitY, yMinLimit, yMaxLimit);
            target.rotation = Quaternion.Euler(0, orbitX, 0);
        }
        else if (leftMouse && !rightMouse)
        {
            orbitX += Input.GetAxis("Mouse X") * leftMouseXSpeed * Time.deltaTime;
            orbitY -= Input.GetAxis("Mouse Y") * leftMouseYSpeed * Time.deltaTime;
            orbitY = ClampAngle(orbitY, yMinLimit, yMaxLimit);
        }
        else
        {
            // Smoothly auto-align orbit with the player's current facing.
            float desiredOrbitX = target.eulerAngles.y;
            orbitX = Mathf.LerpAngle(orbitX, desiredOrbitX, autoAlignSpeed * Time.deltaTime);
        }
    }

    // Updates the camera's zoom based on the scroll wheel.
    private void UpdateZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);
    }

    // Calculates and sets the camera's position, accounting for collisions.
    private void UpdateCameraPosition()
    {
        Vector3 pivot = target.position + pivotOffset;
        Quaternion rotation = Quaternion.Euler(orbitY, orbitX, 0);
        Vector3 desiredCameraPos = pivot - rotation * Vector3.forward * distance;
        Vector3 correctedCameraPos = desiredCameraPos;

        if (Physics.Raycast(pivot, (desiredCameraPos - pivot).normalized, out RaycastHit hit, distance, collisionLayers))
        {
            correctedCameraPos = hit.point + hit.normal * 0.2f;
        }

        transform.position = correctedCameraPos;
        transform.LookAt(pivot);
    }

    // Clamps an angle between specified minimum and maximum limits.
    private float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360f)
            angle += 360f;
        while (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
