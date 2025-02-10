using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItemBehavior : MonoBehaviour
{
    private float speedMultiplier = 1.5f;
    private float boostDuration = 5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehavior>()?.ActivateSpeedBoost(speedMultiplier, boostDuration);
            Destroy(this.gameObject);
            Debug.Log("Speed Item collected - Speed Boost Activated!");
        }
    }
}
