using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform target; // Reference to the player.
    public Vector3 pivotOffset = new Vector3(0f, 1.2f, 0f);
    public float distance = 5f;       // Initial zoom distance.
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float zoomSpeed = 5f;

    // Rotation speeds (tweak as needed).
    public float rightMouseXSpeed = 200f;
    public float rightMouseYSpeed = 200f;
    public float leftMouseXSpeed = 100f;
    public float leftMouseYSpeed = 100f;

    // Speed for auto-aligning the camera orbit with the player.
    public float autoAlignSpeed = 5f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private float orbitX; // Horizontal orbit angle.
    private float orbitY; // Vertical orbit (pitch) angle.

    void Start()
    {
        // Auto-find the player if not assigned.
        if (target == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
        // Initialize the orbit so the camera is behind the player.
        if (target != null)
            orbitX = target.eulerAngles.y;
        else
            orbitX = 0f;

        orbitY = 20f; // Default pitch.
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Lock and hide the cursor while holding the right mouse button.
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

        bool rightMouse = Input.GetMouseButton(1);
        bool leftMouse = Input.GetMouseButton(0);

        if (rightMouse && !leftMouse)
        {
            // Right drag: update orbit and rotate the player.
            orbitX += Input.GetAxis("Mouse X") * rightMouseXSpeed * Time.deltaTime;
            orbitY -= Input.GetAxis("Mouse Y") * rightMouseYSpeed * Time.deltaTime;
            orbitY = ClampAngle(orbitY, yMinLimit, yMaxLimit);

            // Set the player's facing to match the camera's horizontal angle.
            target.rotation = Quaternion.Euler(0, orbitX, 0);
        }
        else if (leftMouse && !rightMouse)
        {
            // Left drag: update orbit only, leaving player's facing unchanged.
            orbitX += Input.GetAxis("Mouse X") * leftMouseXSpeed * Time.deltaTime;
            orbitY -= Input.GetAxis("Mouse Y") * leftMouseYSpeed * Time.deltaTime;
            orbitY = ClampAngle(orbitY, yMinLimit, yMaxLimit);
        }
        else
        {
            // No button pressed: smoothly align orbit to the player's current rotation.
            float desiredOrbitX = target.eulerAngles.y;
            orbitX = Mathf.LerpAngle(orbitX, desiredOrbitX, autoAlignSpeed * Time.deltaTime);
        }

        // Zoom in/out using the scroll wheel.
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);

        // Calculate the pivot (player's position plus offset).
        Vector3 pivot = target.position + pivotOffset;
        // Create a rotation from the current orbit angles.
        Quaternion rotation = Quaternion.Euler(orbitY, orbitX, 0);
        // Calculate the camera's position relative to the pivot.
        Vector3 camPos = pivot - rotation * Vector3.forward * distance;

        transform.position = camPos;
        transform.LookAt(pivot);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360f)
            angle += 360f;
        while (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
