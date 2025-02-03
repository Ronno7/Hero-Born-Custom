using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(this.transform.gameObject);
            Debug.Log("+50 HP collected");
        }
    }
}
