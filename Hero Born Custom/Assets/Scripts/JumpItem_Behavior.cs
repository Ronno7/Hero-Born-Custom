using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItemBehavior : MonoBehaviour
{
    private float jumpMultiplier = 2f;
    private float boostDuration = 5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehavior>()?.ActivateJumpBoost(jumpMultiplier, boostDuration);
            Destroy(this.gameObject);
            Debug.Log("Jump Item collected - Jump Boost Activated!");
        }
    }
}
