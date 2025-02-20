using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemBehavior : MonoBehaviour
{
    public int healthIncrease = 50;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();
            if (player != null)
            {
                player.IncreaseHealth(healthIncrease);
            }
            Destroy(gameObject);
        }
    }
}
