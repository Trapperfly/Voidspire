using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBullet : MonoBehaviour
{
    public Collider2D shipCollider;
    private void Awake()
    {
        if (shipCollider != null)
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shipCollider, true);
    }
    public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision.name);
    }
}
