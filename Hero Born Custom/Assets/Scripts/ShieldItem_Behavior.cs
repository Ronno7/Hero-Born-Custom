using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItemBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();
            if (player != null)
            {
                player.ActivateShield();
                Destroy(gameObject);
                Debug.Log("Shield Item Collected - Shield Activated!");
            }
        }
    }
}
