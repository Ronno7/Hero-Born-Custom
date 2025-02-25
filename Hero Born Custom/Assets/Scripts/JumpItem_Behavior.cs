using System.Collections;
using UnityEngine;

public class JumpItemBehavior : MonoBehaviour
{
    public float jumpMultiplier = 1.5f;
    public float boostDuration = 5f;
    public float respawnTime = 60f;  // Time before the item reappears

    private Renderer itemRenderer;
    private Collider itemCollider;
    private Rigidbody rb;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Awake()
    {
        itemRenderer = GetComponent<Renderer>();
        itemCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        // Store the original spawn position and rotation.
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehavior>()?.ActivateJumpBoost(jumpMultiplier, boostDuration);
            StartCoroutine(DisappearAndRespawn());
            Debug.Log("Jump Item collected - Jump Boost Activated!");
        }
    }

    private IEnumerator DisappearAndRespawn()
    {
        HideItem();
        yield return new WaitForSeconds(respawnTime);
        RespawnItem();
    }

    // Hides the item and resets physics.
    private void HideItem()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (itemRenderer != null)
            itemRenderer.enabled = false;
        if (itemCollider != null)
            itemCollider.enabled = false;
    }

    // Respawns the item, resetting its transform and physics.
    private void RespawnItem()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep(); // Ensure the rigidbody is not active.
        }
        if (itemRenderer != null)
            itemRenderer.enabled = true;
        if (itemCollider != null)
            itemCollider.enabled = true;
    }
}
